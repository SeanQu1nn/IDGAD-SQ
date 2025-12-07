using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelOverride
{
    public int levelId;      // Level this override applies to
    public Sprite sprite;    // Sprite for this level
}

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    private Dictionary<int, LevelData> levelCache = new Dictionary<int, LevelData>();

    [Header("Default Background for Level 6 and above")]
    public Sprite defaultBackground6Plus; // Drag your default level 6+ sprite here

    [Header("Optional Level Overrides (6+)")]
    public List<LevelOverride> levelOverrides = new List<LevelOverride>();

    // Internal lookup for fast access
    private Dictionary<int, Sprite> overrideLookup = new Dictionary<int, Sprite>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Build fast lookup dictionary for per-level overrides
        foreach (var o in levelOverrides)
        {
            if (!overrideLookup.ContainsKey(o.levelId))
                overrideLookup.Add(o.levelId, o.sprite);
        }
    }

    public LevelData LoadLevel(int levelId)
    {
        if (levelCache.ContainsKey(levelId))
            return levelCache[levelId];

        string fileName = $"level_{levelId:D3}";
        TextAsset levelFile = Resources.Load<TextAsset>($"Data/Levels/{fileName}");

        LevelData levelData;

        if (levelFile != null)
        {
            try
            {
                levelData = JsonUtility.FromJson<LevelData>(levelFile.text);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to parse level {levelId}: {e.Message}");
                levelData = CreateDefaultLevel(levelId);
            }
        }
        else
        {
            Debug.LogWarning($"Level file not found: {fileName}");
            levelData = CreateDefaultLevel(levelId);
        }

        // Apply background for levels 6+ automatically
        if (levelId >= 6)
        {
            // Step 1: Use per-level override if exists
            if (overrideLookup.ContainsKey(levelId))
                levelData.backgroundSprite = overrideLookup[levelId];
            // Step 2: Otherwise, apply default
            else if (defaultBackground6Plus != null)
                levelData.backgroundSprite = defaultBackground6Plus;
            // Step 3: If neither, keep existing LevelData backgroundSprite
        }

        levelCache[levelId] = levelData;
        return levelData;
    }

    public int GetNextLevelId(int currentLevelId)
    {
        int nextLevelId = currentLevelId + 1;
        string fileName = $"level_{nextLevelId:D3}";
        TextAsset levelFile = Resources.Load<TextAsset>($"Data/Levels/{fileName}");
        return levelFile != null ? nextLevelId : -1;
    }

    private LevelData CreateDefaultLevel(int levelId)
    {
        LevelData defaultLevel = new LevelData
        {
            levelId = levelId,
            levelName = $"Default Level {levelId}",
            goodDucks = 3,
            decoyDucks = 1,
            timeLimit = 30f,
            spawnRate = 3.0f,
            duckLifetime = 5.0f,
            decoyPenalty = 3,
            sizeDistribution = new LevelData.SizeDistribution
            {
                large = 0.6f,
                medium = 0.3f,
                small = 0.1f
            },
            specialMechanics = new string[0],
            backgroundMusic = "tutorial_theme",
            difficulty = "normal",
            designNotes = "Default level created due to missing level file",
            targetSuccessRate = 0.8f,
            learningObjective = "Complete the level",
            powerUpsAvailable = false
        };

        // Auto-assign background for level 6+ using same priority
        if (levelId >= 6)
        {
            if (overrideLookup.ContainsKey(levelId))
                defaultLevel.backgroundSprite = overrideLookup[levelId];
            else if (defaultBackground6Plus != null)
                defaultLevel.backgroundSprite = defaultBackground6Plus;
        }

        return defaultLevel;
    }

    public void ClearCache()
    {
        levelCache.Clear();
    }
}
