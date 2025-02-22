using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    [SerializeField] private LastHoveredTargetContainer lastHoveredTargetContainer;
    [SerializeField] private PlayerInput playerInput;
    private List<CardSlot> cardSlots;

    public LastHoveredTargetContainer LastHoveredTargetContainer => lastHoveredTargetContainer;
    public PlayerInput PlayerInput => playerInput;

    private void Awake()
    {
        playerInput.Init(this);
        cardSlots = GetComponentsInChildren<CardSlot>().ToList();
        cardSlots.ForEach(slot => slot.Init(this));
        cards.ForEach(card => card.Init(this));
    }
}
