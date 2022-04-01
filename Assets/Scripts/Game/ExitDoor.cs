using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitDoor : MonoBehaviour
{
    public TextMeshProUGUI needToCollectMessage;
    public float textFadeRate;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            if (MainManager.GetCanOpenDoor)
            {
                StartCoroutine("displayDooorOpenSuccessMessage");
            }
            else
            {
                StartCoroutine("displayDooorOpenFailMessage");
            }
        }
    }

    IEnumerator displayDooorOpenSuccessMessage()
    {
        needToCollectMessage.gameObject.SetActive(true);
        needToCollectMessage.text = "Exiting level...";
        yield return new WaitForSeconds(2);
        needToCollectMessage.gameObject.SetActive(false);
        SceneManager.LoadScene(MainManager.nextScene);
    }

    IEnumerator displayDooorOpenFailMessage()
    {
        needToCollectMessage.gameObject.SetActive(true);
        needToCollectMessage.text = "Collect all notes to exit";
        yield return new WaitForSeconds(2);
        needToCollectMessage.gameObject.SetActive(false);

    }
}
