using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActionsManager: MonoBehaviour
{
    [SerializeField] private int currentActionsCounter;
    [SerializeField] private int maxActionsCounterPerTurn;
    [SerializeField] private PlayableCharacter character;
    [SerializeField] private List<CardItem> deck;
    [Header("Selected Cards Setuo")]
    [SerializeField] private Card selectedCard;
    [SerializeField] private GameObject targetSelectorArrow;
    [Header("Key References Setup")]
    [SerializeField] private InputActionReference primaryButton;
    [SerializeField] private InputActionReference secondaryButton;
    private Level level;

    public UnityEvent OnTurnEnded;

    private void Awake()
    {
        primaryButton.action.started += HandlePrimaryButtonInput;
        secondaryButton.action.started += HandleSecondaryButtonInput;
    }

    public void Update()
    {
        DrawArrow();
    }

    public void Init(Level level, PlayableCharacter character)
    {
        this.level = level;
        this.character = character;
        currentActionsCounter = 0;
        maxActionsCounterPerTurn = character.Speed;
    }

    public void Init(Level level)
    {
        this.level = level;
        currentActionsCounter = 0;
        maxActionsCounterPerTurn = character.Speed;
    }

    public void SetSelectedCard(Card card)
    {
        selectedCard = card;
    }

    private void HandlePrimaryButtonInput(InputAction.CallbackContext obj)
    {
        if (level.Executor.IsExecuting)
        {
            return;
        }

        if(selectedCard == null)
        {
            selectedCard = level.LastHoveredTargetContainer.LastHoveredTarget as Card;

            if(selectedCard != null)
                selectedCard.Select();

            return;
        }

        var target = level.LastHoveredTargetContainer.LastHoveredTarget;
        if(level.LastHoveredTargetContainer.LastHoveredTarget != null)
        {
            IncreaseActionsCounter(selectedCard.ActionCost);
            selectedCard.EnqueueAction(target);
            SetSelectedCard(null);
            return;
        }
    }

    private void IncreaseActionsCounter(int value)
    {
        currentActionsCounter += value;

        if(currentActionsCounter >= maxActionsCounterPerTurn)
        {
            currentActionsCounter -= maxActionsCounterPerTurn;
            OnTurnEnded.Invoke();
            print("next turn!");
        }
    }

    private void HandleSecondaryButtonInput(InputAction.CallbackContext obj)
    {
        if (selectedCard == null)
            return;

        selectedCard.Unselect();
        SetSelectedCard(null);
    }

    private void DrawArrow()
    {
        targetSelectorArrow.SetActive(selectedCard != null);

        if (selectedCard == null)
            return;

        Vector3 targetPosition = Input.mousePosition;
        Vector3 objectPosition = selectedCard.transform.position;

        Vector3 direction = targetPosition - objectPosition;
        float magnitude = Vector3.Magnitude(targetPosition - objectPosition);
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        targetSelectorArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        targetSelectorArrow.transform.position = selectedCard.transform.position;
        var arrowsRectTransform = (targetSelectorArrow.transform as RectTransform);
        arrowsRectTransform.sizeDelta = new Vector2(magnitude, arrowsRectTransform.sizeDelta.y);
    }
}
