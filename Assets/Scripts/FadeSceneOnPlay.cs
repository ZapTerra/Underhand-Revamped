using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSceneOnPlay : MonoBehaviour
{
    public float fadeTime = .5f;
    public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn(fadeTime));
    }
    public IEnumerator FadeIn(float time){
        float passed = 0;
        canvasGroup.alpha = 1f;
        yield return new WaitForSecondsRealtime(1f);
        while(passed < time){
            canvasGroup.alpha = 1 - passed/time;
            passed += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = 0f;
    }
}
