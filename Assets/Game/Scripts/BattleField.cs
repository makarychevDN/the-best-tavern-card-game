using UnityEngine;

public class BattleField : MonoBehaviour
{
    [SerializeField] private CardSlot cardSlotPrefab;
    [SerializeField] private RecipeCrafter cecipeCrafter;
    [SerializeField] private Transform cardSlotsParent;
    [SerializeField] private int width;
    [SerializeField] private int height;
    private CardSlot[,] cardSlots;

    public CardSlot[,] CardSlots => cardSlots;

    public void Init(Level level)
    {
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
}
