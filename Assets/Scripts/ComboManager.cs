using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController Instance;

    [Header("Combo Settings")]
    public float comboResetTime = 1.0f;     // Time allowed between clicks
    public float spawnRateMin = 0.25f;      // Minimum spawn interval
    public float spawnRateDecrease = 0.1f;  // Spawn rate reduction per combo
    public int comboBonusMultiplier = 1;    // How much score bonus each combo gives

    private float comboTimer = 0f;
    private int comboCount = 0;

    public event System.Action<int> OnComboChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (comboCount > 0)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= comboResetTime)
            {
                comboCount = 0;
                comboTimer = 0;
                OnComboChanged?.Invoke(comboCount);
            }
        }
    }

    public void OnDuckClicked()
    {
        comboCount++;
        comboTimer = 0f;

        OnComboChanged?.Invoke(comboCount);

        // Speed up duck spawns
        var spawner = FindAnyObjectByType<DuckSpawner>();
        if (spawner != null)
        {
            float newRate = Mathf.Max(spawner.CurrentSpawnRate - spawnRateDecrease, spawnRateMin);
            spawner.SetSpawnRate(newRate);
        }

        // Score bonus per combo
        if (comboCount > 1)
        {
            var gm = GameManager.Instance;

            if (gm != null)
            {
                // Find AddScore(int) automatically without needing to modify GM
                var addScoreMethod = gm.GetType().GetMethod("AddScore");

                if (addScoreMethod != null)
                {
                    int bonus = comboCount * comboBonusMultiplier;
                    addScoreMethod.Invoke(gm, new object[] { bonus });
                }
                else
                {
                    Debug.LogWarning("ComboController: GameManager has no AddScore(int) method, score bonus skipped.");
                }
            }
        }
    }
}
