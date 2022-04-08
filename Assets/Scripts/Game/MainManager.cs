using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField]  private TextMeshPro remainingGeneratorsText;
    [SerializeField]  private string nextSceneName;

    static bool canOpenDoor;
    static int remainingGenerators;

    public static string nextScene;

    private void Start()
    {
        remainingGenerators = GameManager.instance.game.currentLevelProperties.properties.generatorsInLevel;
        nextScene = nextSceneName;
    }

    private void Update()
    {
        if (remainingGenerators <= 0)
        {
            canOpenDoor = true;
        }
    }

    public static int GetRemainingGenerators => remainingGenerators;

    public static int DecrementGeneratorCount()
    {
        remainingGenerators--;
        GameEvents.OnGeneratorTurnedOn?.Invoke(remainingGenerators);

        if(remainingGenerators <= 0)
        {
            canOpenDoor = true;
        }

        return remainingGenerators;
    }

    public TextMeshPro GetRemainingGeneratorsText() => remainingGeneratorsText;
    public static bool GetCanOpenDoor => canOpenDoor;
}
