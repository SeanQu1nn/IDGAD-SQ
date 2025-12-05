using UnityEngine;

public class ComboScoreListener : MonoBehaviour
{
    private int lastScore = 0;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += HandleScoreChanged;
            lastScore = GameManager.Instance.Score; // If you have a getter
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
        }
    }

    private void HandleScoreChanged(int newScore)
    {
        // Score only increases when a GOOD duck is clicked
        if (newScore > lastScore)
        {
            if (ComboController.Instance != null)
                ComboController.Instance.OnDuckClicked();
        }

        lastScore = newScore;
    }
}
