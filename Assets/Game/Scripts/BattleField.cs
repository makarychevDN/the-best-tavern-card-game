using UnityEngine;

public class BattleField : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private CardSlot cardSlotPrefab;
    [SerializeField] private RecipeCrafter cecipeCrafter;
    [SerializeField] private Transform cardSlotsParent;
    [SerializeField] private int width;
    [SerializeField] private int height;
    private CardSlot[,] cardSlots;
    private Level level;

    public CardSlot[,] CardSlots => cardSlots;

    public void Init(Level level)
    {
        this.level = level;

        cardSlots = new CardSlot[width, height];
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                cardSlots[i, j] = Instantiate(cardSlotPrefab, cardSlotsParent);
                cardSlots[i, j].Init(level);
            }
        }

        cecipeCrafter.Init(this);
    }

    public void SpawnCard(CardItem cardItem, CardSlot slot)
    {
        var card = Instantiate(cardPrefab, transform);
        card.transform.position = slot.transform.position;
        card.Init(level, cardItem);
    }
}
