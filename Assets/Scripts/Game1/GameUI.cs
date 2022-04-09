using TMPro;
using System.Collections;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI generatorsLeftText;

    private UIElementGroup levelLostGroup;
    private UIElement levelLostName;
    private UIElement levelLostTime;
    private UIElement levelLostBestTime;
    private UIElement levelLostGeneratorsTurnedOn;

    private UIElementGroup levelWonGroup;
    private UIElement levelWonName;
    private UIElement levelWonTime;
    private UIElement levelWonBestTime;
    private UIElement levelWonGeneratorsTurnedOn;

    private UIElement levelObjectiveText;

    private UIElementGroup fadeGroup;
    private UIElementGroup exitGameGroup;

    private Game game { get { return FindObjectOfType<Game>(); } }


    private void Awake()
    {
        levelLostGroup = game.canvas.FindElementGroupByID("LevelLostGroup");

        if(levelLostGroup != null)
        {
            levelLostName = levelLostGroup.FindElement("levelname");
            levelLostTime = levelLostGroup.FindElement("time");
            levelLostBestTime = levelLostGroup.FindElement("besttime");
            levelLostGeneratorsTurnedOn = levelLostGroup.FindElement("generatorsturnedon");
        }

        levelWonGroup = game.canvas.FindElementGroupByID("LevelEscapedGroup");

        if (levelWonGroup != null)
        {
            levelWonName = levelWonGroup.FindElement("levelname");
            levelWonTime = levelWonGroup.FindElement("time");
            levelWonBestTime = levelWonGroup.FindElement("besttime");
            levelWonGeneratorsTurnedOn = levelWonGroup.FindElement("generatorsturnedon");
        }

        levelObjectiveText = game.canvas.FindElementGroupByID("ObjectiveGroup").FindElement("levelobjectivetext");

        fadeGroup = game.canvas.FindElementGroupByID("FadeGroup");
        exitGameGroup = game.canvas.FindElementGroupByID("ExitGameGroup");
    }

    public void OnEnable()
    {
        GameEvents.OnGeneratorTurnedOn += OnGeneratorTurnedOn;
        GameEvents.OnStartGame += OnGameStarted;
        GameEvents.OnEndGame += OnGameEnded;
    }

    public void OnDisable()
    {
        GameEvents.OnGeneratorTurnedOn -= OnGeneratorTurnedOn;
        GameEvents.OnStartGame -= OnGameStarted;
        GameEvents.OnEndGame -= OnGameEnded;
    }

    private void OnGameEnded(bool won, string name, float time, int generators)
    {
        if (won)
        {
            levelWonName.OverrideValue(name);
            levelWonTime.OverrideValue("TIME: " + time);
            levelWonGeneratorsTurnedOn.OverrideValue("GENERATORS ENABLED: " + generators);
            game.canvas.ShowElementGroup(levelWonGroup, true);
        }
        else
        {
            levelLostName.OverrideValue(name);
            levelLostTime.OverrideValue("TIME: " + time);
            levelLostGeneratorsTurnedOn.OverrideValue("GENERATORS ENABLED: " + generators);
            game.canvas.ShowElementGroup(levelLostGroup, true);
        }

        generatorsLeftText.text = "";
    }



    private void OnGameStarted(LevelProperties properties)
    {
        OnGeneratorTurnedOn(properties.generatorsInLevel);

        PrintText("Find and activate all of the generators.");
    }

    private void PrintText(string message)
    {
        StartCoroutine(PrintText(message, 8.0f));
    }

    private IEnumerator PrintText(string message, float time)
    {
        levelObjectiveText.SetActive(true);
        StartCoroutine(DialogueFunctions.PrintText(message, levelObjectiveText, 0.08f));
        yield return new WaitForSeconds(time);
        StartCoroutine(DialogueFunctions.PrintText(string.Empty, levelObjectiveText, 0.08f));
    }

    private void OnGeneratorTurnedOn(int generatorAmount)
    {
        string text = generatorAmount <= 0 ? "Return to the door!" : "Generators remaining: " + generatorAmount;

        generatorsLeftText.text = text;
    }

    public void SetIntroUI(LevelProperties currentLevelProperties, float time)
    {
        fadeGroup.UpdateElements(1, 0, false);
        fadeGroup.FindElement("levelnumbertext").OverrideValue("LEVEL " + currentLevelProperties.levelIndex);
        fadeGroup.FindElement("levelnametext").OverrideValue(currentLevelProperties.levelName.ToUpper());
        fadeGroup.UpdateElements(0, time, false);
    }

    public void ResetIntroUI()
    {
        fadeGroup.FindElement("levelnumbertext").SetActive(false);
        fadeGroup.FindElement("levelnametext").SetActive(false);
    }
}
