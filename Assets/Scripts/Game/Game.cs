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
    public Player player { get; private set; }
    public GameStates gameState { get; private set; }

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();
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
        GameEvents.OnStartGame?.Invoke();
        player.StartGame();
    }

    private void Loading()
    {

    }
}
