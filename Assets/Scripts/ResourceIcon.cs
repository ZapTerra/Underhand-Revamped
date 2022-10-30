using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class ResourceIcon : MonoBehaviour
{
    public int displayOrder;
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
        if(HandController.placedFieldResources.Count(x => MatchResource.OneToTwo(x, resourceType)) >= displayOrder && isCost){
            GetComponent<Image>().color = Color.gray;
            graeyedOut = true;
        }else if(graeyedOut){
            GetComponent<Image>().color = Color.white;
        }
    }
}
