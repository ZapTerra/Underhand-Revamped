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
    public static List<Card> reshuffleDiscard = new List<Card>();
    public Card desperateMeasures;
    public Card policeRaid;
    public Card greed;
    public GameObject eventCardPrefab;
    private bool attempedDesperateMeasuresInsert;
    private bool attemptedPoliceRaidInsert;
    private bool attemptedGreedInsert;

    void OnEnable(){
        reshuffleDiscard = new List<Card>(inspectorDeck);
    }
    // Start is called before the first frame update
    void Start()
    {
        NextCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextCard(){

        if(deck.Count > 0){
            DrawCard();
        }else{
            attempedDesperateMeasuresInsert = false;
            attemptedPoliceRaidInsert = false;
            attemptedGreedInsert = false;
            
            WeightedList<Card> newDeck = new();
            newDeck.BadWeightErrorHandling = WeightErrorHandlingType.ThrowExceptionOnAdd;
            foreach(Card card in reshuffleDiscard){
                newDeck.Add(card, card.weight);
            }

            deck.Clear();
            reshuffleDiscard.Clear();

            while(newDeck.Count > 0){
                var drawnCard = newDeck.Next();
                deck.Add(drawnCard);
                newDeck.Remove(drawnCard);
            }
            DrawCard();
        }
    }

    void DrawCard(){
        CheckCrisisEvents();
        var card = Instantiate(eventCardPrefab);
        card.GetComponent<EventCard>().data = Instantiate(deck[0]);
        if(deck[0].recurring){
            reshuffleDiscard.Add(deck[0]);
        }
        deck.RemoveAt(0);
    }

    void CheckCrisisEvents(){
        if(HandController.playerResources.Count(x => MatchResource.OneToTwo(x, Resource.Food)) == 0 && !attempedDesperateMeasuresInsert){
            InsertCrisis(desperateMeasures);
            attempedDesperateMeasuresInsert = true;
        }else if(deck.Contains(desperateMeasures)){
            deck.Remove(desperateMeasures);
        }
        if(HandController.playerResources.Count(x => MatchResource.OneToTwo(x, Resource.Suspicion)) > 5 && !attemptedPoliceRaidInsert){
            InsertCrisis(policeRaid);
            attemptedPoliceRaidInsert = true;
        }else if(deck.Contains(policeRaid)){
            deck.Remove(policeRaid);
        }
        if(HandController.playerResources.Count > 15 && !attemptedGreedInsert){
            InsertCrisis(greed);
            attemptedGreedInsert = true;
        }else if(deck.Contains(greed)){
            deck.Remove(greed);
        }
    }

    void InsertCrisis(Card crisis){
        if(!deck.Contains(crisis)){
            int totalWeight = 0;
            foreach(Card item in deck){
                totalWeight += item.weight;
            }
            foreach(Card item in reshuffleDiscard){
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


//Random Event Cards
