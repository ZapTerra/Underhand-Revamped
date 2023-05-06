using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UnderhandButton : Button
{
    private bool cursorWithinButton;
    private bool leavePressToSystem;
    private bool beganAsInteractable;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        cursorWithinButton = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        cursorWithinButton = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(cursorWithinButton){
                leavePressToSystem = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //If the camera is rendering the layer this gameobject is on && the click is within the bounds of this button && other conditions are met
            if (Camera.main.cullingMask == (Camera.main.cullingMask | (1 << gameObject.layer)) && RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !leavePressToSystem && interactable)
            {
                Debug.Log("Button clicked!");
                onClick.Invoke();
            }
            leavePressToSystem = false;
        }
    }
}