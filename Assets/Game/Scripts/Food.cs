using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Scriptable Objects/Food")]
public class Food : CardItem
{
    [SerializeField] private bool spicy;
    [SerializeField] private bool sour;
    [SerializeField] private bool sweet;
    [SerializeField] private bool fancy;
}
