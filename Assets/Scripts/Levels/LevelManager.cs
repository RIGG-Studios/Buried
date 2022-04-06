using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelProperties[] levels;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static void LoadNextLevel()
    {

    }
}
