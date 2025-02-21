using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerEnterHandler
{
    private Level level;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        level.SetLastHoveredTarget(this);
    }
}
