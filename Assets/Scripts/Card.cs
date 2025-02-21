using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EnableHighlight(false);
    }

    private void EnableHighlight(bool value)
    {
        print("Highlighted is " + value);
    }

    private void Select()
    {
        print("Selected");
    }
}
