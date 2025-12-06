using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController Instance;

    [Header("Combo Settings")]
    public float comboResetTime = 1.0f;

    private float comboTimer = 0f;
    private int comboCount = 0;

    private const int MAX_COMBO = 6;

    private DuckSpawner mainSpawner;
    private DecoyDuckSpawner decoySpawner;

    private float baseMainRate = 1.2f; // slightly slower than before
    private float baseDecoyRate = 3f;

    public event System.Action<int> OnComboChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        mainSpawner = FindAnyObjectByType<DuckSpawner>();
        decoySpawner = FindAnyObjectByType<DecoyDuckSpawner>();

        if (mainSpawner != null)
            baseMainRate = mainSpawner.CurrentSpawnRate;

        if (decoySpawner != null)
            baseDecoyRate = decoySpawner.CurrentSpawnRate;
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

        if (mainSpawner != null)
            mainSpawner.SetSpawnRate(CalcMainRate(comboCount));

        if (decoySpawner != null)
            decoySpawner.SetSpawnRate(CalcDecoyRate(comboCount));
    }

    private float CalcMainRate(int combo)
    {
        // Main ducks: 10% faster per combo level
        float modifier = 1f + combo * 0.10f;
        float newRate = baseMainRate / modifier;
        float minRate = baseMainRate * 0.6f; // never faster than 60% of base
        return Mathf.Max(newRate, minRate);
    }

    private float CalcDecoyRate(int combo)
    {
        // Decoys: increase spawn speed gently
        float modifier = 1f + combo * 0.05f;
        float newRate = baseDecoyRate / modifier;
        float minRate = baseDecoyRate * 0.7f;
        return Mathf.Max(newRate, minRate);
    }

    public void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0f;

        OnComboChanged?.Invoke(comboCount);

        if (mainSpawner != null)
            mainSpawner.SetSpawnRate(baseMainRate);

        if (decoySpawner != null)
            decoySpawner.SetSpawnRate(baseDecoyRate);
    }
}
