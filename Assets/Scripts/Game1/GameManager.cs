using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameProperties gameProps;
    public GameProperties gameProperties { get { return gameProps; } }
    public LevelProperties currentLevel { get; private set; }

    private int currentLevelIndex = 0;
    private LevelProperties[] levels;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        levels = gameProps.levelsInGame;
        SceneManager.sceneLoaded += OnLevelLoaded;

        DontDestroyOnLoad(this);
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

        currentLevel.LoadLevel();
    }

    private void OnLevelLoaded(Scene thisscene, LoadSceneMode Single)
    {
        if (Game.instance == null)
            return;

        Game.instance.InitializeGame(currentLevel);
        Game.instance.SetGameState(GameStates.Playing);
    }
}
