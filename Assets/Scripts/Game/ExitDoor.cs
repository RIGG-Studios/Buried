using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitDoor : MonoBehaviour
{
    public TextMeshProUGUI needToCollectMessage;
    public float textFadeRate;


    private Animator animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    
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
        animator.SetTrigger("Open");
        needToCollectMessage.gameObject.SetActive(true);
        needToCollectMessage.text = "Exiting level...";
        yield return new WaitForSeconds(3.5f);
        needToCollectMessage.gameObject.SetActive(false);
        GameManager.instance.game.SetGameState(GameStates.Exiting);
    }

    IEnumerator displayDooorOpenFailMessage()
    {
        needToCollectMessage.gameObject.SetActive(true);
        needToCollectMessage.text = "Enable all the generators to power the door!";
        yield return new WaitForSeconds(2);
        needToCollectMessage.gameObject.SetActive(false);

    }
}
