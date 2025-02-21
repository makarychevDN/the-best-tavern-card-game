using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<CardSlot> cardSlots;
    [SerializeField] private List<Card> cards;
    [SerializeField] private LastHoveredTargetContainer lastHoveredTargetContainer;
    [SerializeField] private PlayerInput playerInput;

    public LastHoveredTargetContainer LastHoveredTargetContainer => lastHoveredTargetContainer;
    public PlayerInput PlayerInput => playerInput;

    private void Awake()
    {
        cardSlots.ForEach(slot => slot.Init(this));
        cards.ForEach(card => card.Init(this));
    }
}
