using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Resources;

public class CrisisEventVisuals : MonoBehaviour
{
    public float blinkyTime;
    public int blinkCount;
    public GameObject foodIndicator;
    public GameObject suspicionIndicator;
    public GameObject greedIndicator;
    private bool foodAnimated;
    private bool suspicionAnimated;
    private bool greedAnimated;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkVisuals(){
        if(HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Food)) == 0){
            if(!foodAnimated){
                StartCoroutine(Blinky(foodIndicator));
                foodAnimated = true;
            }
        }else{
            foodIndicator.SetActive(false);
            foodAnimated = false;
        }
        if(HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Suspicion)) > 4){
            if(!suspicionAnimated){
                StartCoroutine(Blinky(suspicionIndicator));
                suspicionAnimated = true;
            }
        }else{
            suspicionIndicator.SetActive(false);
            suspicionAnimated = false;
        }
        if(HandController.playerResources.Count > 15){
            if(!greedAnimated){
                StartCoroutine(Blinky(greedIndicator));
                greedAnimated = true;
            }
        }else{
            greedIndicator.SetActive(false);
            greedAnimated = false;
        }
    }

    private IEnumerator Blinky(GameObject game){
        for(int i = 1; i < blinkCount; i++){
            game.SetActive(true);
            yield return new WaitForSeconds(blinkyTime);
            game.SetActive(false);
            yield return new WaitForSeconds(blinkyTime);
        }
        game.SetActive(true);
    }
}
