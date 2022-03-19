using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            MainManager.GetRemainingNotes(collision.gameObject.GetComponent<ItemDatabase>());

            if (MainManager.GetCanOpenDoor)
            {
                SceneManager.LoadScene(MainManager.nextScene);
            }
        }
    }
}
