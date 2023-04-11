using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using Resources;

//[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Greed", order = 8)]
public class Greed : CustomEvent {
    public GameObject burningCardPrefab;
    public override void SpecialEffect(){
        Debug.Log("Greed Activated");
        FindObjectOfType<ResourceAnimationManager>().StartNewBurn(new List<Resource>(){Resource.Relic}, true);
        var temp = new List<Resource>(HandController.playerResources);
        List<Resource> cardsToBurn = new List<Resource>();
        Random rnd = new Random();
        for(int i = 0; i < 5; i++){
            Resource selected = temp[rnd.Next(temp.Count)];
            temp.Remove(selected);
            cardsToBurn.Add(selected);
            Debug.Log(selected);
        }
        FindObjectOfType<ResourceAnimationManager>().StartNewBurn(cardsToBurn);
        foreach(Resource r in cardsToBurn){
            if(HandController.fieldResources.Contains(r)){
                HandController.placedFieldResources.Remove(r);
                HandController.fieldResources.Remove(r);
                HandController.playerResources.Remove(r);
            }else{
                HandController.handResources.Remove(r);
                HandController.playerResources.Remove(r);
                var burningCard = Instantiate(burningCardPrefab);
                burningCard.GetComponent<CardBurnKinematics>().resourceType = r;
                burningCard.transform.position = FindObjectOfType<HandController>().gameObject.transform.position;
            }
        }
    }
}