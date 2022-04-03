using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Tutorial,
    Playing,
    Loading
}

public class Game : MonoBehaviour
{
    public static Game instance;

    public GameProperties gameProperties;

    [SerializeField] private GameObject tempListener;


    public Player player { get; private set; }
    public TentacleManager tentacleManager { get; private set; }
    public GameStates gameState { get; private set; }

    private GameObject spawnPoint = null;

    private void Awake()
    {
        instance = this;
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        tentacleManager = FindObjectOfType<TentacleManager>();
    }

    private void Start()
    {
        UpdateState(GameStates.Tutorial);
    }

    public void UpdateState(GameStates gameState)
    {
        this.gameState = gameState;

        switch (gameState)
        {
            case GameStates.Tutorial:
                StartTutorial();
                break;

            case GameStates.Playing:
                StartGame();
                break;

            case GameStates.Loading:
                Loading();
                break;
        }
    }

    private void StartTutorial()
    {
        Tutorial.instance.StartTutorial();
    }

    private void StartGame()
    {
        Player player = PlayerSpawner.SpawnPlayer(spawnPoint.transform);

        if(player)
        {
            this.player = player;

            this.player.Initialize();
            tentacleManager.Initialize();
        }

        Destroy(tempListener);
        GameEvents.OnStartGame?.Invoke();
    }

    private void Loading()
    {

    }
}
