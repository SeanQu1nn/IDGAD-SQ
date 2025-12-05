using UnityEngine;
using UnityEngine.EventSystems;

public class DuckComboNotifier : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ComboController.Instance != null)
            ComboController.Instance.OnDuckClicked();

        Destroy(gameObject);
    }
}
