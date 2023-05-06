using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBlessingsTitleText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "CHOOSE UP TO " + (3 + PlayerPrefs.GetInt("Won Last Game")).ToString() + " BLESSINGS";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
