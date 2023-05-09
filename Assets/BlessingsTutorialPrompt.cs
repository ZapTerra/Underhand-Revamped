using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingsTutorialPrompt : MonoBehaviour
{
    public GameObject blessingsMenu;
    public GameObject tutorialPrompt;
    public GameObject settingsIcon;
    public CanvasGroup tutorialCanvasGroup;
    private bool fadeBlessingsIn;
    // Start is called before the first frame update
    void Start()
    {
        //Yeah, this is a little messy. Makes the scene prettier in the inspector, and could use cleaning.
        blessingsMenu.SetActive(false);
        tutorialPrompt.SetActive(true);

        if(PlayerPrefs.GetInt("blessingsintroduction") == 0){
            PlayerPrefs.SetInt("blessingsintroduction", 1);
            tutorialCanvasGroup.alpha = 1;
            Time.timeScale = 0;
        }else{
            blessingsMenu.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void FadeBlessingsIn(){
        blessingsMenu.SetActive(true);
        blessingsMenu.GetComponent<Animator>().Play("BlessingsInvisible");
        blessingsMenu.GetComponent<CanvasGroup>().alpha = 0;
        fadeBlessingsIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeBlessingsIn){
            tutorialCanvasGroup.alpha -= Time.unscaledDeltaTime;
            if(tutorialCanvasGroup.alpha == 0){
                blessingsMenu.GetComponent<Animator>().Play("FadeBlessingsIn");
                Destroy(gameObject);
            }
        }
    }
}
