using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Resources;

public class HandController : MonoBehaviour
{
    public static float handRotation;
    public static float handRotationBeforeMoving;
    public List<Resource> inspectorInputResources = new List<Resource>();
    public static List<Resource> playerResources = new List<Resource>();
    public static List<Resource> handResources = new List<Resource>();
    public static List<Resource> fieldResources = new List<Resource>();
    public static List<Resource> placedFieldResources = new List<Resource>();
    public GameObject cardPrefab;
    public static List<GameObject> cardObjectsInHand = new List<GameObject>();
    public static List<GameObject> cardObjectsInField = new List<GameObject>();
    private List<float> rotationIndices;

    void Awake(){
        handResources = new List<Resource>(inspectorInputResources);
        playerResources = new List<Resource>(inspectorInputResources);
    }

    void Start()
    {
        UpdateHandContents();
    }

    // Update is called once per frame
    void Update()
    {
        updateHandVisuals();
    }

    public void UpdateHandContents(){
        if(cardObjectsInHand.Count != 0)
        foreach(GameObject obj in cardObjectsInHand){
            DestroyImmediate(obj);
        }
        cardObjectsInHand.Clear();

        SortInput(handResources);

        foreach(Resource res in handResources){
            var newCard = Instantiate(cardPrefab, transform);
            cardObjectsInHand.Add(newCard);
            var newCardScript = newCard.GetComponentInChildren<ResourceCard>();
            newCardScript.handController = this;
            newCardScript.resourceType = res;
        }
        float middleCardsRotation = cardObjectsInHand.Count%2 == 0 ? 7.5f : 0f;
        for(int i = 0; i < cardObjectsInHand.Count; i++){
            var currentRotation = cardObjectsInHand[i].gameObject.transform.eulerAngles;
            float rawRotationIndex = ((i) - (cardObjectsInHand.Count - 1)/2f);
            cardObjectsInHand[i].gameObject.transform.eulerAngles = new Vector3(0, 0, currentRotation.z - (int)rawRotationIndex * 15 - Mathf.Sign(rawRotationIndex) * middleCardsRotation);
        }
    }

    public void updateHandVisuals(){
        transform.eulerAngles = Vector3.forward * handRotation;
        if(cardObjectsInHand.Count != 0)
        foreach(GameObject obj in cardObjectsInHand){
            float cardRotation = obj.transform.eulerAngles.z;
            float amountRotated = -(Mathf.DeltaAngle(0, handRotation) + Mathf.DeltaAngle(handRotation, cardRotation)) / 15f;
            Transform cardSprite = obj.transform.GetChild(0);
            cardSprite.localScale = Vector3.one * (-.01f * Mathf.Pow(amountRotated, 2) + 1f);
            cardSprite.localEulerAngles = Vector3.forward * amountRotated * 1.5f;
            cardSprite.transform.localPosition = Vector3.up * 2 + Vector3.down * Mathf.Abs(amountRotated) * .02f + Vector3.forward * Mathf.Abs(amountRotated);
            obj.GetComponent<Canvas>().sortingOrder = (int)(50 - Mathf.Abs(amountRotated + .5f));
        }
    }

    //Sloppy, but effective. Feel free to rewrite.
    public static List<Resource> SortInput(List<Resource> input){
        List<Resource> inputHand = new List<Resource>(input);
        input.Clear();
        addAllType(inputHand, input, Resource.Relic);
        addAllType(inputHand, input, Resource.Money);
        addAllType(inputHand, input, Resource.Cultist);
        addAllType(inputHand, input, Resource.Food);
        addAllType(inputHand, input, Resource.Prisoner);
        addAllType(inputHand, input, Resource.Suspicion);
        addAllType(inputHand, input, Resource.Human);
        return input;
    }

    // ¯\_(ツ)_/¯
    static void addAllType(List<Resource> inputHand, List<Resource> outputHand, Resource type){
        foreach(Resource res in inputHand){
            if(res == type){
                outputHand.Add(res);
            }
        }
    }
}
