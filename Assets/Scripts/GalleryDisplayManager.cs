using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryDisplayManager : MonoBehaviour
{
    public Image god;
    public Image blessing1;
    public Image blessing2;
    public CanvasGroup galleryDisplayGroup;
    public Button exitButton;
    public ChangeColorOrSprite buttonGraphicModifier;
    public void OpenGallery(){
        StartCoroutine(FadeIn());
    }

    public void CloseGallery(){
        StartCoroutine(FadeOut());
    }

    public void SetGod(Sprite cardImage){
        god.sprite = cardImage;
    }

    public void SetBlessing1(Sprite cardImage){
        blessing1.sprite = cardImage;
    }

    public void SetBlessing2(Sprite cardImage){
        blessing2.sprite = cardImage;
    }

    public IEnumerator FadeIn(){
        galleryDisplayGroup.alpha = 0;
        exitButton.enabled = false;
        buttonGraphicModifier.enabled = false;
        GetComponent<GraphicRaycaster>().enabled = true;
        while(galleryDisplayGroup.alpha < 1){
            galleryDisplayGroup.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        exitButton.enabled = true;
        buttonGraphicModifier.enabled = true;
    }
    public IEnumerator FadeOut(){
        galleryDisplayGroup.alpha = 1;
        exitButton.enabled = false;
        buttonGraphicModifier.enabled = false;
        while(galleryDisplayGroup.alpha > 0){
            galleryDisplayGroup.alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<GraphicRaycaster>().enabled = false;
    }

}
