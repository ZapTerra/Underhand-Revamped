using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseUI : MonoBehaviour
{
    public CanvasGroup win;
    public Button winContinue;
    public CanvasGroup lose;
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
        win.alpha = 0;
        winContinue.enabled = false;
        while(win.alpha < 1){
            win.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        winContinue.enabled = true;
    }


    private IEnumerator FadeLose(){
        lose.alpha = 0;
        lossContinue.enabled = false;
        while(lose.alpha < 1){
            lose.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
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

