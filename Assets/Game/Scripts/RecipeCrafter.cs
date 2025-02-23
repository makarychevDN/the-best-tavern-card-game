using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCrafter : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipies;
    [SerializeField] private CardItem lemon;
    private BattleField battleField;

    [SerializedDictionary("Color", "CardItem")]
    public SerializedDictionary<Color, CardItem> cardItemsByColor;

    public void Init(BattleField battleField)
    {
        this.battleField = battleField;
        foreach(var cardSlot in battleField.CardSlots)
        {
            cardSlot.OnFilled.AddListener(TryToExecuteRecepies);
        }
        print(cardItemsByColor[recipies[0].RecipeImage.GetPixel(0, 0)]);
    }

    private void TryToExecuteRecepies()
    {
        int iterations = 0;
        foreach(var recipe in recipies)
        {
            for (int i = 0; i < battleField.CardSlots.GetLength(0); i++)
            {
                for (int j = 0; j < battleField.CardSlots.GetLength(1); j++)
                {
                    iterations++;
                    TryToExecuteRecepie(recipe, i, j);
                }
            }
        }
        print(iterations);
    }

    private void TryToExecuteRecepie(Recipe recipe, int x, int y)
    {
        if (battleField.CardSlots.GetLength(0) - x < recipe.RecipeImage.width)
            return;

        if (battleField.CardSlots.GetLength(1) - y < recipe.RecipeImage.height)
            return;

        for (int i = 0; i < recipe.RecipeImage.width; i++)
        {
            for (int j = 0; j < recipe.RecipeImage.height; j++)
            {
                var expectedItem = cardItemsByColor[recipe.RecipeImage.GetPixel(i, j)];
                if (expectedItem != battleField.CardSlots[i + x, j + y].Card.CardItem)
                    return;
            }
        }

        print($"{recipe.name} is crafted successfully!");
    }
}
