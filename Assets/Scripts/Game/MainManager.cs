using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField]  private TextMeshPro remainingGeneratorsText;
    [SerializeField]  private string nextSceneName;

    static bool canOpenDoor = false;
    static int remainingGenerators;

    public static string nextScene;

    private void Start()
    {
        remainingGenerators = GameManager.instance.game.currentLevelProperties.properties.generatorsInLevel;
        Debug.Log(remainingGenerators);
        nextScene = nextSceneName;
    }

    public static int GetRemainingGenerators => remainingGenerators;

    public static int DecrementGeneratorCount()
    {
        remainingGenerators--;
        GameEvents.OnGeneratorTurnedOn?.Invoke(remainingGenerators);
        Debug.Log(remainingGenerators);

        if (remainingGenerators <= 0)
        {
            canOpenDoor = true;
        }

        return remainingGenerators;
    }

    public TextMeshPro GetRemainingGeneratorsText() => remainingGeneratorsText;
    public static bool GetCanOpenDoor => canOpenDoor;
}
