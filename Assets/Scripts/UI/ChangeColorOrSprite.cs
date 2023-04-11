using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeColorOrSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Color colorDefault = Color.white, colorActive = Color.white;
    public bool swapToActiveUseCurrentDefault;
    public Sprite spriteDefault, spriteActive;
    public Image targetImage;
    private bool highlight;

    public void OnPointerEnter(PointerEventData eventData){
        targetImage.color = colorActive;
        if(spriteActive != null){
            highlight = true;
        }
    }
    void LateUpdate(){
        if(highlight){
            targetImage.sprite = spriteActive;
            if(swapToActiveUseCurrentDefault && spriteDefault == null){
                spriteDefault = targetImage.sprite;
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
        SetDefault();
        highlight = false;
    }
    void OnDisable(){
        SetDefault();
        highlight = false;
    }
}