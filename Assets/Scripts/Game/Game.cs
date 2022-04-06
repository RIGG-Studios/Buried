using UnityEngine;

public enum GameStates
{
    Entering,
    Playing,
    Exiting
}

public class Game : MonoBehaviour
{
    public static Game instance;

    [SerializeField] private Transform spawnPoint;

    public GameStates state { get; private set; }
    public LevelProperties currentLevelProperties { get; private set; }
    public TentacleManager tentacleManager { get; private set; }
    public Player player { get; private set; }

    private float timeSinceStart;
    private int generatorsEnabled;


    private void Awake()
    {
        instance = this;
    }

    public void InitializeGame(LevelProperties currentLevelProperties)
    {
        if (instance == null)
            instance = this;

        this.currentLevelProperties = currentLevelProperties;
        tentacleManager = FindObjectOfType<TentacleManager>();
    }

    private void Update()
    {
        if (state == GameStates.Playing)
        {
            timeSinceStart += Time.deltaTime;
            generatorsEnabled = MainManager.GetRemainingGenerators;
        }
    }

    public void SetGameState(GameStates state)
    {
        if (state == this.state)
            return;

        switch (state)
        {
            case GameStates.Entering:
                EnterGame();
                break;

            case GameStates.Exiting:
                ExitGame(player.stateManager.GetStateInEnum() != PlayerStates.Dead);
                break;

            case GameStates.Playing:
                StartPlayingGame();
                break;
        }

        this.state = state;
    }

    private void EnterGame()
    {
        SetGameState(GameStates.Playing);
    }

    private void StartPlayingGame()
    {
        Player player = PlayerSpawner.SpawnPlayer(spawnPoint.transform);

        if (player)
        {
            this.player = player;

            this.player.Initialize();
            tentacleManager.Initialize();
        }

        GameEvents.OnStartGame?.Invoke(currentLevelProperties);
    }

    private void ExitGame(bool lost)
    {
        Destroy(player.gameObject);
        tentacleManager.enabled = false;
        
        GameEvents.OnEndGame?.Invoke(lost, currentLevelProperties.levelName, timeSinceStart, generatorsEnabled);
    }

    public void ContinueToNextLevel()
    {
        GameManager.instance.LoadNextLevelScene(1);
    }

    public void ResetLevel()
    {
        GameManager.instance.currentLevel.LoadLevel();
    }
}
