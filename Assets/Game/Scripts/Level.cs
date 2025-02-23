using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LastHoveredTargetContainer lastHoveredTargetContainer;
    [SerializeField] private PlayerActionsManager playerInput;
    [SerializeField] private BattleField battleField;
    private List<Card> cards;

    public LastHoveredTargetContainer LastHoveredTargetContainer => lastHoveredTargetContainer;
    public PlayerActionsManager PlayerInput => playerInput;

    private void Awake()
    {
        battleField.Init(this);
        playerInput.Init(this);
        cards = GetComponentsInChildren<Card>().ToList();
        cards.ForEach(card => card.Init(this));
    }
}
