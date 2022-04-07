using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum GameStates
{
    Entering,
    Playing,
    Exiting
}

public class Game : MonoBehaviour
{
    [SerializeField] private CanvasManager gameCanvas = null;
    [SerializeField] private Transform spawnPoint = null;

    public GameStates state { get; private set; }
    public LevelProperties currentLevelProperties { get; private set; }
    public TentacleManager tentacleManager { get; private set; }
    public Player player { get; private set; }
    public CanvasManager canvas { get { return gameCanvas != null ? gameCanvas : null; } }

    private PlayerCamera playerCam = null;
    private GameUI gameUI = null;
    private float timeSinceStart = 0;
    private int generatorsEnabled = 0;


    private void Awake()
    {
        playerCam = FindObjectOfType<PlayerCamera>();
        gameUI = FindObjectOfType<GameUI>();

        this.currentLevelProperties = GameManager.instance.currentLevel;
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
                ExitGame(player.stateManager.GetStateInEnum(player.stateManager.currentState) != PlayerStates.Dead);
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
        gameCanvas.FindElementGroupByID("FadeGroup").UpdateElements(2.0f, 0.0f, false);
        StartCoroutine(FadeOut(2f));
    }

    private void ExitGame(bool lost)
    {
        Destroy(player.gameObject);
        tentacleManager.enabled = false;
        
        GameEvents.OnEndGame?.Invoke(lost, currentLevelProperties.levelName, timeSinceStart, generatorsEnabled);
    }

    private IEnumerator FadeOut(float time)
    {
        playerCam.SetTarget(spawnPoint);
        gameUI.SetIntroUI(currentLevelProperties, time);
        yield return new WaitForSeconds(time + 2.5f);
        gameUI.ResetIntroUI();
        StartGame();
    }

    private void StartGame()
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

    public void ExitToMenu() => GameManager.instance.LoadMainMenu();

    public void ExitToDesktop() => Application.Quit();

    public void ContinueToNextLevel() => GameManager.instance.LoadNextLevelScene(1);

    public void ResetLevel() => GameManager.instance.currentLevel.LoadLevel();
}
