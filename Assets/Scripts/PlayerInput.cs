using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    [SerializeField] private Card selectedCard;

    public void SetSelectedCard(Card card)
    {
        selectedCard = card;
    }

    public void RemoveSelectedCard() => SetSelectedCard(null);

    public void Update()
    {
        if (selectedCard == null)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RemoveSelectedCard();
            return;
        }

        print(selectedCard.gameObject.name);
    }
}
