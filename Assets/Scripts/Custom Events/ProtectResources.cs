using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Protect Resources", menuName = "Game Events/Protect Resources", order = 6)]
public class ProtectResources : CustomEvent {
    public List<Resource> resourcesToProtect = new List<Resource>();
    public virtual void SpecialEffect(){

    }
    public virtual List<Resource> ReturnSpecialValue(){
        return new List<Resource>(0);
    }
}