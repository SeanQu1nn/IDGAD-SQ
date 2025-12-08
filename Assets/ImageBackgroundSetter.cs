using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundSetter : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelLoaded += ApplyBackground;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelLoaded -= ApplyBackground;
        }
    }

    private void ApplyBackground(LevelData levelData)
    {
        if (backgroundImage != null && levelData.backgroundSprite != null)
        {
            backgroundImage.sprite = levelData.backgroundSprite;
        }
        else
        {
            Debug.LogWarning("UIBackgroundSetter: Missing Image or Background Sprite");
        }
    }
}
