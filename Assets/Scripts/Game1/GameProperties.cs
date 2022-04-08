using UnityEngine;

[CreateAssetMenu()]
public class GameProperties : ScriptableObject
{
    [Header("Game Properties")]

    public string gameVersion;

    [Header("Levels")]

    public LevelProperties[] levelsInGame;
}
