using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Effect", order = 2)]
public class CustomEvent : ScriptableObject {
    public virtual void SpecialEffect(){

    }
    public virtual List<Resource> ReturnSpecialValue(){
        return new List<Resource>(0);
    }
    public virtual Option SpecialOption(){
        return new Option();
    }
}