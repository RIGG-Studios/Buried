using UnityEngine;

[CreateAssetMenu()]
public class GameProperties : ScriptableObject
{
    [Header("Game Properties")]

    public string gameVersion;

    [Header("Game Difficulty")]

    [Range(0, 5)] public int maxAttackingTentacles = 5;
}
