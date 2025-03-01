using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Stats")]
    [SerializeField] private int spawnedCardsPerTurnCount;
    [SerializeField] private int spawnedCustomerCardsPerTurnCount;
    [Header("References")]
    [SerializeField] private LastHoveredTargetContainer lastHoveredTargetContainer;
    [SerializeField] private PlayerActionsManager playerInput;
    [SerializeField] private BattleField battleField;
    [SerializeField] private AnimatedActionsExecutor executor;
    [Header("Spawn Cards System")]
    [SerializeField] private Transform spawnedCardSlotsParent;
    [SerializeField] private CardSlot cardSlotPrefab;
    [SerializeField] private List<CardSlot> cardSlotsToSpawnCards;
    [SerializeField] private List<Card> spawnedCards;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private CardItem lemon;
    private List<Card> cards;

    public LastHoveredTargetContainer LastHoveredTargetContainer => lastHoveredTargetContainer;
    public PlayerActionsManager PlayerInput => playerInput;
    public AnimatedActionsExecutor Executor => executor;

    private async void Awake()
    {
        battleField.Init(this);
        playerInput.Init(this);
        playerInput.OnTurnEnded.AddListener(OnTurnEndedHandler);

        SpawnCardSlotsToSpawnedCards(spawnedCardSlotsParent);
        await ExecuteWaveOfCards();
    }

    private void SpawnCardSlotsToSpawnedCards(Transform spawnedCardSlotsParent)
    {
        while(spawnedCardSlotsParent.childCount < spawnedCardsPerTurnCount)
        {
            var spawnedSlot = Instantiate(cardSlotPrefab, spawnedCardSlotsParent);
            spawnedSlot.Init(this);
            cardSlotsToSpawnCards.Add(spawnedSlot);
        }
    }

    private async void OnTurnEndedHandler() => await ExecuteWaveOfCards();

    private async Task ExecuteWaveOfCards()
    {
        if (spawnedCards.Count == 0)
            SpawnCards();

        await MoveSpawnedCardsOnBattleField();
        SpawnCards();

    }

    private void SpawnCards()
    {
        foreach(var slot in cardSlotsToSpawnCards) 
        {
            spawnedCards.Add(SpawnCard(lemon, slot));
        }
    }

    private async Task MoveSpawnedCardsOnBattleField()
    {
        foreach (var card in spawnedCards)
        {
            await card.EnqueueMove(battleField.GetRandomEmptySlot());
        }
        spawnedCards.Clear();
    }

    public Card SpawnCard(CardItem cardItem, CardSlot slot)
    {
        var card = Instantiate(cardPrefab, transform);
        card.transform.position = slot.transform.position;
        slot.SetCard(card);
        card.SetSlot(slot);
        card.Init(this, cardItem);
        return card;
    }
}
