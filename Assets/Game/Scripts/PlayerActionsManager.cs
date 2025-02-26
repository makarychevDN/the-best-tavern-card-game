using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionsManager: MonoBehaviour
{
    [SerializeField] private Card selectedCard;
    [SerializeField] private GameObject targetSelectorArrow;
    [SerializeField] private InputActionReference primaryButton;
    [SerializeField] private InputActionReference secondaryButton;
    private Level level;

    private void Awake()
    {
        primaryButton.action.started += HandlePrimaryButtonInput;
        secondaryButton.action.started += HandleSecondaryButtonInput;
    }

    public void Update()
    {
        DrawArrow();
    }

    public void Init(Level level)
    {
        this.level = level;
    }

    public void SetSelectedCard(Card card)
    {
        selectedCard = card;
    }

    private void HandlePrimaryButtonInput(InputAction.CallbackContext obj)
    {
        if (level.Executor.IsExecuting)
        {
            Debug.LogError("nonono mr fish");
            return;
        }

        if(selectedCard == null)
        {
            selectedCard = level.LastHoveredTargetContainer.LastHoveredTarget as Card;

            if(selectedCard != null)
                selectedCard.Select();

            return;
        }

        var cardSlot = level.LastHoveredTargetContainer.LastHoveredTarget as CardSlot;
        if (cardSlot != null)
        {
            selectedCard.EnqueueMove(cardSlot);
            SetSelectedCard(null);
            return;
        }

        var card = level.LastHoveredTargetContainer.LastHoveredTarget as Card;
        if (card != null)
        {
            selectedCard.EnqueueInteract(card);
            SetSelectedCard(null);
            return;
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
