using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class CardBurnKinematics : MonoBehaviour
{
    public float travelSpeed;
    public Animator animator;
    public Resource resourceType;
    public Vector3 targetMovement;
    public Vector3 startingPosition;
    public float totalToTravel;
    private bool playingAnimation;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Image>().sprite = FindObjectOfType<GetCardSprite>().ReturnAppropriateCardSprite(resourceType);
        startingPosition = transform.position;
        targetMovement = FindObjectOfType<ResourceAnimationManager>().giveMePosition(resourceType) - transform.position;
        totalToTravel = targetMovement.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if(totalToTravel > 0){
            Vector3 movementThisFrame = targetMovement * Time.deltaTime * travelSpeed;
            transform.position += movementThisFrame;
            totalToTravel -= movementThisFrame.magnitude;
        }else{
            if(!playingAnimation){
                animator.enabled = true;
                playingAnimation = true;
                if(resourceType == Resource.Cultist){
                animator.Play("COut");
                }
                if(resourceType == Resource.Money){
                    animator.Play("MOut");
                }
                if(resourceType== Resource.Food){
                    animator.Play("FOut");
                }
                if(resourceType== Resource.Prisoner){
                    animator.Play("POut");
                }
                if(resourceType == Resource.Human){
                    Debug.Log("There should not be a Human resource in the player hand");
                }
                if(resourceType == Resource.Suspicion){
                    animator.Play("SOut");
                }
                if(resourceType == Resource.Relic){
                animator.Play("ROut");
                }
            }
        }
    }
}
