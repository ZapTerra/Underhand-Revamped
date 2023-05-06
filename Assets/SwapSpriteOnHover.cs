using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This script is used in the tutorial for when the player's mouse hovers over the sigil.




public class SwapSpriteOnHover : MonoBehaviour
{
    public Animator animator;
    public Sprite hoverSprite;
    void OnMouseOver(){
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = hoverSprite;
    }

    void OnMouseExit(){
        animator.enabled = true;
    }

}
