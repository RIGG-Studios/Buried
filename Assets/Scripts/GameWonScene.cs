using UnityEngine;

public class GameWonScene : MonoBehaviour
{
    private void Awake()
    {
        GameManager.instance.FadeOut(1f);
    }

    public void PlayAgain() => GameManager.instance.LoadMainMenu();

    public void ExitGame() => GameManager.instance.ExitGame();
}