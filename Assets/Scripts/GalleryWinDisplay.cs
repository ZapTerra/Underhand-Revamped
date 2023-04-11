using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryWinDisplay : MonoBehaviour
{
    public static string win;
    public Animator UIAnimator;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if(win != null){
            UIAnimator.Play("MenuIdle2");
            GameObject.Find(win).GetComponent<Button>().onClick.Invoke();
            win = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
