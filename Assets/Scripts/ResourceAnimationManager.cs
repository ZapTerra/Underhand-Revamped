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
    public List<GameObject> array = new List<GameObject>();
    public List<Resource> arrangement = new List<Resource>();
    public List<Resource> optionCost = new List<Resource>();
    private int burnCount = 0;

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

    public void startNewBurn(List<Resource> cost){
        HandController.SortInput(cost);
        arrangement = new List<Resource>(cost);
        optionCost = new List<Resource>(cost);
        burnCount = cost.Count;
    }

    public Vector3 giveMePosition(Resource type){
        if(burnArray.Count >= burnCount){
            arrangement.Remove(type);
            return burnArray[burnCount - 1][optionCost.IndexOf(type) + (arrangement.Count(x => MatchResource.OneToTwoNoRelic(x, type)))].transform.position;
        }
        return Vector3.zero;
    }
}
