using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

//[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Lose", order = 3)]
public class Lose : CustomEvent {
    public override void SpecialEffect(){
        FindObjectOfType<WinLoseUI>().Lose();
    }
}