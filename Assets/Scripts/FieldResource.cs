using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class FieldResource : MonoBehaviour
{
    public static int upperCardRenderLayer = 1000;
    public static int lowerCardRenderLayer = 800;
    public GameObject burningCardPrefab;
    public static event Action<int> ReorderCanvases;
    public static event Action<Resource, int> ThereAreLessOfUsNow;
    public Resource resourceType;
    public HandController handController;
    [SerializeField]
    private int deathPriorityQueue;
    private bool iAmPlaced;
    private bool justPlaced;
    private Vector2 firstMousePos;
    private Vector3 clickedPos;
    private GameObject selectedObject;
    private bool placedInHand;

    void OnEnable()
    {
        ReorderCanvases += SetNewOrder;
        ThereAreLessOfUsNow += CheckIfIShouldDieLater;
    }
    void OnDisable()
    {
        ReorderCanvases -= SetNewOrder;
        ThereAreLessOfUsNow -= CheckIfIShouldDieLater;
    }

    void Start()
    {

    }

    void OnMouseOver(){
        if(((ResourceCard.heldCard == null) || (ResourceCard.heldCard.GetComponentInParent<Canvas>().sortingOrder < GetComponentInParent<Canvas>().sortingOrder)) && Input.GetMouseButton(0)){
            ResourceCard.heldCard = gameObject;
            firstMousePos = Input.mousePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        justPlaced = false;
        if(!Input.GetMouseButton(0) && ResourceCard.heldCard == gameObject){
            if(ResourceCard.heldCard == gameObject){
                ResourceCard.pickedUpCard = false;
                if(Input.mousePosition.y/Screen.height < .3f){
                    HandController.cardObjectsInField.Remove(gameObject);
                    HandController.handResources.Add(resourceType);
                    HandController.fieldResources.Remove(resourceType);
                    handController.UpdateHandContents();
                    handController.updateHandVisuals();
                    ResourceCard.heldCard = null;
                    placedInHand = true;
                    Destroy(gameObject);
                }else{
                    HandController.placedFieldResources.Add(resourceType);
                    ResourceCard.heldCard = null;
                    transform.position = GetTargetPosition();
                    iAmPlaced = true;
                    justPlaced = true;
                }
            }
            ResourceCard.heldCard = null;
        }
        if(ResourceCard.heldCard == gameObject && Input.GetMouseButton(0)){
            if(!ResourceCard.pickedUpCard){
                ResourceCard.pickedUpCard = true;
                iAmPlaced = false;
                ThereAreLessOfUsNow?.Invoke(resourceType, deathPriorityQueue);
                placeCardOnTop();
                HandController.placedFieldResources.Remove(resourceType);
            }
            transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), .25f);
        }
        if(HandController.fieldResources.Where(item => item == resourceType).Count() < deathPriorityQueue){
            if(!placedInHand){
                var burningCard = Instantiate(burningCardPrefab);
                burningCard.GetComponent<CardBurnKinematics>().resourceType = resourceType;
                burningCard.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
    }

    void SetNewOrder(int moved){
        if(GetComponentInParent<Canvas>().sortingOrder > moved && GetComponentInParent<Canvas>().sortingOrder > lowerCardRenderLayer){
            GetComponentInParent<Canvas>().sortingOrder--;
        }
    }

    void CheckIfIShouldDieLater(Resource typeToCheck, int cardMoved){
        if(typeToCheck == resourceType && !justPlaced && cardMoved < deathPriorityQueue){
            deathPriorityQueue--;
        }
    }

    Vector3 GetTargetPosition(){
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target = new Vector3(target.x, target.y, 0);
            return target;
    }

    public void placeCardOnTop(){
        ReorderCanvases?.Invoke(transform.GetComponentInParent<Canvas>().renderOrder);
        GetComponentInParent<Canvas>().sortingOrder = upperCardRenderLayer;
        deathPriorityQueue = HandController.fieldResources.Where(item => item == resourceType).Count();
    }
}
