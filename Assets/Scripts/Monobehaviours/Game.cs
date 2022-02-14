using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance;

    public GameObject startUI;

    public GameObject endUI;
    public MonsterController monster { get; private set; }  
    public Player player { get; private set; }

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<MonsterController>();
    }

    public void StartGame()
    {
        startUI.SetActive(false);

        player.InitializePlayer();
        monster.InitializeMonster();
    }

    public void EndGame()
    {
        player.DisablePlayer();
        startUI.SetActive(false);
        endUI.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GiveFeedBack()
    {
        Application.OpenURL("https://forms.gle/Z5hjJTLKDkeMnGyGA");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
