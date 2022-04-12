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
    [SerializeField] private GameObject tempListener = null;


    public GameStates state { get; private set; }
    public Level currentLevelProperties { get; private set; }
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
        GameManager.instance.FadeOut(2f);
        StartCoroutine(FadeOut(2f));
    }

    private void ExitGame(bool lost)
    {
        Destroy(player.gameObject);
        tentacleManager.enabled = false;
        GameManager.instance.currentLevel.unlocked = true;
        GameEvents.OnEndGame?.Invoke(lost, currentLevelProperties.properties.levelName, timeSinceStart, generatorsEnabled);
    }

    private IEnumerator FadeOut(float time)
    {
        playerCam.SetTarget(spawnPoint);
        gameUI.SetIntroUI(currentLevelProperties.properties, time);
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

        Destroy(tempListener);
        GameEvents.OnStartGame?.Invoke(currentLevelProperties.properties);
    }

    public void ExitToMenu() => GameManager.instance.LoadMainMenu();

    public void ExitToDesktop() => GameManager.instance.ExitGame();

    public void ContinueToNextLevel()
    {
        GameManager.instance.FadeIn(2f);

        GameManager.instance.LoadNextLevelScene(1);
    }

    public void ResetLevel() => GameManager.instance.currentLevel.properties.LoadLevel();
}
