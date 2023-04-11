using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCount : MonoBehaviour
{
    public TMPro.TextMeshProUGUI cardCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cardCount.text = HandController.cardObjectsInHand.Count.ToString();
    }
}
