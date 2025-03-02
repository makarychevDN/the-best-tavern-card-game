using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Stats")]
    [SerializeField] private int spawnedCustomerCardsPerTurnCount;
    [SerializeField] private int spawnedProductCardsPerTurnCount;
    [Header("References")]
    [SerializeField] private LastHoveredTargetContainer lastHoveredTargetContainer;
    [SerializeField] private PlayerActionsManager playerInput;
    [SerializeField] private BattleField battleField;
    [SerializeField] private AnimatedActionsExecutor executor;
    [Header("Spawn Cards System")]
    [SerializeField] private Transform spawnedCardSlotsParent;
    [SerializeField] private Transform cardsParent;
    [SerializeField] private Transform selectedCardParent;
    [SerializeField] private LevelSetup levelSetup;
    [SerializeField] private List<CardItem> customersDeck;
    [SerializeField] private List<CardItem> productsDeck;
    [SerializeField] private CardSlot cardSlotPrefab;
    [SerializeField] private List<CardSlot> customerCardSlotsToSpawnCards;
    [SerializeField] private List<CardSlot> productCardSlotsToSpawnCards;
    [SerializeField] private List<Card> cards;
    [SerializeField] private Card cardPrefab;

    public LastHoveredTargetContainer LastHoveredTargetContainer => lastHoveredTargetContainer;
    public PlayerActionsManager PlayerInput => playerInput;
    public AnimatedActionsExecutor Executor => executor;
    public Transform CardsParent => cardsParent;
    public Transform SelectedCardParent => selectedCardParent;

    private async void Awake()
    {
        battleField.Init(this);
        playerInput.Init(this);
        playerInput.OnTurnEnded.AddListener(OnTurnEndedHandler);

        RefreshDeck(customersDeck, levelSetup.CustomersDeck);
        RefreshDeck(productsDeck, playerInput.ProductesDeck);

        SpawnCardSlotsToSpawnedCards(spawnedCardSlotsParent);
        await Task.Delay(1000);
        await ExecuteWaveOfCards();
    }

    private void SpawnCardSlotsToSpawnedCards(Transform spawnedCardSlotsParent)
    {
        SpawnCertainDeckCardSlots(spawnedCustomerCardsPerTurnCount, customerCardSlotsToSpawnCards);
        SpawnCertainDeckCardSlots(spawnedProductCardsPerTurnCount, productCardSlotsToSpawnCards);
    }

    private void SpawnCertainDeckCardSlots(int count, List<CardSlot> targetCardSlots)
    {
        while (targetCardSlots.Count < count)
        {
            var spawnedSlot = Instantiate(cardSlotPrefab, spawnedCardSlotsParent);
            spawnedSlot.Init(this);
            targetCardSlots.Add(spawnedSlot);
        }
    }

    private async void OnTurnEndedHandler() => await ExecuteWaveOfCards();

    private async Task ExecuteWaveOfCards()
    {
        if (cards.Count == 0)
            SpawnCards();

        await MoveSpawnedCardsOnBattleField();
        SpawnCards();

    }

    private void SpawnCards()
    {
        SpawnCertainDeckCards(spawnedCustomerCardsPerTurnCount,
            customersDeck, levelSetup.CustomersDeck,
            cards, customerCardSlotsToSpawnCards);

        SpawnCertainDeckCards(spawnedProductCardsPerTurnCount,
            productsDeck, playerInput.ProductesDeck,
            cards, productCardSlotsToSpawnCards);
    }

    private void SpawnCertainDeckCards(int count, List<CardItem> deck, List<CardItem> sourceDeck, 
        List<Card> spawnedCards, List<CardSlot> cardSlots)
    {
        for (int i = 0; i < count; i++)
        {
            if (deck.Count == 0)
                RefreshDeck(deck, sourceDeck);

            spawnedCards.Add(SpawnCard(deck[0], cardSlots[i]));
            deck.RemoveAt(0);
        }
    }

    private void RefreshDeck(List<CardItem> deck, List<CardItem> source)
    {
        deck.Clear();
        deck.AddRange(source);
        deck.Shuffle();
    }

    private async Task MoveSpawnedCardsOnBattleField()
    {
        foreach (var card in cards)
        {
            await card.EnqueueMove(battleField.GetRandomEmptySlot());
        }
        cards.Clear();
    }

    public Card SpawnCard(CardItem cardItem, CardSlot slot)
    {
        var card = Instantiate(cardPrefab, cardsParent);
        card.transform.position = slot.transform.position;
        slot.SetCard(card);
        card.SetSlot(slot);
        card.Init(this, cardItem);
        return card;
    }
}
