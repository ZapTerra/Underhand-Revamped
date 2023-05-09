using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonOnPause : MonoBehaviour
{
    public Button targetButton;
    //just felt like adding it
    public bool keepFrozenOnUnpause;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0){
            targetButton.enabled = false;
        }else if(keepFrozenOnUnpause == false){
            targetButton.enabled = true;
        }
    }
}
