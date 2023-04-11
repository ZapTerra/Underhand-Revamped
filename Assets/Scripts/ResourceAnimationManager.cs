using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Resources;

//I know that this is all kind of bad but I'm a one man development team and it works the same as doing it well but using slightly more resources
//Plus it's MUCH easier to visualize where the cards are going to go while in the editor this way
public class ResourceAnimationManager : MonoBehaviour
{
    public List<List<GameObject>> burnArray = new List<List<GameObject>>();
    //Unity can't serialize a list of lists apparently so I gotta do this manually
    public List<GameObject> array1 = new List<GameObject>();
    public List<GameObject> array2 = new List<GameObject>();
    public List<GameObject> array3 = new List<GameObject>();
    public List<GameObject> array4 = new List<GameObject>();
    public List<GameObject> array5 = new List<GameObject>();
    public List<Resource> burnArrangement = new List<Resource>();
    public GameObject newCardAnimationPrefab;
    public Animator circleAnimator;
    public HandController handController;
    private int burnCount = 0;
    public static float burnDelay = 1f;
    public static float returnDelay = 3f;
    public static float burnInTime = 21/60f;
    public List<Resource> optionCost = new List<Resource>();

    // Start is called before the first frame update
    void Start()
    {
        burnArray.Add(array1);
        burnArray.Add(array2);
        burnArray.Add(array3);
        burnArray.Add(array4);
        burnArray.Add(array5);
    }

    // Update is called once per frame
    void Update()
    {

    }


    //badly organized and maybe suboptimal goodnight
    public void StartNewBurn(List<Resource> cost, bool newCall = false){
        HandController.SortInput(cost);
        burnArrangement = new List<Resource>(cost);
        optionCost = new List<Resource>(cost);
        burnCount = cost.Count;
        if(newCall){
            if(cost.Count > 0){
                Debug.Log("Source of burn animation and its delay");
                StartCoroutine(DelayRewardAnimation());
                StartCoroutine(BurnAnimation());
            }else{
                RewardAnimation();
            }
        }
    }

    public IEnumerator BurnAnimation(){
        yield return new WaitForSeconds(burnDelay);
        circleAnimator.Play("Hellfire");
        FindObjectOfType<AudioManager>().Play("Sacrifice");
    }

    public IEnumerator DelayRewardAnimation(){
        yield return new WaitForSeconds(returnDelay);
        RewardAnimation();
    }
    
    public void RewardAnimation(){
        StartNewBurn(HandController.uninstantiatedResources);
        Debug.Log(HandController.uninstantiatedResources.Count());
        foreach(Resource r in HandController.uninstantiatedResources){
            var newCardVisual = Instantiate(newCardAnimationPrefab);
            CardBurnKinematics k = newCardVisual.GetComponent<CardBurnKinematics>();
            k.rewardResource = true;
            k.resourceType = r;
        }
        StartCoroutine(GetNewCardsOnDelay());
    }

    public IEnumerator GetNewCardsOnDelay(){
        yield return new WaitForSeconds(burnInTime);
        StartNewBurn(HandController.uninstantiatedResources);
        HandController.handResources.AddRange(HandController.uninstantiatedResources);
        HandController.uninstantiatedResources.Clear();
        handController.UpdateHandContents(2);
    }

    public Vector3 giveMePositionBurn(Resource type, bool final = false){
        if(burnArray.Count >= burnCount){
            Debug.Log(burnArrangement.Count);
            burnArrangement.Remove(type);
            Debug.Log(burnArrangement.Count);
            Debug.Log(burnArrangement.Count + " " + final.ToString() + " " + type);
            return burnArray[burnCount - 1][optionCost.IndexOf(type) + (burnArrangement.Count(x => MatchResource.OneToTwoNoRelic(x, type)))].transform.position;
        }

        return Vector3.zero;
    }
}
