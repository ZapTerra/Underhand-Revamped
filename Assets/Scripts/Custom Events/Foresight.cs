using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/Foresight", order = 6)]
public class Foresight : CustomEvent {
    public int scryCount;
    public bool foresightWithDiscard;
    public GameObject revealedCard;
    public override void SpecialEffect(){
        //Community voted against this feature. It's still implemented, if you'd like to turn it on by uncommenting this line.
        //Guarantees the future won't change once you've seen it.
        //CardManager.crisisImmunityFromForesight = scryCount + 1;
        List<Card> foresight = new(CardManager.GetForesightCards(scryCount));
        FindObjectOfType<ForesightAnimationManager>().newForesight(foresight);
        int counter = 0;
        foreach(Card c in foresight){
            var newCard = Instantiate(revealedCard);
            var cardScript = newCard.GetComponent<ForesightCardKinematics>();
            cardScript.card = c;
            cardScript.index = counter;
            cardScript.canDiscard = foresightWithDiscard;
            counter++;
        }
        Debug.Log("break");
    }
}