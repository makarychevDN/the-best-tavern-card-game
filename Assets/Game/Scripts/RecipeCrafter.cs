using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RecipeCrafter : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private ColorToCardItemDictionary cardItemsByColorDictionary;
    private BattleField battleField;


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
        foreach (var recipeTexture in recipe.RecipeImages)
        {
            if (battleField.CardSlots.GetLength(0) - x < recipeTexture.width)
                return;

            if (battleField.CardSlots.GetLength(1) - y < recipeTexture.height)
                return;

            List<Card> cards = new List<Card>();

            for (int i = 0; i < recipeTexture.width; i++)
            {
                for (int j = 0; j < recipeTexture.height; j++)
                {
                    if (recipeTexture.GetPixel(i, j).a == 0)
                        continue;

                    var card = battleField.CardSlots[i + x, j + y].Card;
                    var expectedItem = cardItemsByColorDictionary.CardItemsByColor[recipeTexture.GetPixel(i, j)];

                    if (card == null || expectedItem != battleField.CardSlots[i + x, j + y].Card.CardItem)
                    {
                        return;
                    }

                    cards.Add(card);
                }
            }

            ExecuteRecipe(cards, targetSlot, recipe);
        }
    }

    private async void ExecuteRecipe(List<Card> involvedCards, CardSlot targetSlot, Recipe recipe)
    {
        List<Task> animationTasks = new List<Task>();
        foreach (Card card in involvedCards)
        {
            animationTasks.Add(card.EnqueueExecuteRecipe(targetSlot.transform.position));
        }
        await Task.WhenAll(animationTasks);

        battleField.Level.SpawnCard(recipe.ResultItem, targetSlot);
        print($"{recipe.name} is crafted successfully into {targetSlot.name}! The result is {recipe.ResultItem.name}");
    }
}
