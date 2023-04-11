using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public bool pauseMenuActive;
    public GameObject gameplayCameras;
    public GameObject pauseMenuCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void swapCameraView(){
        Time.timeScale = 0;
        if(!pauseMenuActive){
            gameplayCameras.SetActive(false);
            pauseMenuCamera.SetActive(true);
        }else{
            gameplayCameras.SetActive(true);
            pauseMenuCamera.SetActive(false);
        }
    }
}
