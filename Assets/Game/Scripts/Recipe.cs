using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private CardItem resultItem;
    [SerializeField] private List<Texture2D> recipeImages;

    public List<Texture2D> RecipeImages => recipeImages;

    public CardItem ResultItem => resultItem;
}
