using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeColorOrSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Color colorDefault = Color.white, colorActive = Color.white;
    public bool swapToActiveUseCurrentDefault;
    public Sprite spriteDefault, spriteActive;
    public Image targetImage;
    public bool disableInFrozenTime;
    private bool highlight;

    public void OnPointerEnter(PointerEventData eventData){
        if(Time.timeScale > 0 || !disableInFrozenTime){
            targetImage.color = colorActive;
            if(spriteActive != null){
                highlight = true;
            }
        }
    }
    void LateUpdate(){
        if(highlight){
            if(Time.timeScale > 0 || !disableInFrozenTime){
                targetImage.sprite = spriteActive;
                if(swapToActiveUseCurrentDefault && spriteDefault == null){
                    spriteDefault = targetImage.sprite;
                }
            }
        }
    }
    private void SetDefault(){
        targetImage.color = colorDefault;
        if(spriteDefault != null){
            targetImage.sprite = spriteDefault;
        }
    }
    public void OnPointerExit(PointerEventData eventData){
        if(Time.timeScale > 0 || !disableInFrozenTime){
            SetDefault();
            highlight = false;
        }
    }
    void OnDisable(){
        SetDefault();
        highlight = false;
    }
}