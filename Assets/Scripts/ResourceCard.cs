using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class ResourceCard : MonoBehaviour
{
    public float travelSpeedOnCreated = 2f;
    public static GameObject heldCard;
    public static bool pickedUpCard;
    public Resource resourceType;
    public HandController handController;
    public GameObject fieldResource;
    private Vector2 lastMousePos;
    private Vector2 firstMousePos;
    private float movementLerp;
    public int creationBehavior;
    
    private Vector3 startPosition;
    private float handRotationBeforeMoving;
    public bool reachedPosition;

    private LayerMask clickableLayers;
    private RaycastHit2D[] hits; //Change this number to however many selectable objects you think you'll have layered on top of eachother. This is for performance reasons.
    private float rayStart = -100; //Start Raycast from this Z-Coordinate
    private float rayEnd = 100;  //End Raycast at this Z-Coordinate
    private Vector3 clickedPos;
    private GameObject selectedObject;

    void Start()
    {
        gameObject.GetComponent<Image>().sprite = FindObjectOfType<GetCardSprite>().ReturnAppropriateCardSprite(resourceType);
        if(creationBehavior == 2){
            startPosition = FindObjectOfType<ResourceAnimationManager>().giveMePositionBurn(resourceType);
        }else if(creationBehavior == 1){
            startPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        }else{
            reachedPosition = true;
        }

        if(creationBehavior > 0){
            transform.parent.position = startPosition - (transform.position - transform.parent.position);
            var z = transform.parent.parent.position;
            var p = transform.parent.position;
            transform.parent.position = new Vector3(p.x, p.y, z.z);
        }
    }

    void OnMouseOver(){
        if(((heldCard == null) || (heldCard.GetComponentInParent<Canvas>().sortingOrder < GetComponentInParent<Canvas>().sortingOrder)) && Input.GetMouseButton(0)){
            heldCard = gameObject;
            handRotationBeforeMoving = HandController.handRotation;
            lastMousePos = Input.mousePosition;
            firstMousePos = Input.mousePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!reachedPosition){
            //This will break if you rearrange the hierarchy
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, transform.parent.parent.position, Time.deltaTime * travelSpeedOnCreated);
            if(transform.parent.position == transform.parent.parent.position){
                reachedPosition = true;
            }
        }

        if(Input.GetMouseButtonUp(0)){
            if(heldCard == gameObject){
                heldCard = null;
            }
        }
        if(heldCard == gameObject && Input.GetMouseButton(0)){
            if((Input.mousePosition.y - firstMousePos.y) / Screen.height > .05f){
                pickedUpCard = true;
                var fieldCard = Instantiate(fieldResource);
                HandController.handResources.Remove(resourceType);
                HandController.fieldResources.Add(resourceType);
                HandController.cardObjectsInField.Add(fieldCard);
                var fieldScript = fieldCard.GetComponent<FieldResource>();
                //I know this is bad,
                //
                //
                ////
                fieldCard.GetComponentInChildren<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                fieldCard.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                fieldCard.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                fieldScript.resourceType = resourceType;
                fieldScript.placeCardOnTop();
                fieldScript.handController = handController;
                heldCard = fieldCard;
                //Shouldn't result in the creation of new cards. In the case of a bug it's probably better to just not animate the new cards.
                handController.UpdateHandContents(0);
                handController.updateHandVisuals();
            }
            //If the player drags within the second/ninth tenth (i hate ENGLISH I HATE ASHFKLJ) of the screen,
            //Lerp over to a higher value so that by the time they reach the first/last tenth of the screen the hand reaches its maximum rotation
            float rot = 0f;
            if(Mathf.Abs(Input.mousePosition.x - Screen.width / 2) >= Screen.width * .3f){
                //how far the hand should be lerped from where rotation begins to ends
                float ampedLerpValue = (Mathf.Clamp((Mathf.Abs(Input.mousePosition.x - Screen.width / 2) - (Screen.width * .3f)), 0, Screen.width * .1f) / (Screen.width * .1f));
                //whether the hand's rotation value should be negative or positive.
                float sign = -Mathf.Sign(Input.mousePosition.x - Screen.width / 2);
                //Move where the hand is at towards the target lerped value, by Time.deltaTime.
                //I wasn't originally going to use the value of where the hand should be without the effect, but it makes a bezier-like smoothness even with no lerping
                //I think whoever programmed this before I did had to go through the same headache and found the same solution :)
                rot = Mathf.MoveTowards(HandController.handRotation, Mathf.Lerp(handRotationAtX(Input.mousePosition.x), handController.maxHandRotation() * sign, ampedLerpValue), Time.deltaTime * handController.fancyScrollSpeed);
            }else{
                rot = handRotationAtX(Input.mousePosition.x);
            }
            HandController.handRotation = rot;
        }
    }

    private float handRotationAtX(float x){
        return handRotationBeforeMoving - (x - lastMousePos.x) / Screen.width * 120;
    }

    void SelectTopTile()
    {
        clickedPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        hits = Physics2D.LinecastAll(new Vector3(clickedPos.x,clickedPos.y,rayStart),new Vector3(clickedPos.x,clickedPos.y,rayEnd),clickableLayers); //Cast ray at the world space the mouse is at
       
        if(hits.Length > 0) //Only function if we actually hit something
        {
            int topHit = 0; //Set our top hit to a default of the first index in our "hits" array, in case there are no others
            int preValue = hits[0].transform.GetComponentInParent<Canvas>().sortingOrder; //Set our first compare value to the SortingOrder value of the first object in the array, so it doesn't get skipped
           
            for (int arrayID = 1; arrayID < hits.Length; arrayID++) //Loop for every extra item the raycast hit
            {
                int tempValue = hits[arrayID].transform.GetComponentInParent<Canvas>().sortingOrder; //Store SortingOrder value from the current item in the array being accessed
               
                if (tempValue > preValue) //If the SortingOrder value of the current check is higher than the previous lowest
                {
                    preValue = tempValue; //Set the "Previous Value" to the current one just changed, for comparison later in loop
                    topHit = arrayID; //Set our topHit with the Array Index value of the current Array item, since it currently has the highest/closest SortingOrder value
                }
            }
            selectedObject = hits[topHit].transform.gameObject;
        }
    }
}

