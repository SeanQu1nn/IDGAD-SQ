using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [Header("Total Levels In Game")]
    public int totalLevels = 12;

    [Header("Default Background for Level 7+")]
    public Sprite defaultBackground7Plus;

    [Header("Level Overrides (Backgrounds)")]
    public List<LevelOverride> levelOverrides = new List<LevelOverride>();

    private Dictionary<int, Sprite> overrideLookup = new Dictionary<int, Sprite>();
    private Dictionary<int, LevelData> levelCache = new Dictionary<int, LevelData>();

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

        // Build override lookup table
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

        // Generate level dynamically (no JSON)
        LevelData level = CreateAutoLevel(levelId);

        // Assign background
        if (levelId >= 7)
        {
            if (overrideLookup.ContainsKey(levelId))
                level.backgroundSprite = overrideLookup[levelId];
            else if (defaultBackground7Plus != null)
                level.backgroundSprite = defaultBackground7Plus;
        }

        levelCache[levelId] = level;
        return level;
    }

    public int GetNextLevelId(int currentLevelId)
    {
        if (currentLevelId >= totalLevels)
            return -1;

        return currentLevelId + 1;
    }

    // Generates levels 1 to 12 with increasing difficulty.
    // Level 1 = 3 ducks, Level 12 = 25 ducks.
    private LevelData CreateAutoLevel(int levelId)
    {
        // Scale good ducks 3 to 25 across 12 levels (this is the WIN condition)
        float progress = (levelId - 1) / 11f;
        int goodDucks = Mathf.RoundToInt(Mathf.Lerp(3, 25, progress));

        // --- EXTRA DUCKS FOR SPAWNING ---
        int extraDucksToSpawn = 2; // Add 2 extra ducks per level
        int totalGoodDucksToSpawn = goodDucks + extraDucksToSpawn;

        LevelData data = new LevelData
        {
            levelId = levelId,
            levelName = "Level " + levelId,

            goodDucks = goodDucks,                     // WIN condition stays the same
            decoyDucks = Mathf.RoundToInt(goodDucks * 0.2f),

            timeLimit = Mathf.Lerp(30f, 20f, progress),
            spawnRate = Mathf.Lerp(2.8f, 1.4f, progress),

            duckLifetime = Mathf.Lerp(5f, 3f, progress),
            decoyPenalty = 3,

            maxTotalSpawns = totalGoodDucksToSpawn,   // SPAWN pool increased
            continueSpawning = true,

            sizeDistribution = new LevelData.SizeDistribution
            {
                large = 0.5f,
                medium = 0.35f,
                small = 0.15f
            },

            specialMechanics = new string[0],
            backgroundMusic = "level_theme",
            difficulty = "normal",
            designNotes = "Auto-generated level",
            targetSuccessRate = 0.8f,
            learningObjective = "Hit all ducks",
            powerUpsAvailable = false
        };

        return data;
    }

    public void ClearCache()
    {
        levelCache.Clear();
    }
}

[System.Serializable]
public class LevelOverride
{
    public int levelId;
    public Sprite sprite;
}
