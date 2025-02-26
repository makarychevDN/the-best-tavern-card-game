using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RecipeCrafter : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes;
    private BattleField battleField;

    [SerializedDictionary("Color", "CardItem")]
    public SerializedDictionary<Color, CardItem> cardItemsByColor;

    public void Init(BattleField battleField)
    {
        this.battleField = battleField;
        foreach(var cardSlot in battleField.CardSlots)
        {
            cardSlot.OnSlotFilled.AddListener(TryToExecuteRecipes);
        }
    }

    private void TryToExecuteRecipes(CardSlot targetSlot)
    {
        foreach(var recipe in recipes)
        {
            for (int i = 0; i < battleField.CardSlots.GetLength(0); i++)
            {
                for (int j = 0; j < battleField.CardSlots.GetLength(1); j++)
                {
                    TryToExecuteRecipe(recipe, targetSlot, i, j);
                }
            }
        }
    }

    private void TryToExecuteRecipe(Recipe recipe, CardSlot targetSlot, int x, int y)
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

        ExecuteRecipe(cards, targetSlot, recipe);
    }

    private async void ExecuteRecipe(List<Card> involvedCards, CardSlot targetSlot, Recipe recipe)
    {
        List<Task> animationTasks = new List<Task>();
        foreach (Card card in involvedCards)
        {
            animationTasks.Add(card.EnqueueExecuteRecipe(targetSlot.transform.position));
        }
        await Task.WhenAll(animationTasks);

        battleField.SpawnCard(recipe.ResultItem, targetSlot);
        print($"{recipe.name} is crafted successfully into {targetSlot.name}! The result is {recipe.ResultItem.name}");
    }
}
