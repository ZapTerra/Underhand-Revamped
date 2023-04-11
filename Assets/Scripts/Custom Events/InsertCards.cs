using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/InsertCards", order = 2)]
public class InsertCards : CustomEvent {

    public bool randomSelection;
    [Header("0 ok, treats as 1")]
    public int insertionCount;
    public bool allowDuplicates;
    public bool dontInsertIfExistsAlready;
    public List<Card> cardsToInsert = new List<Card>();
    //I know I can check this programmatically but it just feels right and I'm the primary user of this editor
    [Header("Add a special effect here, and specify its odds.")]
    [Header("Ex: Insert Locusts, one in 15 times")]
    public bool hasRareEvent;
    public int rareEventOdds;
    public CustomEvent rareEvent;


    //The rare event stuff is shoehorned in there
    //I feel bad about it but I'm also done
    public override void SpecialEffect(){
        if(insertionCount == 0){
            insertionCount = 1;
        }
        Random rnd = new Random();
        List<Card> cards = new List<Card>();
        if(!randomSelection){
            foreach(Card card in cardsToInsert){
                if(!(hasRareEvent && rnd.Next(rareEventOdds) < 1)){
                    cards.Add(card);
                }else{
                    rareEvent.SpecialEffect();
                }
            }
        }else{
            if(allowDuplicates){
                for(int i = 0; i < insertionCount; i++){       
                    if(!(hasRareEvent && rnd.Next(rareEventOdds) < 1)){
                        cards.Add(cardsToInsert[rnd.Next(cardsToInsert.Count)]);
                    }else{
                        rareEvent.SpecialEffect();
                    }
                }
            }else{
                List<Card> temp = new List<Card>(cardsToInsert);
                for(int i = 0; i < insertionCount; i++){
                    if(!(hasRareEvent && rnd.Next(rareEventOdds) < 1)){
                        Card selected = temp[rnd.Next(temp.Count)];
                        if(temp.Count > 1){
                            temp.Remove(selected);
                        }
                        cards.Add(selected);
                    }else{
                        rareEvent.SpecialEffect();
                    }
                }
            }
        }
        if(dontInsertIfExistsAlready){
            foreach(Card card in new List<Card>(cards)){
                if(CardManager.deck.Contains(card) || CardManager.nextDeckTopCards.Contains(card) || CardManager.discardPile.Contains(card) || CardManager.banishedDiscard.Contains(card) || CardManager.superBanishedDiscard.Contains(card) || CardManager.ultraSuperBanishedDiscard.Contains(card)){
                    cards.Remove(card);
                }
            }
        }
        CardManager.discardPile.AddRange(cards);
    }
}