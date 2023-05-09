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
    public GameObject foodTutorial;
    public GameObject suspicionTutorial;
    public GameObject greedTutorial;
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
                if(PlayerPrefs.GetInt("desperatemeasures") == 0){
                    PlayerPrefs.SetInt("desperatemeasures", 1);
                    FindObjectOfType<ForesightAnimationManager>().newForesight(new List<Card>());
                    Instantiate(foodTutorial, GameObject.FindWithTag("PauseOverlayCanvas").transform);
                }
                StartCoroutine(Blinky(foodIndicator));
                foodAnimated = true;
            }
        }else{
            foodIndicator.SetActive(false);
            foodAnimated = false;
        }
        if(HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Suspicion)) > 4){
            if(!suspicionAnimated){
                if(PlayerPrefs.GetInt("policeraid") == 0){
                    PlayerPrefs.SetInt("policeraid", 1);
                    FindObjectOfType<ForesightAnimationManager>().newForesight(new List<Card>());
                    Instantiate(suspicionTutorial, GameObject.FindWithTag("PauseOverlayCanvas").transform);
                }
                StartCoroutine(Blinky(suspicionIndicator));
                suspicionAnimated = true;
            }
        }else{
            suspicionIndicator.SetActive(false);
            suspicionAnimated = false;
        }
        if(HandController.playerResources.Count > 15){
            if(!greedAnimated){
                if(PlayerPrefs.GetInt("greed") == 0){
                    PlayerPrefs.SetInt("greed", 1);
                    FindObjectOfType<ForesightAnimationManager>().newForesight(new List<Card>());
                    Instantiate(greedTutorial, GameObject.FindWithTag("PauseOverlayCanvas").transform);
                }
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
