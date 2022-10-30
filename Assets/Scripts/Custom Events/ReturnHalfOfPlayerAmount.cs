using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Return Half of Player Amount", order = 4)]
public class ReturnHalfOfPlayerAmount : CustomEvent {
    public List<Resource> resourcesToHalve;
    public List<Resource> resourcesToReturn;


    //This is coded so that you may input all of the resources which you wish to halve.
    //Then, if you wish to only output one resource, it will return half of the input rounded up (input type by default)
    //But if you wish for multiple types of resources to be returned,
    //It will return half of each input type rounded up, converted to the types of the output list (first input by default)
    public override List<Resource> ReturnSpecialValue(){
        List<Resource> returnArray = new List<Resource>(0);
        int count = 0;
        int loop = 0;
        foreach(Resource x in resourcesToHalve){
            foreach(Resource r in HandController.playerResources){
                //Don't change this to the MatchResource function, it evaluates Relics as valid matches.
                if(MatchResource.OneToTwoNoRelic(r, x)){
                    count++;
                }
            }
            if(resourcesToReturn.Count > 1){
                for(int i = 0; i < Mathf.Ceil((float)count / 2); i++){
                    returnArray.Add(resourcesToReturn[loop]);
                }
                count = 0;
            }
            loop++;
        }
        if(resourcesToReturn.Count < 2)
        for(int i = 0; i < Mathf.Ceil((float)count / 2); i++){
            if(resourcesToReturn.Count == 1)
            returnArray.Add(resourcesToReturn[0]);
            else
            returnArray.Add(resourcesToHalve[0]);
        }
        return returnArray;
    }
}