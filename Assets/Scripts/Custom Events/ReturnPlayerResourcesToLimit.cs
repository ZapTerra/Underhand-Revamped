using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Return Player Resources To Limit", order = 6)]
public class ReturnPlayerResourcesToLimit : CustomEvent {
    public List<Resource> resourcesToEvaluate;
    public List<int> resourceCaps;

    public override List<Resource> ReturnSpecialValue(){
        List<Resource> returnArray = new List<Resource>(0);
        int loop = 0;
        foreach(Resource x in resourcesToEvaluate){
            int count = 0;
            foreach(Resource r in HandController.playerResources){
                //Don't change this to the MatchResource function, it evaluates Relics as valid matches.
                if(MatchResource.OneToTwoNoRelic(r, x) && count < resourceCaps[loop]){
                    returnArray.Add(resourcesToEvaluate[loop]);
                    count++;
                }
            }
            loop++;
        }
        return returnArray;
    }
}