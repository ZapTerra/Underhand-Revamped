using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Return All of Player Amount", order = 5)]
public class ReturnAllOfPlayerAmount : CustomEvent {
    public List<Resource> resources;
    public List<Resource> resourcesToReturn;


    //This is coded so that you may input all of the resources you want evaluated.
    //Then, if you wish to only output one resource, it will return the input type by default.
    //But if you wish for multiple types of resources to be returned,
    //It will return each input type, converted to the types of the output list (first input by default)

    //This can't output a cost in multiple resources given a single input, and if you're this deep already you can write that feature yourself
    public override List<Resource> ReturnSpecialValue(){
        List<Resource> returnArray = new List<Resource>(0);
        int count = 0;
        int loop = 0;
        foreach(Resource x in resources){
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
        for(int i = 0; i < count; i++){
            if(resourcesToReturn.Count == 1)
            returnArray.Add(resourcesToReturn[0]);
            else
            returnArray.Add(resources[0]);
        }
        return returnArray;
    }
}