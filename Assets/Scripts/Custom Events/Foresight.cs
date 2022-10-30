using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Foresight", order = 6)]
public class Foresight : CustomEvent {
    public bool foresightWithDiscard;
    public virtual void SpecialEffect(){
        
    }
    public virtual List<Resource> ReturnSpecialValue(){
        return new List<Resource>(0);
    }
}