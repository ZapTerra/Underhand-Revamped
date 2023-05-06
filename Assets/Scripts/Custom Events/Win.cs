using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Win", order = 3)]
public class Win : CustomEvent {
    public string godName;
    public override void SpecialEffect(){
        PlayerPrefs.SetInt("Won Last Game", 1);
        PlayerPrefs.SetInt(godName, PlayerPrefs.GetInt(godName) + 1);
        Debug.Log(PlayerPrefs.GetInt(godName));
        GalleryWinDisplay.win = godName;
        FindObjectOfType<WinLoseUI>().Win();
    }
}