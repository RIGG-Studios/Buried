using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameProperties gameProps;
    public GameProperties gameProperties { get { return gameProps; } }
    public LevelProperties currentLevel { get; private set; }

    public Game game { get { return FindObjectOfType<Game>(); } }

    private int currentLevelIndex = 0;
    private LevelProperties[] levels;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        levels = gameProps.levelsInGame;
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void LoadDeathScene()
    {
        Destroy(game.player.gameObject);
        SceneManager.LoadScene(3);
    }

    public void QuitToDesktop() => Application.Quit();
    
    public void LoadMainMenu() => SceneManager.LoadScene(0);

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
        if (game == null)
            return;

        if(game.player != null)
        {
            Destroy(game.player);
        }

        game.SetGameState(GameStates.Playing);
    }
}
