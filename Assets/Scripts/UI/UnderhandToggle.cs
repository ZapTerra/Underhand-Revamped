using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnderhandToggle : Toggle
{
    private bool clickStartedOutside;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                clickStartedOutside = true;
            }else{
                clickStartedOutside = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Camera.main.ScreenToWorldPoint(Input.mousePosition)) && interactable)
            {
                Debug.Log("Toggle clicked!");
                if(clickStartedOutside){

                    isOn = !isOn;
                }
            }
            clickStartedOutside = false;
        }
    }
}