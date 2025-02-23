using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LastHoveredTargetContainer lastHoveredTargetContainer;
    [SerializeField] private PlayerInput playerInput;
    private List<CardSlot> cardSlots;
    private List<Card> cards;

    public LastHoveredTargetContainer LastHoveredTargetContainer => lastHoveredTargetContainer;
    public PlayerInput PlayerInput => playerInput;

    private void Awake()
    {
        playerInput.Init(this);
        cardSlots = GetComponentsInChildren<CardSlot>().ToList();
        cards = GetComponentsInChildren<Card>().ToList();
        cardSlots.ForEach(slot => slot.Init(this));
        cards.ForEach(card => card.Init(this));
    }
}
