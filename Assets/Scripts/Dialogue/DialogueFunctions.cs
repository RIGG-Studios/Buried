using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class DialogueFunctions
{
    public static IEnumerator PrintText(string text, UIElement textField, float time)
    {
        Debug.Log("message");

        for (int i = 0; i < text.Length + 1; i++)
        {
            object outputText = text.Substring(0, i);

            textField.OverrideValue(outputText);
            yield return new WaitForSeconds(time);
        }
    }
}
