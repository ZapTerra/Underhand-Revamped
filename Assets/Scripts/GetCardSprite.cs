using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class GetCardSprite : MonoBehaviour {
    public Sprite relic;
    public Sprite cultist;
    public Sprite money;
    public Sprite food;
    public Sprite prisoner;
    public Sprite human;
    public Sprite suspicion;
    public Sprite ReturnAppropriateCardSprite(Resource resourceType){
        var sprite = relic;
        if(resourceType == Resource.Cultist){
            sprite = cultist;
        }
        if(resourceType == Resource.Money){
            sprite = money;
        }
        if(resourceType== Resource.Food){
            sprite = food;
        }
        if(resourceType== Resource.Prisoner){
            sprite = prisoner;
        }
        if(resourceType == Resource.Human){
            sprite = human;
        }
        if(resourceType == Resource.Suspicion){
            sprite = suspicion;
        }
        return sprite;
    }
}