using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Win", order = 2)]
public class Win : CustomEvent {
    public virtual void SpecialEffect(){
        
    }
    public virtual List<Resource> ReturnSpecialValue(){
        return new List<Resource>(0);
    }
}