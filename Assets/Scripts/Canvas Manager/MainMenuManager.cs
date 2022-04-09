using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<UIElementGroup> menus = new List<UIElementGroup>();
    [SerializeField, Range(0, 10f)] private float playGameDelay = 0.0f;

    CanvasManager manager;
    bool toggledOptions;

    public float audioFadeSpeed;
    public AudioSource source;

    bool startedFading;
    UIElementGroup currentGroup;

    private void Start()
    {
        manager = GetComponent<CanvasManager>();
        source.volume = 1;
    }

    private void Update()
    {
        if (startedFading)
        {
            source.volume -= Time.deltaTime * audioFadeSpeed;
        }
    }

    public void ShowMenu(int index)
    {
        if (index < 0)
            return;

        UIElementGroup group = menus[index];

        if(group != null)
        {
            if(currentGroup != null)
                manager.HideElementGroup(currentGroup);

            currentGroup = group;
            manager.ShowElementGroup(currentGroup);
        }
    }

    public void QuitGame() => GameManager.instance.ExitGame();

    public void EnterLevel(Level properties) => StartCoroutine(IEPlayGame(properties));

    private IEnumerator IEPlayGame(Level properties)
    {
        startedFading = true;
        GameManager.instance.FadeIn(playGameDelay);
        yield return new WaitForSeconds(playGameDelay + 1f);
        startedFading = false;
        GameManager.instance.LoadLevel(properties);
    }
}
