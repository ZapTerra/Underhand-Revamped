using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeParentIn : MonoBehaviour
{
    private CanvasGroup parentCanvasGroup;
    private bool canvasGroupAlreadyExists;
    private bool stopFading;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.gameObject.GetComponent<CanvasGroup>() != null){
            canvasGroupAlreadyExists = true;
            parentCanvasGroup = transform.parent.gameObject.GetComponent<CanvasGroup>();
        }else{
            parentCanvasGroup = transform.parent.gameObject.AddComponent<CanvasGroup>();
        }
        parentCanvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(parentCanvasGroup != null){
            parentCanvasGroup.alpha += Time.unscaledDeltaTime;
            if(parentCanvasGroup.alpha == 1 && canvasGroupAlreadyExists == false){
                Destroy(parentCanvasGroup);
                stopFading = true;
            }
        }
    }

    void OnDestroy(){
        if(parentCanvasGroup != null && canvasGroupAlreadyExists == false){
            Destroy(parentCanvasGroup);
        }
    }
}
