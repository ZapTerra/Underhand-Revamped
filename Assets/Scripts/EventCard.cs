using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Resources;

public class EventCard : MonoBehaviour
{
    public static bool choiceActivated;

    public Card eventCard;
    public Card data;
    public Option choice1;
    public Option choice2;
    public Option choice3;

    public Image cardImage;
    public Image bg1;
    public Image bg2;
    public Image bg3;
    public GameObject costArray1;
    public GameObject costArray2;
    public GameObject costArray3;
    public GameObject rewardArray1;
    public GameObject rewardArray2;
    public GameObject rewardArray3;
    public TMPro.TextMeshProUGUI text1;
    public TMPro.TextMeshProUGUI text2;
    public TMPro.TextMeshProUGUI text3;
    public TMPro.TextMeshProUGUI endText1;
    public TMPro.TextMeshProUGUI endText2;
    public TMPro.TextMeshProUGUI endText3;

    public GameObject resourcePrefab;

    public Image background1;
    public Image background2;
    public Image background3;
    public Sprite activeChoice;
    public Sprite availableChoice;
    public Sprite expensiveChoice;
    public Sprite unavailableChoice;

    private int checkResult = 0;

    // Start is called before the first frame update
    void Start()
    {
        populateCard();
        choiceActivated = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void populateCard(){
        cardImage.sprite = data.eventSprite;
    
        //I have to include this if I want to stay true to the original game because of a ridiculous dev workaround    
        if(data.option1.specialEffects.usesCustomOption){
            choice1 = data.option1.specialEffects.customOption.SpecialOption();
        }else{
            choice1 = data.option1;
        }
        if(choice1.specialEffects.useSpecialCost)
        choice1.cost.AddRange(data.option1.specialEffects.specialCost.ReturnSpecialValue());
        checkResult = checkIfOptionIsPossible(data.option1);
        if(checkResult == 0){
            data.option1.exists = false;
        }
        if(!data.option1.exists){
            checkResult = 0;
        }
        setCardBackground(background1, checkResult);

        //I have to include this if I want to stay true to the original game because of a ridiculous dev workaround    
        if(data.option2.specialEffects.usesCustomOption){
            choice2 = data.option2.specialEffects.customOption.SpecialOption();
        }else{
            choice2 = data.option2;
        }
        if(choice2.specialEffects.useSpecialCost)
        choice2.cost.AddRange(data.option2.specialEffects.specialCost.ReturnSpecialValue());
        checkResult = checkIfOptionIsPossible(data.option2);
        if(checkResult == 0){
            data.option2.exists = false;
        }
        if(!data.option2.exists){
            checkResult = 0;
        }
        setCardBackground(background2, checkResult);

        //I have to include this if I want to stay true to the original game because of a ridiculous dev workaround    
        if(data.option3.specialEffects.usesCustomOption){
            choice3 = data.option3.specialEffects.customOption.SpecialOption();
        }else{
            choice3 = data.option3;
        }
        if(choice3.specialEffects.useSpecialCost)
        choice3.cost.AddRange(data.option3.specialEffects.specialCost.ReturnSpecialValue());
        checkResult = checkIfOptionIsPossible(data.option3);
        if(checkResult == 0){
            data.option3.exists = false;
        }
        if(!data.option3.exists){
            checkResult = 0;
        }
        setCardBackground(background3, checkResult);


        text1.text = choice1.description;
        text2.text = choice2.description;
        text3.text = choice3.description;
        endText1.text = choice1.result;
        endText2.text = choice2.result;
        endText3.text = choice3.result;
        populateIconHolder(choice1.cost, costArray1, true);
        populateIconHolder(choice2.cost, costArray2, true);
        populateIconHolder(choice3.cost, costArray3, true);
        populateIconHolder(choice1.reward, rewardArray1, false);
        populateIconHolder(choice2.reward, rewardArray2, false);
        populateIconHolder(choice3.reward, rewardArray3, false);
    }

    public void ChooseOption1(){
        ActivateConsequences(choice1);
    }

    public void ChooseOption2(){
        ActivateConsequences(choice2);
    }

    public void ChooseOption3(){
        ActivateConsequences(choice3);
    }

    public void ActivateConsequences(Option choice){
        if(!choiceActivated && choice.exists){
            //I don't know if the new<> is necessary, will look up when I have internet
            List<Resource> tempList = new List<Resource>(HandController.SortInput(HandController.placedFieldResources));
            List<Resource> costList = new List<Resource>(choice.cost);
            List<Resource> resourcesForSacrifice = new List<Resource>();
            List<Resource> checkoff = new List<Resource>();

            tempList.Reverse();
            bool usesRelic;
            foreach(Resource re in costList){
                List<Resource> tempTemp = new List<Resource>(tempList);
                foreach(Resource check in tempTemp){
                    if(MatchResource.OneToTwoLowValue(check, re)){
                        tempList.Remove(check);
                        resourcesForSacrifice.Add(check);
                        checkoff.Add(re);
                        break;
                    }else if(MatchResource.OneToTwoNoRelic(check, re)){
                        tempList.Remove(check);
                        resourcesForSacrifice.Add(check);
                        checkoff.Add(re);
                        break;
                    }else if(MatchResource.OneToTwo(check, re)){
                        tempList.Remove(check);
                        resourcesForSacrifice.Add(check);
                        checkoff.Add(re);
                        usesRelic = true;
                        break;
                    }
                }
            }
            
            if(checkoff.SequenceEqual(choice.cost)){
                choiceActivated = true;
                FindObjectOfType<ResourceAnimationManager>().startNewBurn(resourcesForSacrifice);
                foreach(Resource res in resourcesForSacrifice){
                    HandController.placedFieldResources.Remove(res);
                    HandController.fieldResources.Remove(res);
                    HandController.playerResources.Remove(res);
                }
                HandController.playerResources.AddRange(choice.reward);
                HandController.handResources.AddRange(choice.reward);
                if(choice.specialEffects.useSpecialEffect){
                    choice.specialEffects.specialEffect.SpecialEffect();
                }

                FindObjectOfType<HandController>().UpdateHandContents();
                FindObjectOfType<HandController>().updateHandVisuals();
                FindObjectOfType<CardManager>().NextCard();
                Destroy(gameObject);
            }else{
                //LAUGH AT THE PLAYER (nicely)
            }
        }
    }

    public void populateIconHolder(List<Resource> cost, GameObject holder, bool isCost){
        HandController.SortInput(cost);
        Resource currentResource = Resource.Relic;
        if(cost.Count > 0){
            currentResource = cost[0];
        }
        int currentStackCount = 0;
        foreach(Resource re in cost){
            if(currentResource == re){
                currentStackCount++;
            }else{
                currentResource = re;
                currentStackCount = 1;
            }
            InstantiateAppropriateIcon(re, holder, currentStackCount, isCost);
        }
    }

    public void setCardBackground(Image background, int evaluator){
        background.sprite = evaluator > 0 ? (evaluator > 1 ? availableChoice : expensiveChoice) : unavailableChoice;
    }

    public void InstantiateAppropriateIcon(Resource re, GameObject holder, int displayOrder, bool isCost){
        GameObject newIcon = Instantiate(resourcePrefab, holder.transform);
        newIcon.GetComponentInChildren<ResourceIcon>().displayOrder = displayOrder;
        newIcon.GetComponentInChildren<ResourceIcon>().resourceType = re;
        newIcon.GetComponentInChildren<ResourceIcon>().isCost = isCost;
    }

    public int checkIfOptionIsPossible(Option choice){
        //I don't know if the new<> is necessary, will look up when I have internet
        List<Resource> tempList = new List<Resource>(HandController.SortInput(HandController.playerResources));
        List<Resource> costList = new List<Resource>(choice.cost);
        List<Resource> resourcesForSacrifice = new List<Resource>();
        List<Resource> checkoff = new List<Resource>();

        tempList.Reverse();
        bool usesRelic = false;
        foreach(Resource re in costList){
            List<Resource> tempTemp = new List<Resource>(tempList);
            foreach(Resource check in tempTemp){
                if(MatchResource.OneToTwoLowValue(check, re)){
                    tempList.Remove(check);
                    resourcesForSacrifice.Add(check);
                    checkoff.Add(re);
                    break;
                }else if(MatchResource.OneToTwoNoRelic(check, re)){
                    tempList.Remove(check);
                    resourcesForSacrifice.Add(check);
                    checkoff.Add(re);
                    break;
                }else if(MatchResource.OneToTwo(check, re)){
                    tempList.Remove(check);
                    resourcesForSacrifice.Add(check);
                    checkoff.Add(re);
                    usesRelic = true;
                    break;
                }
            }
        }
        if(checkoff.SequenceEqual(choice.cost)){
            if(usesRelic){
                return 1;
            }else{
                return 2;
            }
        }else{
            return 0;
        }
    }
}

[System.Serializable]
public class Option
{
    public bool exists;
    public string description;
    public List<Resource> cost = new List<Resource>();
    public List<Resource> reward = new List<Resource>();
    public string result;
    public SpecialEffects specialEffects;
}

[System.Serializable]
public class SpecialEffects {
    public bool useSpecialCost;
    public CustomEvent specialCost;
    public bool useSpecialReward;
    public CustomEvent specialReward;
    public bool useSpecialEffect;
    public CustomEvent specialEffect;
    public bool usesCustomOption;
    public CustomEvent customOption;
}

namespace Resources
{
    public enum Resource
    {
        Relic,
        Cultist,
        Money,
        Food,
        Prisoner,
        Human,
        Suspicion
    }
}