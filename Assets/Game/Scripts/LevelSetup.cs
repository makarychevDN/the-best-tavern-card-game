using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level setup", menuName = "Scriptable Objects/Level Setup")]
public class LevelSetup : ScriptableObject
{
    [SerializeField] private List<CardItem> customersDeck;

    public List<CardItem> CustomersDeck => customersDeck;
}
