using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Lose", order = 3)]
public class Lose : CustomEvent {
    public virtual void SpecialEffect(){
        
    }
    public virtual List<Resource> ReturnSpecialValue(){
        return new List<Resource>(0);
    }
}