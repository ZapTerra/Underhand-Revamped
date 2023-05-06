using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;
using KaimiraGames;
using Resources;

public class CardManager : MonoBehaviour
{
    public List<Card> inspectorDeck = new List<Card>();
    public static List<Card> deck = new List<Card>();
    //contains cards which won't come back for two shuffles, such as a kidnapped Tax Collector
    public static List<Card> banishedDiscard = new List<Card>();
    //contains cards which won't come back for three shuffles
    public static List<Card> superBanishedDiscard = new List<Card>();
    //contains cards which won't come back for four shuffles
    public static List<Card> ultraSuperBanishedDiscard = new List<Card>();
    public static List<Card> nextDeckTopCards = new List<Card>();
    public static List<Card> discardPile = new List<Card>();
    public int basicEventInsertionCount = 10;
    public List<Card> basicEvents = new List<Card>();
    public bool insertGodChains = true;
    public List<GodNameWrapper> tieredGodNames = new List<GodNameWrapper>();
    public List<Card> godChainBeginnings = new List<Card>();
    public bool insertCrisisEvents = true;
    public Card desperateMeasures;
    public Card policeRaid;
    public Card greed;
    public GameObject eventCardPrefab;
    public bool beginImmediately;
    public static int crisisImmunityFromForesight;
    private static bool attempedDesperateMeasuresInsert;
    private static bool attemptedPoliceRaidInsert;
    private static bool attemptedGreedInsert;
    public static List<int> foresightDiscardIndices = new();
    

    void OnEnable(){
        deck.Clear();
        banishedDiscard.Clear();
        superBanishedDiscard.Clear();
        ultraSuperBanishedDiscard.Clear();
        nextDeckTopCards.Clear();
        discardPile.Clear();
        discardPile = new List<Card>(inspectorDeck);
        attempedDesperateMeasuresInsert = false;
        attemptedPoliceRaidInsert = false;
        attemptedGreedInsert = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        //This lets the tutorial skip past the blessings screen.
        if(beginImmediately){
            ResetTimeScaleAndBegin();
        }
    }

    public void ResetTimeScaleAndBegin(){
        //WARUDO ゴ
        //   ゴ
        //ゴ
        List<Card> temp = new List<Card>(basicEvents);
        Random rnd = new Random();

        //Go through the tiers from 0 onward, inserting all god beginnings from each tier
        //Then, if the player hasn't summoned all the gods in the current tier yet don't insert the next one.
        if(insertGodChains){

        bool insertTier = true;
        int count = 0;
        for(int i = 0; i < tieredGodNames.Count; i++){
            if(insertTier){
                for(int x = 0; x < tieredGodNames[i].godList.Count; x++){
                    if(PlayerPrefs.GetInt(tieredGodNames[i].godList[x]) < 1){
                        insertTier = false;
                    }

                    //wiindigoo doesn't insert at beginning of the game
                    if(godChainBeginnings[count] != null){
                        discardPile.Add(godChainBeginnings[count]);
                    }
                    count++;
                }
            }
        }
        
        }


        for(int i = 0; i < basicEventInsertionCount; i++){
            int index = rnd.Next(1, temp.Count);
            discardPile.Add(temp[index]);
            temp.RemoveAt(index);
        }
        Time.timeScale = 1;
        NextCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextCard(){
        if(Time.timeScale > 0){
            if(deck.Count > 0){
                DrawCard();
            }else{
                Debug.Log("Reshuffling");
                deck = new List<Card>(ReshuffleDeck());
                nextDeckTopCards = new();
                discardPile.Clear();
                DrawCard();
            }
        }
    }

    void DrawCard(){
        if(insertCrisisEvents){
            CheckCrisisEvents();
        }
        inspectorDeck = new List<Card>(deck);
        crisisImmunityFromForesight--;
        var card = Instantiate(eventCardPrefab);
        if(deck.Count > 0){
            card.GetComponent<EventCard>().data = Instantiate(deck[0]);
            if(deck[0].recurring && !discardPile.Contains(deck[0])){
                discardPile.Add(deck[0]);
            }
            deck.RemoveAt(0);
        }
    }

    public static List<Card> ReshuffleDeck(){
        attempedDesperateMeasuresInsert = false;
        attemptedPoliceRaidInsert = false;
        attemptedGreedInsert = false;
        
        List<Card> returnDeck = new(nextDeckTopCards);

        WeightedList<Card> newDeck = new();
        newDeck.BadWeightErrorHandling = WeightErrorHandlingType.ThrowExceptionOnAdd;
        foreach(Card card in discardPile){
            newDeck.Add(card, card.weight);
        }

        while(newDeck.Count > 0){
            var drawnCard = newDeck.Next();
            returnDeck.Add(drawnCard);
            newDeck.Remove(drawnCard);
        }

        discardPile.AddRange(banishedDiscard);
        banishedDiscard.Clear();
        banishedDiscard.AddRange(superBanishedDiscard);
        superBanishedDiscard.Clear();
        superBanishedDiscard.AddRange(ultraSuperBanishedDiscard);
        ultraSuperBanishedDiscard.Clear();

        return returnDeck;
    }

    public static List<Card> GetForesightCards(int scry){
        List<Card> foresightDeck = new();
        if(deck.Count >= scry){
            foresightDeck.AddRange(deck.GetRange(0, scry));
        }else{
            //If there are not enough cards in the current deck to look at, set aside cards from the discard pile to put on top of the next deck.
            //If a previous foresight set aside cards, but more cards need to be set aside, add onto the set-aside pile.
            //When the deck is reshuffled, the set-aside pile is placed on top.
            //Then return the cards the player should see.
            //If they were shown more cards in the set-aside pile by a previous foresight than this current one will show,
            //only show as many from the set-aside pile as this foresight lets them.
            int remaining = Mathf.Clamp(scry - deck.Count - nextDeckTopCards.Count, 0, int.MaxValue);
            discardPile = new(ReshuffleDeck());
            Debug.Log("Remaining: " + remaining);
            Debug.Log("Scry: " + scry);
            Debug.Log("Deck cards: " + deck.Count);
            Debug.Log("Cards in Discard Pile: " + discardPile.Count);
            if(remaining <= discardPile.Count){
                foresightDeck.AddRange(deck);
                nextDeckTopCards.AddRange(discardPile.GetRange(0, remaining));
                foresightDeck.AddRange(nextDeckTopCards.GetRange(0, scry - deck.Count));
                discardPile.RemoveRange(0, remaining);
            }else{
                //If the discard has so few cards that sufficient cards for foresight cannot be drawn from it,
                //add the deck, previously revealed cards, and a shuffled discard pile to the revealed cards,
                //and add the entire shuffled discard to the set-aside pile, clearing the discard because its order is now set.
                //Leads to strange behavior when there are very few cards in play
                foresightDeck.AddRange(deck);
                foresightDeck.AddRange(nextDeckTopCards);
                foresightDeck.AddRange(discardPile);
                nextDeckTopCards.AddRange(discardPile);
                discardPile.Clear();
            }
            Debug.Log("Next Deck Set Cards: " + nextDeckTopCards.Count);
        }
        return foresightDeck;
    }

    //Remove cards from the deck and set-aside pile which the player has chosen to discard.
    //Indices which are out of bounds for the deck have reached into the set-aside pile, remove the cards from there.
    public static void PopForesightDiscards(){
        foresightDiscardIndices.Sort();
        foresightDiscardIndices.Reverse();
        List<Card> temp = new List<Card>(deck);
        List<Card> temp2 = new List<Card>(nextDeckTopCards);
        foreach(int i in foresightDiscardIndices){
            if(i < deck.Count){
                discardPile.Add(deck[i]);
                temp.RemoveAt(i);
            }else{
                discardPile.Add(nextDeckTopCards[i - deck.Count]);
                temp2.RemoveAt(i - deck.Count);
            }
        }
        deck = new List<Card>(temp);
        nextDeckTopCards = new List<Card>(temp2);
        foresightDiscardIndices.Clear();
    }

    void CheckCrisisEvents(){
        if(HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Food)) == 0 && !attempedDesperateMeasuresInsert && crisisImmunityFromForesight < 1){
            InsertCrisis(desperateMeasures);
            attempedDesperateMeasuresInsert = true;
        }else if(deck.Contains(desperateMeasures) && HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Food)) > 0){
            deck.Remove(desperateMeasures);
        }
        if(HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Suspicion)) > 4 && !attemptedPoliceRaidInsert && crisisImmunityFromForesight < 1){
            InsertCrisis(policeRaid);
            attemptedPoliceRaidInsert = true;
        }else if(deck.Contains(policeRaid) && HandController.playerResources.Count(x => MatchResource.OneToTwoNoRelic(x, Resource.Suspicion)) < 5){
            deck.Remove(policeRaid);
        }
        if(HandController.playerResources.Count > 15 && !attemptedGreedInsert && crisisImmunityFromForesight < 1){
            InsertCrisis(greed);
            attemptedGreedInsert = true;
        }else if(deck.Contains(greed) && HandController.playerResources.Count < 16){
            deck.Remove(greed);
        }
    }

    void InsertCrisis(Card crisis){
        if(!deck.Contains(crisis)){
            int totalWeight = 0;
            foreach(Card item in deck){
                totalWeight += item.weight;
            }
            foreach(Card item in discardPile){
                if(item.weight < 26){
                    totalWeight += item.weight;
                }
            }
            totalWeight += crisis.weight;
            Random rnd = new Random();
            for(int i = 0; i < deck.Count + 1; i++){
                if(rnd.Next(totalWeight) - 1 > crisis.weight){
                    deck.Insert(i, crisis);
                    break;
                }
            }
        }
    }
}

[System.Serializable]
 public class GodNameWrapper
 {
      public List<string> godList;
 }