using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private Level level;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHighlight(true);
        level.SetLastHoveredTarget(this);
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
