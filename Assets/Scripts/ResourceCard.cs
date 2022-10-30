using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class ResourceCard : MonoBehaviour
{
    public static GameObject heldCard;
    public static bool pickedUpCard;
    public Resource resourceType;
    public HandController handController;
    public GameObject fieldResource;
    private Vector2 lastMousePos;
    private Vector2 firstMousePos;

    private LayerMask clickableLayers;
    private RaycastHit2D[] hits; //Change this number to however many selectable objects you think you'll have layered on top of eachother. This is for performance reasons.
    private float rayStart = -100; //Start Raycast from this Z-Coordinate
    private float rayEnd = 100;  //End Raycast at this Z-Coordinate
    private Vector3 clickedPos;
    private GameObject selectedObject;

    void Start()
    {
        gameObject.GetComponent<Image>().sprite = FindObjectOfType<GetCardSprite>().ReturnAppropriateCardSprite(resourceType);
    }

    void OnMouseOver(){
        if(((heldCard == null) || (heldCard.GetComponentInParent<Canvas>().sortingOrder < GetComponentInParent<Canvas>().sortingOrder)) && Input.GetMouseButton(0)){
            heldCard = gameObject;
            lastMousePos = Input.mousePosition;
            firstMousePos = Input.mousePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
                handController.UpdateHandContents();
                handController.updateHandVisuals();
            }
            HandController.handRotation -= (Input.mousePosition.x - lastMousePos.x) / Screen.width * 120;
            lastMousePos = Input.mousePosition;
        }
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

