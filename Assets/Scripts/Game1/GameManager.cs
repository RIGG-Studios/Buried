using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameProperties gameProps;


    public GameProperties gameProperties { get { return gameProps; } }
    public Level currentLevel { get; private set; }
    public Level[] levels { get; private set; }
    public Game game { get { return FindObjectOfType<Game>(); } }

    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        List<Level> lvls = new List<Level>();
        for(int i = 0; i < gameProperties.levelsInGame.Length; i++)
        {
            Level lvl = new Level(gameProperties.levelsInGame[i], i == 0 ? true : false);
            lvls.Add(lvl);
        }

        levels = lvls.ToArray();

        for (int i = 1; i < levels.Length; i++)
        {
            int unlocked = PlayerPrefs.GetInt("level" + i + "unlocked");

            levels[i].unlocked = unlocked == 1 ? true : false;
        }

        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void LoadDeathScene()
    {
        Destroy(game.player.gameObject);
        SceneManager.LoadScene(3);
    }
    
    public void LoadMainMenu() => SceneManager.LoadScene(0, LoadSceneMode.Single);
    public void ExitGame() => Application.Quit();

    public void LoadLevel(Level properties)
    {
        currentLevel = properties;
        SceneManager.LoadScene(properties.properties.levelIndex);
    }

    public void LoadNextLevelScene(int index)
    {
        currentLevelIndex += index;

        if(currentLevelIndex >= levels.Length)
        {
            Debug.Log("Game finished!");
            return;
        }

        currentLevel = levels[currentLevelIndex];
        currentLevel.properties.LoadLevel();
    }    

    private void OnLevelLoaded(Scene thisscene, LoadSceneMode Single)
    {
        if (game == null)
            return;

        if(game.player != null)
        {
            Destroy(game.player);
        }

        game.SetGameState(GameStates.Playing);
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            PlayerPrefs.SetInt("level" + i + "unlocked", levels[i].unlocked ? 1 : 0);
        }
    }
}

[System.Serializable]
public class Level
{
    public LevelProperties properties;
    public bool unlocked;

    public Level(LevelProperties properties, bool unlocked)
    {
        this.properties = properties;
        this.unlocked = unlocked;
    }
}
