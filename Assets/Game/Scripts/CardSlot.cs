using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Card card;
    private Level level;

    public UnityEvent<CardSlot> OnSlotFilled;

    public Card Card => card;

    public bool IsEmpty => card == null;

    public void SetCard(Card card)
    {
        this.card = card;

        if (card != null)
        {
            OnSlotFilled.Invoke(this);
        }
    }

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
