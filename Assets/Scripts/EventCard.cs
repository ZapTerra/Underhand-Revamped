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

    public Card data;
    public Option choice1;
    public Option choice2;
    public Option choice3;

    //To allow effects like Foresight to activate once the burn animation completes
    public float BurnAnimationLength;
    public Image cardImage;
    public SpriteRenderer deckSprite;
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
    public Sprite unavailableChoice;
    public Sprite availableChoice;
    public Sprite expensiveChoice;
    public Sprite paidExpensiveChoice;
    public Sprite paidAvailableChoice;
    public List<Sprite> deckSprites = new List<Sprite>();
    public Animator animator;

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
        setCardBackground(background1, checkIfOptionIsPossible(data.option1, HandController.playerResources));
        setCardBackground(background2, checkIfOptionIsPossible(data.option2, HandController.playerResources));
        setCardBackground(background3, checkIfOptionIsPossible(data.option3, HandController.playerResources));
    }
    
    public void populateCard(){
        cardImage.sprite = data.eventSprite;

        bool lose1 = false;
        bool lose2 = false;
        bool lose3 = false;
        //I have to include this if I want to stay true to the original game because of a ridiculous dev workaround    
        if(data.option1.specialEffects.usesCustomOption){
            choice1 = data.option1.specialEffects.customOption.SpecialOption();
        }else{
            choice1 = data.option1;
        }
        if(choice1.specialEffects.useSpecialCost)
        choice1.cost.AddRange(data.option1.specialEffects.specialCost.ReturnSpecialValue());
        checkResult = checkIfOptionIsPossible(data.option1, HandController.playerResources);
        if(checkResult == 0){
            data.option1.exists = false;
        }
        if(data.option1.specialEffects.useSpecialEffect){
            if(data.option1.specialEffects.specialEffect.GetType().Name == "Lose"){
                data.option1.exists = false;
                lose1 = true;
            }
        }
        if(!data.option1.exists){
            checkResult = 0;
        }

        //I have to include this if I want to stay true to the original game because of a ridiculous dev workaround    
        if(data.option2.specialEffects.usesCustomOption){
            choice2 = data.option2.specialEffects.customOption.SpecialOption();
        }else{
            choice2 = data.option2;
        }
        if(choice2.specialEffects.useSpecialCost)
        choice2.cost.AddRange(data.option2.specialEffects.specialCost.ReturnSpecialValue());
        checkResult = checkIfOptionIsPossible(data.option2, HandController.playerResources);
        if(checkResult == 0){
            data.option2.exists = false;
        }
        if(data.option2.specialEffects.useSpecialEffect){
            if(data.option2.specialEffects.specialEffect.GetType().Name == "Lose"){
                data.option2.exists = false;
                lose2 = true;
            }
        }
        if(!data.option2.exists){
            checkResult = 0;
        }

        //I have to include this if I want to stay true to the original game because of a ridiculous dev workaround
        if(data.option3.specialEffects.usesCustomOption){
            choice3 = data.option3.specialEffects.customOption.SpecialOption();
        }else{
            choice3 = data.option3;
        }
        if(choice3.specialEffects.useSpecialCost)
        choice3.cost.AddRange(data.option3.specialEffects.specialCost.ReturnSpecialValue());
        checkResult = checkIfOptionIsPossible(data.option3, HandController.playerResources);
        if(checkResult == 0){
            data.option3.exists = false;
        }
        if(data.option3.specialEffects.useSpecialEffect){
            if(data.option3.specialEffects.specialEffect.GetType().Name == "Lose"){
                data.option3.exists = false;
                lose3 = true;
            }
        }
        if(!data.option3.exists){
            checkResult = 0;
        }

        //If the other options are not possible, losing options will be available.
        if(lose1 && !data.option2.exists && !data.option3.exists){
            lose1 = false;}else{lose1 = true;}
        if(lose2 && !data.option1.exists && !data.option3.exists){
            lose2 = false;}else{lose2 = true;}
        if(lose3 && !data.option1.exists && !data.option2.exists){
            lose3 = false;}else{lose3 = true;}
        if(!lose1){
            data.option1.exists = true;}
        if(!lose2){
            data.option2.exists = true;}
        if(!lose3){
            data.option3.exists = true;}


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
                foreach(Resource res in resourcesForSacrifice){
                    HandController.placedFieldResources.Remove(res);
                    HandController.fieldResources.Remove(res);
                    HandController.playerResources.Remove(res);
                }
                HandController.playerResources.AddRange(choice.reward);
                HandController.uninstantiatedResources.AddRange(choice.reward);
                FindObjectOfType<ResourceAnimationManager>().StartNewBurn(resourcesForSacrifice, true);

                StartCoroutine(DoEffectsAndAnimations(choice, resourcesForSacrifice.Count > 0));
                background1.GetComponent<ChangeColorOrSprite>().enabled = false;
                background2.GetComponent<ChangeColorOrSprite>().enabled = false;
                background3.GetComponent<ChangeColorOrSprite>().enabled = false;
                if(data.recurring){
                    animator.SetBool("Reshuffle", true);
                }else{
                    animator.SetBool("Discard", true);
                }
                if(choice.radioVoice != null){
                    Radio.soundBytes.Add(choice.radioVoice);
                }
            }else{
                //LAUGH AT THE PLAYER (nicely)
            }
        }
    }

    public IEnumerator DoEffectsAndAnimations(Option choice, bool resourcesWillBurn){
        //Determines whether the game waits for the burn animation before or after an effect activates, because other visuals are always delayed by a burn.
        //logic might be messy but I think it's good.
        if(choice.specialEffects.useSpecialEffect){
            if(choice.specialEffects.specialEffect.waitForBurn && resourcesWillBurn){
                yield return new WaitForSeconds(BurnAnimationLength);
            }
            if(choice.specialEffects.specialEffect.GetType().Name == "InsertCards"){
                animator.SetBool("Insert", true);
            }
            choice.specialEffects.specialEffect.SpecialEffect();
        }else if(resourcesWillBurn){
            yield return new WaitForSeconds(BurnAnimationLength);
        }
        if(FindObjectOfType<CrisisEventVisuals>() != null){
            FindObjectOfType<CrisisEventVisuals>().checkVisuals();
        }else{
            Debug.Log("No CrisisEventVisuals object found in the scene. HUD alerts will not appear.");
        }
        animator.Play("RefoldCard");
    }


    //Called at end of reshuffle animation
    public void DoNextCard(){
                FindObjectOfType<CardManager>().NextCard();
                Destroy(gameObject);
    }

    public void populateIconHolder(List<Resource> cost, GameObject holder, bool isCost){
        HandController.SortInput(cost);
        Resource currentResource = Resource.Relic;
        if(cost.Count > 0){
            currentResource = cost[0];
        }
        int currentStackCount = 0;
        int totalStackCount = 0;
        foreach(Resource re in cost){
            totalStackCount++;
            if(currentResource == re){
                currentStackCount++;
            }else{
                currentResource = re;
                currentStackCount = 1;
            }
            InstantiateAppropriateIcon(re, holder, currentStackCount, cost, isCost);
        }
        if(cost.Count == 0){
            Destroy(holder);
        }
    }

    public void setCardBackground(Image background, int evaluator){
        background.sprite =
         evaluator < 4 ? 
        (evaluator < 3 ? 
        (evaluator < 2 ? 
        (evaluator < 1  
        ? unavailableChoice
        : expensiveChoice)
        : availableChoice)
        : paidExpensiveChoice)
        : paidAvailableChoice;
    }

    public void InstantiateAppropriateIcon(Resource re, GameObject holder, int resourceOrder, List<Resource> cost, bool isCost){
        GameObject newIcon = Instantiate(resourcePrefab, holder.transform);
        newIcon.GetComponentInChildren<ResourceIcon>().resourceOrder = resourceOrder;
        newIcon.GetComponentInChildren<ResourceIcon>().costOption = cost;
        newIcon.GetComponentInChildren<ResourceIcon>().resourceType = re;
        newIcon.GetComponentInChildren<ResourceIcon>().isCost = isCost;
    }

    public int checkIfOptionIsPossible(Option choice, List<Resource> resources, bool secondRun = false){
        if(choice.exists){
            //I don't know if the new<> is necessary, will look up when I have internet
            List<Resource> tempList = new List<Resource>(HandController.SortInput(resources));
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
                    return 1 + (int)(secondRun ? 0 : (checkIfOptionIsPossible(choice, HandController.placedFieldResources, true) > 0 ? 2 : 0));
                }else{
                    return 2 + (int)(secondRun ? 0 : checkIfOptionIsPossible(choice, HandController.placedFieldResources, true));
                }
            }else{
                return 0;
            }
        }else{
            return 0;
        }
    }

    //Yes, all of these are necessary for the fancy animations.
    public void SetSpriteDeckNonzero(){
        if(CardManager.deck.Count == 0){
            deckSprite.sprite = deckSprites[1];
        }else{
            deckSprite.sprite = deckSprites[Mathf.Clamp(Mathf.CeilToInt((CardManager.deck.Count) / 3f), 0, deckSprites.Count - 1)];
        }
    }
    public void SetSpriteDeck(){
        deckSprite.sprite = deckSprites[Mathf.Clamp(Mathf.CeilToInt((CardManager.deck.Count) / 3f), 0, deckSprites.Count - 1)];
    }
    public void SetSpriteDiscard(){  
        deckSprite.sprite = deckSprites[Mathf.Clamp(Mathf.CeilToInt((CardManager.discardCountBeforeDraw) / 3f), 0, deckSprites.Count - 1)];
    }
    public void SetSpriteDiscardPlusReshuffled(){
        deckSprite.sprite = deckSprites[Mathf.Clamp(Mathf.CeilToInt((CardManager.discardCountBeforeDraw + 1) / 3f), 0, deckSprites.Count - 1)];
    }
    public void SetSpriteDiscardPlusInserts(){
        deckSprite.sprite = deckSprites[Mathf.Clamp(Mathf.CeilToInt((CardManager.discardPile.Count) / 3f), 0, deckSprites.Count - 1)];
    }

    public void TryTutorialMessageDraw(){
        Debug.Log("Attempted a tutorial overlay");
        // && PlayerPrefs.GetInt(data.tutorialOverlay.playerPrefsNameDraw, 0) == 0
        if(data.tutorialOverlay.overlayOnDraw){
            PlayerPrefs.SetInt(data.tutorialOverlay.playerPrefsNameDraw, 1);
            FindObjectOfType<ForesightAnimationManager>().newForesight(new List<Card>());
            Instantiate(data.tutorialOverlay.drawOverlay, GameObject.FindWithTag("PauseOverlayCanvas").transform);
        }
    }
    public void TryTutorialMessageUnfold(){
        // && PlayerPrefs.GetInt(data.tutorialOverlay.playerPrefsNameUnfold, 0) == 0
        if(data.tutorialOverlay.overlayOnUnfold){
            PlayerPrefs.SetInt(data.tutorialOverlay.playerPrefsNameUnfold, 1);
            FindObjectOfType<ForesightAnimationManager>().newForesight(new List<Card>());
            Instantiate(data.tutorialOverlay.unfoldOverlay, GameObject.FindWithTag("PauseOverlayCanvas").transform);
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
    public AudioClip radioVoice;
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

[System.Serializable]
public class TutorialOverlay {
    public string playerPrefsNameDraw;
    public string playerPrefsNameUnfold;
    public bool overlayOnDraw;
    public GameObject drawOverlay;
    public bool overlayOnUnfold;
    public GameObject unfoldOverlay;
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