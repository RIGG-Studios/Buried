using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDemoManager : MonoBehaviour
{ 
    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void GiveFeedback()
    {
        Application.OpenURL("");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
