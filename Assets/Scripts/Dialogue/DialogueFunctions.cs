using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueFunctions : MonoBehaviour
{
    public void PrintTextToField(string text, TextMeshProUGUI textField, float timeBetweenCharacters)
    {
        StartCoroutine(PrintText(text, textField, timeBetweenCharacters));
    }

    IEnumerator PrintText(string text, TextMeshProUGUI field, float time)
    {
        for(int i = 0; i < text.Length + 1; i++)
        {
            field.text = text.Substring(0, i);
            yield return new WaitForSeconds(time);
        }
    }
}
