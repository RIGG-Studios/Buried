using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaredManager : MonoBehaviour
{
    public Transform heart;
    public int beatsPerMinute;
    public float beatSize;
    public float beatDelay;
    public int beatSizeIncrease;

    Vector3 originalScale;
    Vector3 targetScale;
    float time;

    Animator heartAnimator;

    private void Start()
    {
        originalScale = heart.localScale;
        targetScale = originalScale;

        if (heart)
            heartAnimator = heart.GetComponent<Animator>();

        StartCoroutine(BeatHeart());
    }

    private void Update()
    {
        heart.localScale = Vector3.Lerp(heart.localScale, targetScale, Time.deltaTime * 5f);

    }

    private IEnumerator BeatHeart()
    {
        float delay = (60f / beatsPerMinute);
        yield return new WaitForSeconds(delay / 2);

        heartAnimator.SetTrigger("BeatHeart");

        StartCoroutine(BeatHeart());
    }
}
