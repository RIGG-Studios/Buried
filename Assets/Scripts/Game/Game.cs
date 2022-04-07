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
    public static Game instance;

    [SerializeField] private CanvasManager gameCanvas = null;
    [SerializeField] private Transform spawnPoint = null;

    public GameStates state { get; private set; }
    public LevelProperties currentLevelProperties { get; private set; }
    public TentacleManager tentacleManager { get; private set; }
    public Player player { get; private set; }

    public CanvasManager canvas { get { return gameCanvas != null ? gameCanvas : null; } }


    private PlayerCamera playerCam;
    private float timeSinceStart;
    private int generatorsEnabled;


    private void Awake()
    {
        playerCam = FindObjectOfType<PlayerCamera>();
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

    public void ContinueToNextLevel()
    {
        GameManager.instance.LoadNextLevelScene(1);
    }

    public void ResetLevel()
    {
        GameManager.instance.currentLevel.LoadLevel();
    }

    private IEnumerator FadeOut(float time)
    {
        playerCam.SetTarget(spawnPoint);
        gameCanvas.FindElementGroupByID("FadeGroup").FindElement("levelnumbertext").OverrideValue("LEVEL " + currentLevelProperties.levelIndex);
        gameCanvas.FindElementGroupByID("FadeGroup").FindElement("levelnametext").OverrideValue(currentLevelProperties.levelName.ToUpper());
        gameCanvas.FindElementGroupByID("FadeGroup").UpdateElements(0, time, false);
        yield return new WaitForSeconds(time + 2.5f);
        gameCanvas.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(false);
        gameCanvas.FindElementGroupByID("FadeGroup").FindElement("levelnumbertext").SetActive(false);
        gameCanvas.FindElementGroupByID("FadeGroup").FindElement("levelnametext").SetActive(false);
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
}
