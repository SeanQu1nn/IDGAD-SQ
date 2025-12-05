using UnityEngine;
using TMPro;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;

    private void Start()
    {
        ComboController.Instance.OnComboChanged += UpdateCombo;
        comboText.text = "";
    }

    private void OnDestroy()
    {
        if (ComboController.Instance != null)
            ComboController.Instance.OnComboChanged -= UpdateCombo;
    }

    private void UpdateCombo(int combo)
    {
        if (combo <= 1)
            comboText.text = "";
        else
            comboText.text = $"Combo x{combo}";
    }
}
