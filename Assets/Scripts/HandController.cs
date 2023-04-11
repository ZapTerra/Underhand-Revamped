using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class HandController : MonoBehaviour
{
    public static float handRotation;
    public static float handRotationBeforeMoving;
    public float fancyScrollSpeed = 100f;
    public List<Resource> inspectorInputResources = new List<Resource>();
    public GameObject cardPrefab;
    public static List<Resource> playerResources = new List<Resource>();
    public static List<Resource> handResources = new List<Resource>();
    public static List<Resource> uninstantiatedResources = new List<Resource>();
    public static List<Resource> fieldResources = new List<Resource>();
    public static List<Resource> placedFieldResources = new List<Resource>();
    public static List<GameObject> cardObjectsInHand = new List<GameObject>();
    public static List<GameObject> cardObjectsInField = new List<GameObject>();
    private List<float> rotationIndices;

    void Awake(){
        handRotation = 0;
        handRotationBeforeMoving = 0;
        playerResources.Clear();
        handResources.Clear();
        uninstantiatedResources.Clear();
        fieldResources.Clear();
        placedFieldResources.Clear();
        cardObjectsInHand.Clear();
        cardObjectsInField.Clear();
        ResourceCard.heldCard = null;
        ResourceCard.pickedUpCard = false;
        handResources = new List<Resource>(inspectorInputResources);
        playerResources = new List<Resource>(inspectorInputResources);
    }

    void Start()
    {
        UpdateHandContents(0);
    }

    // Update is called once per frame
    void Update()
    {
        updateHandVisuals();
    }

    //newResources bool tells new cards created whether to animate into the hand from the mouse cursor or from the play table.
    public void UpdateHandContents(int newResourceBehavior){
        List<Resource> handResourceObjects = new List<Resource>();
        List<Resource> extraResourceObjects = new List<Resource>();
        if(cardObjectsInHand.Count != 0)
        foreach(GameObject obj in cardObjectsInHand){
            handResourceObjects.Add(obj.GetComponentInChildren<ResourceCard>().resourceType);
            extraResourceObjects.Add(obj.GetComponentInChildren<ResourceCard>().resourceType);
        }
        List<Resource> neededResources = new List<Resource>();
        //determine missing resources
        foreach(Resource res in handResources){
            if(extraResourceObjects.Contains(res)){
                extraResourceObjects.Remove(res);
            }else{
                neededResources.Add(res);
            }
        }
        //determine excess resources
        List<GameObject> deathRow = new List<GameObject>();
        foreach(GameObject gam in cardObjectsInHand){
            Resource res = gam.GetComponentInChildren<ResourceCard>().resourceType;
            if(extraResourceObjects.Contains(res)){
                deathRow.Add(gam);
                extraResourceObjects.Remove(res);
            }
        }
        //destroy excess resources
        foreach(GameObject gam in deathRow){
                cardObjectsInHand.Remove(gam);
                Destroy(gam);
        }

        //create missing resources
        List<Resource> targetContents = new List<Resource>();
        targetContents.AddRange(handResourceObjects);
        targetContents.AddRange(neededResources);
        List<int> insertionIndices = new List<int>{};
        SortInput(neededResources);
        SortInput(targetContents);

        foreach(Resource res in neededResources){
            var newCard = Instantiate(cardPrefab, transform);
            cardObjectsInHand.Insert(targetContents.IndexOf(res), newCard);
            var newCardScript = newCard.GetComponentInChildren<ResourceCard>();
            newCardScript.handController = this;
            newCardScript.resourceType = res;
            newCardScript.creationBehavior = newResourceBehavior;
        }
        
        updateHandVisuals();
    }

    private float GetRawRotation(int i, float middleCardsRotation){
        var currentRotation = cardObjectsInHand[i].gameObject.transform.eulerAngles;
        float rawRotationIndex = (i) - (cardObjectsInHand.Count - 1)/2f;
        return handRotation - (int)rawRotationIndex * 15 - Mathf.Sign(rawRotationIndex) * middleCardsRotation;
    }

    public float maxHandRotation(){
        float maxRot = 15f;
        if(cardObjectsInHand.Count > 0){
            maxRot = cardObjectsInHand.Count / 2 * 15 - (cardObjectsInHand.Count%2 == 0 ? 7.5f : 0f);
        }
        return maxRot;
    }

    public void updateHandVisuals(){
        handRotation = Mathf.Clamp(handRotation, -maxHandRotation(), maxHandRotation());
        transform.eulerAngles = Vector3.forward * handRotation;
        if(cardObjectsInHand.Count != 0){
            int i= 0;
            foreach(GameObject obj in cardObjectsInHand){
                float cardRotation = obj.transform.eulerAngles.z;
                float amountRotated = -(Mathf.DeltaAngle(0, cardRotation)) / 15f;
                Transform cardSprite = obj.transform.GetChild(0);
                var cardScale = (-.01f * Mathf.Pow(amountRotated, 2) + 1f);
                cardSprite.localScale = Vector3.one * Mathf.Clamp(cardScale, .6f, 2f);;
                cardSprite.localEulerAngles = Vector3.forward * amountRotated * 1.5f;
                cardSprite.transform.localPosition = Vector3.up * 2 + Vector3.down * Mathf.Abs(amountRotated) * .02f + Vector3.forward * Mathf.Abs(amountRotated);
                var sprite = cardSprite.GetComponent<Image>();
                float middleCardsRotation = cardObjectsInHand.Count%2 == 0 ? 7.5f : 0f;
                cardObjectsInHand[i].gameObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(GetRawRotation(i, middleCardsRotation), -180, 180));
                i++;
                obj.GetComponent<Canvas>().sortingOrder = (int)(50 - Mathf.Abs(amountRotated + .5f));
            }
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
