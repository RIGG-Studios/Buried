using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameProperties gameProps;

    public Level currentLevel { get; private set; }
    public Level[] levels { get; private set; }

    public Game game
    {
        get
        {
            return FindObjectOfType<Game>();
        }
    }

    public GameProperties gameProperties
    {
        get
        {
            return gameProps;
        }
    }

    private int currentLevelIndex = 0;
    private bool fade;
    private CanvasManager sceneUI = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
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

        sceneUI = GetComponentInChildren<CanvasManager>();
        sceneUI.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(false);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void LoadDeathScene()
    {
        FadeIn(.1f);
        StartCoroutine(DelayLoadLevel(0.75f, 6));
    }

    public void LoadLevel(Level properties)
    {
        currentLevel = properties;
        SceneManager.LoadScene(properties.properties.levelIndex, LoadSceneMode.Single);
    }

    public void LoadNextLevelScene(int dir)
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex -1 + dir;

        if (currentLevelIndex >= levels.Length)
        {
            StartCoroutine(DelayLoadLevel(2f, 7));
            return;
        }

        currentLevel = levels[currentLevelIndex];
        currentLevel.unlocked = true;
        StartCoroutine(DelayLoadLevel(3.75f));
    }    

    private void OnLevelLoaded(Scene thisscene, LoadSceneMode Single)
    {
        if (game == null)
            return;

        if (fade)
        {
            sceneUI.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(false);
            fade = false;
        }

        game.SetGameState(GameStates.Playing);
    }

    private IEnumerator DelayLoadLevel(float time)
    {
        yield return new WaitForSeconds(time);
        currentLevel.properties.LoadLevel();
    }

    private IEnumerator DelayLoadLevel(float time, int index)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            PlayerPrefs.SetInt("level" + i + "unlocked", levels[i].unlocked ? 1 : 0);
        }
    }

    public void FadeOut(float dur)
    {
        sceneUI.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(true);
        sceneUI.FindElementGroupByID("FadeGroup").UpdateElements(1, 0, true);
        sceneUI.FindElementGroupByID("FadeGroup").UpdateElements(0, dur, true);

        StartCoroutine(Fade(dur));
    }

    public void FadeIn(float dur)
    {
        sceneUI.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(true);
        sceneUI.FindElementGroupByID("FadeGroup").UpdateElements(0, 0, true);
        sceneUI.FindElementGroupByID("FadeGroup").UpdateElements(1, dur, true);
        fade = true;
    }

    private IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);
        sceneUI.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(false);
        fade = false;
    }

    public void LoadMainMenu() => SceneManager.LoadScene(0);
    public void ExitGame() => Application.Quit();

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
