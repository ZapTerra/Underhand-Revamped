using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using Resources;

[CreateAssetMenu(fileName = "Event", menuName = "Game Events/InsertCards", order = 1)]
public class InsertCards : CustomEvent {

    public bool randomSelection;
    public int insertionCount;
    public bool allowDuplicates;
    public List<Card> cardsToInsert = new List<Card>();

    public override void SpecialEffect(){
        if(!randomSelection){
            foreach(Card card in cardsToInsert){
                CardManager.reshuffleDiscard.Add(card);
            }
        }else{
            if(allowDuplicates){
                for(int i = 0; i < insertionCount; i++){
                    Random rnd = new Random();
                    CardManager.reshuffleDiscard.Add(cardsToInsert[rnd.Next(cardsToInsert.Count)]);
                }
            }else{
                List<Card> temp = new List<Card>(cardsToInsert);
                for(int i = 0; i < insertionCount; i++){
                    Random rnd = new Random();
                    Card selected = temp[rnd.Next(temp.Count)];
                    temp.Remove(selected);
                    CardManager.reshuffleDiscard.Add(selected);
                }
            }
        }
    }
}