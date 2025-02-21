using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    [SerializeField] private Card selectedCard;
    [SerializeField] private GameObject targetSelectorArrow;

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
    }

    private void PointArrowFromSelectedCardToMouse()
    {
        Vector3 targetPosition = Input.mousePosition;
        Vector3 objectPosition = targetSelectorArrow.transform.position;

        Vector3 direction = targetPosition - objectPosition;
        float magnitude = Vector3.Magnitude(targetPosition - objectPosition);
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        targetSelectorArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        var arrowsRectTransform = (targetSelectorArrow.transform as RectTransform);
        arrowsRectTransform.sizeDelta = new Vector2(magnitude, arrowsRectTransform.sizeDelta.y);
    }
}
