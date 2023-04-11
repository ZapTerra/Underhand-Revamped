using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class ResourceIcon : MonoBehaviour
{
    public int resourceOrder;
    public List<Resource> costOption;
    public Resource resourceType;
    public Resource resourceTypes;
    public bool isCost;

    public Sprite relic;
    public Sprite cultist;
    public Sprite money;
    public Sprite food;
    public Sprite prisoner;
    public Sprite human;
    public Sprite suspicion;

    private bool graeyedOut;
    private List<Resource> lastResourcesInPlay = new List<Resource>();
    void Start()
    {
        var displaySprite = relic;
        if(resourceType == Resource.Cultist){
            displaySprite = cultist;
        }
        if(resourceType == Resource.Money){
            displaySprite = money;
        }
        if(resourceType == Resource.Food){
            displaySprite = food;
        }
        if(resourceType == Resource.Prisoner){
            displaySprite = prisoner;
        }
        if(resourceType == Resource.Human){
            displaySprite = human;
        }
        if(resourceType == Resource.Suspicion){
            displaySprite = suspicion;
        }
        GetComponent<Image>().sprite = displaySprite;
    }

    void Update()
    {
        //kinda inefficient but a lot easier to implement than a better solution and the computing cost is fairly small
        if(!lastResourcesInPlay.Equals(HandController.placedFieldResources)){
            lastResourcesInPlay = new List<Resource>(HandController.placedFieldResources);
            var tempList = new List<Resource>(HandController.placedFieldResources);
            var tempList2 = new List<Resource>(HandController.placedFieldResources);
            var needResource = new List<Resource>(costOption);
            foreach(Resource r in costOption){
                tempList = new List<Resource>(tempList2);
                foreach(Resource t in tempList){
                    if(MatchResource.OneToTwoNoRelic(t, r)){
                        //uses r and t respectively because of resource requirements which can be satisfied in multiple ways
                        needResource.Remove(r);
                        tempList2.Remove(t);
                        break;
                    }
                }
            }
            foreach(Resource r in tempList2){
                if(r == Resource.Relic && needResource.Count > 0){
                    needResource.RemoveAt(0);
                }
            }
            
            if((costOption.Count(x => MatchResource.OneToTwoNoRelic(x, resourceType)) - needResource.Count(x => MatchResource.OneToTwoNoRelic(x, resourceType)) >= resourceOrder) && isCost){
                GetComponent<Image>().color = Color.gray;
                graeyedOut = true;
            }else if(graeyedOut){
                GetComponent<Image>().color = Color.white;
            }
        }
    }
}
