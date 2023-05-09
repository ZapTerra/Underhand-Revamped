using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This script is used in the tutorial when the player's mouse hovers over the sigil.




public class SwapSpriteOnHover : MonoBehaviour
{
    public Animator animator;
    public Sprite hoverSprite;
    void Update(){
        if(ResourceCard.pickedUpCard == false){
            animator.SetBool("highlight", false);
        }else{
            animator.SetBool("highlight", true);
        }
    }
}
