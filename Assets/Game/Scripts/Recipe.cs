using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private CardItem resultItem;
    [SerializeField] private Texture2D recipeImage;

    public Texture2D RecipeImage => recipeImage;
}
