using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public bool pauseMenuActive;
    public GameObject gameplayCameras;
    public GameObject pauseMenuCamera;
    public Canvas pauseMenuCanvas;
    private bool fading;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void swapCameraViewOnDelay(float fadeTime){
        if(fading == false){
            StartCoroutine(DoSwap(fadeTime));
            fading = true;
        }
    }

    public IEnumerator DoSwap(float fadeTime){
        FindObjectOfType<SettingsFade>().Fade(fadeTime);
        yield return new WaitForSecondsRealtime(fadeTime / 2);
        if(!pauseMenuActive){
            pauseMenuActive = true;
            Time.timeScale = 0;
            gameplayCameras.SetActive(false);
            pauseMenuCamera.SetActive(true);
            pauseMenuCanvas.enabled = true;
        }else{
            pauseMenuActive = false;
            Time.timeScale = 1;
            gameplayCameras.SetActive(true);
            pauseMenuCamera.SetActive(false);
            pauseMenuCanvas.enabled = false;
        }
        fading = false;
    }
}
