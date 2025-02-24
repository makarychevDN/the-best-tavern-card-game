using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class Customer : CardItem
{
    [SerializeField] private bool burning;
    [SerializeField] private int snobbery;
}
