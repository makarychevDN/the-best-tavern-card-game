using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<CardSlot> cardSlots;
    [SerializeField] private List<Card> cards;
    [SerializeField] private MonoBehaviour lastHoveredTarget;

    private void Awake()
    {
        cardSlots.ForEach(slot => slot.Init(this));
        cards.ForEach(card => card.Init(this));
    }

    public void SetLastHoveredTarget(MonoBehaviour target)
    {
        lastHoveredTarget = target;
    }
}
