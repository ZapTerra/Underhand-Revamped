using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BlessingInsertToggle : MonoBehaviour
{
    public static int blessingCount;
    public List<Card> blessings;
    public string godName;
    public Image graphic;
    public Toggle toggle;
    public ChangeColorOrSprite graphicHighlightController;
    public GameObject highlight;
    private bool currentlyInserted;
    void Start(){
        blessingCount = 0;
        if(PlayerPrefs.GetInt(godName) < 1){
            Debug.Log("Not summoned yet");
            graphic.color = new Color(2/3f, 2/3f, 2/3f, 1);
            toggle.enabled = false;
            graphicHighlightController.enabled = false;
            highlight.SetActive(false);
        }else{
            graphic.color = Color.white;
            toggle.enabled = true;
            graphicHighlightController.enabled = true;
            highlight.SetActive(true);
        }
    }
    public void Toggle(){
        if(currentlyInserted){
            Debug.Log("Blessings removed.");
            CardManager.discardPile = CardManager.discardPile.Except(blessings).ToList();
            blessingCount--;
        }else{
            if(blessingCount < 3 + PlayerPrefs.GetInt("Won Last Game")){
                Debug.Log("Blessings inserted!");
                CardManager.discardPile.AddRange(blessings);
                blessingCount++;
            }else{
                toggle.isOn = false;
            }
        }
        currentlyInserted = !currentlyInserted;
    }
}
