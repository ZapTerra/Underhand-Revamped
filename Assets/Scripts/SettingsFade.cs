using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsFade : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    private bool fading;

    public void Fade(float totalTime){
        StartCoroutine(DoFade(totalTime));
    }

    public IEnumerator DoFade(float totalTime){
        float timeFading = 0f;
        fadeCanvas.alpha = 0f;
        while(timeFading < totalTime / 2){
            fadeCanvas.alpha += Time.unscaledDeltaTime / (totalTime / 2);
            timeFading += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        timeFading = 0;
        while(timeFading < totalTime / 2){
            fadeCanvas.alpha -= Time.unscaledDeltaTime / (totalTime / 2);
            timeFading += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        fadeCanvas.alpha = 0f;
    }
}
