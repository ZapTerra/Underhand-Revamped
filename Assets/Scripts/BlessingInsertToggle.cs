using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BlessingInsertToggle : MonoBehaviour
{
    public List<Card> blessings;
    public string godName;
    public Image graphic;
    public Toggle toggle;
    public Button button;
    public GameObject highlight;
    private bool currentlyInserted;
    void Start(){
        if(PlayerPrefs.GetInt(godName) < 1){
            Debug.Log("Not summoned yet");
            graphic.color = new Color(2/3f, 2/3f, 2/3f, 1);
            toggle.enabled = false;
            button.enabled = false;
            highlight.SetActive(false);
        }else{
            graphic.color = Color.white;
            toggle.enabled = true;
            button.enabled = true;
            highlight.SetActive(true);
        }
    }
    public void Toggle(){
        if(currentlyInserted){
            Debug.Log("Blessings removed.");
            CardManager.discardPile = CardManager.discardPile.Except(blessings).ToList();
        }else{
            Debug.Log("Blessings inserted!");
            CardManager.discardPile.AddRange(blessings);
        }
        currentlyInserted = !currentlyInserted;
    }
}
