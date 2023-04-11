using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Resources;

//[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Necronomicon Workaround (Original Dev)", order = 7)]
public class NecronomiconDevWorkaround : CustomEvent {
    public Option canRead;
    public Option cantRead;
    public override Option SpecialOption(){
        if(HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Cultist)) > 0){
            return canRead;
        }else{
            return cantRead;
        }
    }
}