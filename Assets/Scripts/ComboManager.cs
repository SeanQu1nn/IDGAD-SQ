using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController Instance;

    [Header("Combo Settings")]
    public float comboResetTime = 1.0f;

    private float comboTimer = 0f;
    private int comboCount = 0;

    private const int MAX_COMBO = 6;

    private DuckSpawner spawner;
    private float baseSpawnRate = 1f;

    public event System.Action<int> OnComboChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        spawner = FindAnyObjectByType<DuckSpawner>();

        if (spawner != null)
        {
            baseSpawnRate = spawner.CurrentSpawnRate;
        }
        else
        {
            Debug.LogWarning("ComboController: No DuckSpawner found in scene.");
        }
    }

    private void Update()
    {
        if (comboCount > 0)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= comboResetTime)
            {
                ResetCombo();
            }
        }
    }

    public void OnDuckClicked()
    {
        comboCount = Mathf.Min(comboCount + 1, MAX_COMBO);
        comboTimer = 0f;

        OnComboChanged?.Invoke(comboCount);

        if (spawner != null)
        {
            float newRate = CalculateSpawnRate(comboCount);
            spawner.SetSpawnRate(newRate);
        }
    }

    private float CalculateSpawnRate(int combo)
    {
        // Each combo gives 15% faster spawn rate
        float modifier = 1f + combo * 0.15f;

        float newRate = baseSpawnRate / modifier;

        // Hard cap: no faster than 2× base speed
        float minRate = baseSpawnRate * 0.5f;

        return Mathf.Max(newRate, minRate);
    }

    private void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0f;

        OnComboChanged?.Invoke(comboCount);

        if (spawner != null)
        {
            spawner.SetSpawnRate(baseSpawnRate);
        }
    }
}
