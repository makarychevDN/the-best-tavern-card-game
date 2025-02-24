using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCrafter : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipies;
    private BattleField battleField;

    [SerializedDictionary("Color", "CardItem")]
    public SerializedDictionary<Color, CardItem> cardItemsByColor;

    public void Init(BattleField battleField)
    {
        this.battleField = battleField;
        foreach(var cardSlot in battleField.CardSlots)
        {
            cardSlot.OnSlotFilled.AddListener(TryToExecuteRecepies);
        }
    }

    private void TryToExecuteRecepies(CardSlot targetSlot)
    {
        foreach(var recipe in recipies)
        {
            for (int i = 0; i < battleField.CardSlots.GetLength(0); i++)
            {
                for (int j = 0; j < battleField.CardSlots.GetLength(1); j++)
                {
                    TryToExecuteRecepie(recipe, targetSlot, i, j);
                }
            }
        }
    }

    private void TryToExecuteRecepie(Recipe recipe, CardSlot targetSlot, int x, int y)
    {
        if (battleField.CardSlots.GetLength(0) - x < recipe.RecipeImage.width)
            return;

        if (battleField.CardSlots.GetLength(1) - y < recipe.RecipeImage.height)
            return;

        List<Card> cards = new List<Card>();

        for (int i = 0; i < recipe.RecipeImage.width; i++)
        {
            for (int j = 0; j < recipe.RecipeImage.height; j++)
            {
                if (recipe.RecipeImage.GetPixel(i, j).a == 0)
                    continue;

                var card = battleField.CardSlots[i + x, j + y].Card;
                var expectedItem = cardItemsByColor[recipe.RecipeImage.GetPixel(i, j)];

                if (card == null || expectedItem != battleField.CardSlots[i + x, j + y].Card.CardItem)
                {
                    return;
                }

                cards.Add(card);
            }
        }

        ExecuteRecepe(cards, targetSlot, recipe);
    }

    private async void ExecuteRecepe(List<Card> involvedCards, CardSlot targetSlot, Recipe recipe)
    {
        print($"{recipe.name} is crafted successfully into {targetSlot.name}! The result is {recipe.ResultItem.name}");

        foreach (Card card in involvedCards)
        {
            await card.Move(targetSlot.transform.position);
        }

        while(involvedCards.Count > 0)
        {
            var removedCard = involvedCards[0];
            involvedCards.Remove(removedCard);
            Destroy(removedCard.gameObject);
        }

        battleField.SpawnCard(recipe.ResultItem, targetSlot);
    }
}
