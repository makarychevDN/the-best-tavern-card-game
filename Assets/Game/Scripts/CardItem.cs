using UnityEngine;

[CreateAssetMenu(fileName = "Card Item", menuName = "Scriptable Objects/Card Item")]
public class CardItem : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string itemName;
    [SerializeField] private int power;
    [SerializeField] private int charges;

    public Sprite Image => image;
    public string ItemName => itemName;
    public int Power => power;
    public int Charges => charges;
}
