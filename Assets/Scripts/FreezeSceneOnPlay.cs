using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSceneOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ZA    ゴ
        //   ゴ
        //ゴ
        Time.timeScale = 0;
        Debug.Log("Freezing the scene on play! Likely attached to the 'Blessings' object.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
