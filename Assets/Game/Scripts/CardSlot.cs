using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Card card;
    private Level level;

    public void SetCard(Card card) => this.card = card;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        level.LastHoveredTargetContainer.SetLastHoveredTarget(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        level.LastHoveredTargetContainer.RemoveLastHoveredTarget();
    }
}
