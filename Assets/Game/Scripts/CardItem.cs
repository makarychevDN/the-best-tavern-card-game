using UnityEngine;

[CreateAssetMenu(fileName = "Card Item", menuName = "Scriptable Objects/Card Item")]
public class CardItem : ScriptableObject
{
    [SerializeField] private Sprite image;

    public Sprite Image => image;
}
