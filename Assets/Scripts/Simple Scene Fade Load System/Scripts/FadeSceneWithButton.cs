using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSceneWithButton : MonoBehaviour
{
    public void Transition(string scene){
        Initiate.Fade(scene, Color.black, 2f);
    }
}
