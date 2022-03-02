using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    CanvasManager canvas;

    UIElement gameTimer;
    UIElement watchCount;

    private void Start()
    {
        canvas = FindObjectOfType<CanvasManager>();

        if (canvas != null)
        {
            UIElementGroup group = canvas.FindElementGroupByID("GameGroup");

            gameTimer = group.FindElement("gametimer");
            gameTimer = group.FindElement("watchcounttext");
        }
    }

    public void UpdateGameTimer(int min, int sec) => gameTimer.OverrideValue(min.ToString("00") + ":" + sec.ToString("00"));
    public void UpdateWatchCountText(int watchCount) => gameTimer.OverrideValue(watchCount.ToString());
    public void UpdateDifficultyText(string difficulty) => watchCount.OverrideValue(difficulty);
}
