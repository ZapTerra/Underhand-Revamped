using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Replace Cards", menuName = "Game Events/Replace Cards", order = 25)]
public class ReplaceCards : CustomEvent {
    public bool randomSelection;
    public bool noDuplicates;
    public List<Card> cardsToReplace = new List<Card>();
    public List<Card> cardsToInsert = new List<Card>();
    [Header("Add a rare insertion here, and specify its odds.")]
    [Header("Ex: Insert 'Rare Treasure!', one in 5 times")]
    //I know I can check this programmatically but it just feels right and I'm the primary user of this editor
    public bool hasRareInsert;
    public Card rareInsert;
    public int rareInsertOdds;
    [Header("If 0, no max amount")]
    public int maxRareInsertCount;
    private int rareInsertCount;

    //It's a headache, but it does what it says on the box.

    public override void SpecialEffect(){
        Random rnd = new Random();
        List<List<Card>> allCards = new List<List<Card>>(){CardManager.deck, CardManager.nextDeckTopCards, CardManager.discardPile, CardManager.banishedDiscard, CardManager.superBanishedDiscard, CardManager.ultraSuperBanishedDiscard};
        List<Card> tempInsertionList = new List<Card>(cardsToInsert);
        foreach(List<Card> l in allCards){
            List<Card> temp = new List<Card>(l);
            foreach(Card c in temp){
                foreach(Card r in cardsToReplace){
                    if(c == r){
                        Card selected;
                        if(!(hasRareInsert && rnd.Next(rareInsertOdds) < 1 && (rareInsertCount < maxRareInsertCount || maxRareInsertCount == 0))){
                            if(randomSelection){
                                selected = tempInsertionList[rnd.Next(tempInsertionList.Count)];
                            }else{
                                selected = tempInsertionList[0];
                            }
                        }else{
                            selected = rareInsert;
                            rareInsertCount++;
                        }
                        
                        if(noDuplicates || !randomSelection){
                            if(tempInsertionList.Count > 1){
                                temp.Remove(selected);
                            }
                        }
                        l.Insert(l.IndexOf(c), selected);
                        l.Remove(c);
                    }
                }
            }
        }
    }
}