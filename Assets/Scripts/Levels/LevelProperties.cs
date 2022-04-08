using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelDifficulty
{
    Easy,
    Medium,
    Hard,
    Difficult
}

[CreateAssetMenu()]
public class LevelProperties : ScriptableObject
{
    public string levelName;
    public Sprite levelIcon;
    [TextArea] public string levelDescription;
    public LevelDifficulty levelDifficulty;

    public List<ItemProperties> startingTools = new List<ItemProperties>();
    public List<ItemProperties> startingItems = new List<ItemProperties>();

    public int maxAttackingTentacles;
    public int generatorsInLevel;
    public int levelIndex;

    public void LoadLevel() => SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
}
