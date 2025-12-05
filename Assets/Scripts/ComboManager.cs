using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController Instance;

    [Header("Combo Settings")]
    public float comboResetTime = 1.0f;       // Time allowed between duck clicks
    public float spawnRateMin = 0.25f;        // Lowest spawn interval allowed
    public float spawnRateDecrease = 0.1f;    // How much each combo speeds up spawns

    private float comboTimer = 0f;
    private int comboCount = 0;

    // UI event
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

        // Notify UI
        OnComboChanged?.Invoke(comboCount);

        // Adjust spawn rate
        var spawner = FindAnyObjectByType<DuckSpawner>();
        if (spawner != null)
        {
            float newRate = Mathf.Max(spawner.CurrentSpawnRate - spawnRateDecrease, spawnRateMin);
            spawner.SetSpawnRate(newRate);
        }
    }
}
