using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "color to card item dictionary", menuName = "Scriptable Objects/Color To Card Item Dictionary")]
public class ColorToCardItemDictionary : ScriptableObject
{
    [SerializedDictionary("Color", "CardItem")]
    public SerializedDictionary<Color, CardItem> cardItemsByColor;

    public SerializedDictionary<Color, CardItem> CardItemsByColor => cardItemsByColor;
}
