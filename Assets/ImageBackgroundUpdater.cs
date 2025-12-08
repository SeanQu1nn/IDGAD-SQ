using UnityEngine;
using UnityEngine.UI;

public class NextLevelBackgroundSwitcher : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite level7AndUpBackground;

    void Start()
    {
        // Auto-assign this button's click if attached to the button
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(ApplyBackgroundIfNeeded);
        }
    }

    private void ApplyBackgroundIfNeeded()
    {
        if (GameManager.Instance == null) return;

        int nextLevel = GameManager.Instance.CurrentLevelId + 1;

        if (nextLevel >= 7)
        {
            if (backgroundImage != null && level7AndUpBackground != null)
            {
                backgroundImage.sprite = level7AndUpBackground;
            }
        }
    }
}
