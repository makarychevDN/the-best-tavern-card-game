using UnityEngine;

[CreateAssetMenu(fileName = "playable character", menuName = "Scriptable Objects/playable character")]
public class PlayableCharacter : ScriptableObject
{
    [SerializeField] private int speed;
    public int Speed => speed;
}
