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
        // if(Time.timeScale == 0 && interactable){
        //     interactable = false;
        //     beganAsInteractable = true;
        // }else if(Time.timeScale > 0 && !interactable && beganAsInteractable){
        //     interactable = true;
        // }

        if(Input.GetMouseButtonDown(0)){
            if(cursorWithinButton){
                leavePressToSystem = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            if (RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !leavePressToSystem && interactable)
            {
                Debug.Log("Button clicked!");
                onClick.Invoke();
            }
            leavePressToSystem = false;
        }
    }
}