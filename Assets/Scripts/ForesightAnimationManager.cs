using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForesightAnimationManager : MonoBehaviour
{
    public List<List<GameObject>> placementArray = new List<List<GameObject>>();
    //Unity can't serialize a list of lists apparently so I gotta do this manually
    public List<GameObject> array1 = new List<GameObject>();
    public List<GameObject> array2 = new List<GameObject>();
    public List<GameObject> array3 = new List<GameObject>();
    public GameObject overlay;
    public GameObject exitButton;
    public static int scryCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        overlay.SetActive(false);
        placementArray.Add(array1);
        placementArray.Add(array2);
        placementArray.Add(array3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newForesight(List<Card> cards){
        overlay.SetActive(true);
        exitButton.SetActive(true);
        Time.timeScale = 0;
        scryCount = cards.Count;
    }

    public void EndForesight(){
        exitButton.SetActive(false);

        //might be bad? clean and effective.
        Debug.Log("Killing all objects with an attached foresight kinematics script");
        var foresightCardGraphics = FindObjectsOfType<ForesightCardKinematics>();
        int count = 0;
        foreach(var c in foresightCardGraphics){
            StartCoroutine(c.animateAway((foresightCardGraphics.Length - 1 - count) / 2));
            Destroy(c, 5f);
            count++;
        }
        Debug.Log("Discarding cards selected by player (If any)");
        CardManager.PopForesightDiscards();
        StartCoroutine(EndForesightCoroutine(foresightCardGraphics.Length));
    }

    private IEnumerator EndForesightCoroutine(float delay){
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
        overlay.SetActive(false);
    }

    public Vector3 giveMePosition(int index){
        //I have no clue why this would ever be called with a scrycount of 0 or less, but just to be safe lol
        if(placementArray.Count < scryCount || scryCount <= 0){
            Debug.Log("No foresight card layout for this many cards");
            return Vector3.zero;
        }
        return placementArray[scryCount - 1][index].transform.position;
    }
}
