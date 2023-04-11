using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class CardBurnKinematics : MonoBehaviour
{
    public bool rewardResource;
    public float travelSpeed;
    public float minimumSpeedVector;
    public Animator animator;
    public Resource resourceType;
    public Vector3 targetMovement;
    public Vector3 startingPosition;
    public float totalToTravel;
    private bool playingAnimation;
    private float creationTime;

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
        GetComponentInChildren<Image>().sprite = FindObjectOfType<GetCardSprite>().ReturnAppropriateCardSprite(resourceType);

        //I programmed the top one first and it comes first in execution.
        //Do related events for the resource being destroyed or yielded to the player's hand.
        if(!rewardResource){
            startingPosition = transform.position;
            targetMovement = FindObjectOfType<ResourceAnimationManager>().giveMePositionBurn(resourceType) - transform.position;
            totalToTravel = targetMovement.magnitude;
            if(targetMovement.magnitude < minimumSpeedVector){
                targetMovement = targetMovement.normalized * minimumSpeedVector;
            }
        }else{
            transform.position = FindObjectOfType<ResourceAnimationManager>().giveMePositionBurn(resourceType, true);
            animator.enabled = true;
            playingAnimation = true;
            if(resourceType == Resource.Cultist){
                animator.Play("CIn");
            }
            if(resourceType == Resource.Money){
                animator.Play("MIn");
            }
            if(resourceType== Resource.Food){
                animator.Play("FIn");
            }
            if(resourceType== Resource.Prisoner){
                animator.Play("PIn");
            }
            if(resourceType == Resource.Human){
                Debug.Log("There should not be a Human resource being given to the player");
            }
            if(resourceType == Resource.Suspicion){
                animator.Play("SIn");
            }
            if(resourceType == Resource.Relic){
                animator.Play("RIn");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(totalToTravel > 0){
            Vector3 movementThisFrame = targetMovement * Time.deltaTime * travelSpeed;
            movementThisFrame = movementThisFrame.normalized * Mathf.Clamp(movementThisFrame.magnitude, 0,  totalToTravel);
            transform.position += movementThisFrame;
            totalToTravel -= movementThisFrame.magnitude;
            if(totalToTravel <= 0){
                FindObjectOfType<AudioManager>().Play("Drop");
            }
        }else{
            if(!playingAnimation && Time.time - creationTime > (ResourceAnimationManager.burnDelay + 1f)){
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
