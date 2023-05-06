using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseUI : MonoBehaviour
{
    public CanvasGroup winCanvasGroup;
    public Button winContinue;
    public CanvasGroup loseCanvasGroup;
    public Button lossContinue;
    // Start is called before the first frame update
    public void Win(){
        DisableInteractables();
        StartCoroutine(FadeWin());
    }

    public void Lose(){
        DisableInteractables();
        StartCoroutine(FadeLose());
    }

    private IEnumerator FadeWin(){
        winCanvasGroup.alpha = 0;
        winContinue.enabled = false;
        while(winCanvasGroup.alpha < 1){
            winCanvasGroup.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        winCanvasGroup.enabled = false;
        winContinue.enabled = true;
    }


    private IEnumerator FadeLose(){
        loseCanvasGroup.alpha = 0;
        lossContinue.enabled = false;
        while(loseCanvasGroup.alpha < 1){
            loseCanvasGroup.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        loseCanvasGroup.enabled = false;
        lossContinue.enabled = true;
    }

    void DisableInteractables(){
        //Because of the custom scripting which lets buttons mimic the original game,
        //as well as the code for picking up cards, the elements cannot be blocked and so must be disabled, or the other features rewritten.
        //Since winning is the definitive end of gameplay in this case, I have opted for the disabling method.
        Time.timeScale = 0;
        foreach(Button u in FindObjectsOfType<Button>()){
            u.enabled = false;
        }

        foreach(ResourceCard u in FindObjectsOfType<ResourceCard>()){
            u.enabled = false;
        }

        foreach(FieldResource u in FindObjectsOfType<FieldResource>()){
            u.enabled = false;
        }
    }
}

