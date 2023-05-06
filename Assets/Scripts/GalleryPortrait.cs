using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryPortrait : MonoBehaviour
{
    public string godName;
    public Image graphic;
    public GameObject highlight;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt(godName) < 1){
            Debug.Log("Not summoned yet");
            graphic.color = new Color(2/3f, 2/3f, 2/3f, 1);
            button.enabled = false;
            highlight.SetActive(false);
        }else{
            graphic.color = Color.white;
            button.enabled = true;
            highlight.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
