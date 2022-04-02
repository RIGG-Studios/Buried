using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MainManager : MonoBehaviour
{
    [SerializeField]
    int generatorsInLevel;

    [SerializeField]
    string nextSceneName;

    static bool canOpenDoor;
    static int remainingGenerators;

    public static string nextScene;

    private void Start()
    {
        remainingGenerators = generatorsInLevel;
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

    public static bool GetCanOpenDoor => canOpenDoor;
}
