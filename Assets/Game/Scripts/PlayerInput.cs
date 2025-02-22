using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    [SerializeField] private Card selectedCard;
    [SerializeField] private GameObject targetSelectorArrow;
    private Level level;

    public void Init(Level level)
    {
        this.level = level;
    }

    public void SetSelectedCard(Card card)
    {
        selectedCard = card;
    }

    public void RemoveSelectedCard() => SetSelectedCard(null);

    public void Update()
    {
        if (selectedCard == null)
        {
            targetSelectorArrow.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RemoveSelectedCard();
            return;
        }

        print(selectedCard.gameObject.name);
        targetSelectorArrow.SetActive(true);
        PointArrowFromSelectedCardToMouse();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var cardSlot = level.LastHoveredTargetContainer.LastHoveredTarget as CardSlot;
            if (cardSlot != null)
            {
                selectedCard.Move(cardSlot);
                RemoveSelectedCard();
            }
        }
    }

    private void PointArrowFromSelectedCardToMouse()
    {
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
