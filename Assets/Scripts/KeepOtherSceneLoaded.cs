using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepOtherSceneLoaded : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("LoreCorkboard", LoadSceneMode.Additive);
    }
}
