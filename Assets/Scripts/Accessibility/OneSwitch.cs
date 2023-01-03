using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OneSwitch : MonoBehaviour
{
    [SerializeField] private List<Targetable> targetableRightNow = new List<Targetable>();
    [SerializeField] private Targetable[] thingsToTargetInNormalSituation;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    [SerializeField] private Targetable[] thingsToTargetShop;
    [SerializeField] private Targetable[] thingsToTargetSettingsMenu;
    [SerializeField] private Targetable[] thingsToTargetOptionsMenu;
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject[] whatToCheck; 

    private int index = -1;
    private int prevIndex = 0;
    private int indexTargets = 0; 
    private bool clicked = false;
    private bool firstTime = true;

    public WhatShouldBeOneSwitch oneSwitchActiveNow;
    private WhatShouldBeOneSwitch oneSwitchActivePrevious;

    private PlayCardManager playCardManager;
    private Choice choice;

    private void Start()
    {
        playCardManager = PlayCardManager.Instance;
        choice = Choice.Instance;
    }

    private void HideTarget()
    {
        if (index - 1 < 0) return;
        
        if (targetableRightNow[prevIndex].TryGetComponent(out CardDisplay cardDisplayPrevious))
            cardDisplayPrevious.MouseExit();
        if (targetableRightNow[prevIndex].TryGetComponent(out ChoiceButton choiceButtonPrev))
            choiceButtonPrev.OneSwitchHoverChoice();
        if (targetableRightNow[prevIndex].TryGetComponent(out PurchaseCard purchaseCardPrev))
            purchaseCardPrev.OneSwitchHover();
        if (targetableRightNow[prevIndex].TryGetComponent(out ReturnButton returnButtonPrev))
            returnButtonPrev.OneSwitchButtonSelect();
        if (targetableRightNow[prevIndex].TryGetComponent(out ShopHover shopHoverPrev))
            shopHoverPrev.OneSwitchSelected();
        prevIndex = index;
    }
    private void ShowTarget()
    {
        if (targetableRightNow[index].TryGetComponent(out CardDisplay cardDisplayShow))
            cardDisplayShow.MouseEnter();
        if (targetableRightNow[index].TryGetComponent(out ChoiceButton choiceButton))
            choiceButton.OneSwitchHoverChoice();
        if (targetableRightNow[index].TryGetComponent(out Button confirmButton))
            confirmButton.Select();
        if (targetableRightNow[index].TryGetComponent(out PurchaseCard purchaseCardPrev))
            purchaseCardPrev.OneSwitchHover();
        if (targetableRightNow[index].TryGetComponent(out ReturnButton returnButtonPrev))
            returnButtonPrev.OneSwitchButtonSelect();
        if (targetableRightNow[index].TryGetComponent(out ShopHover shopHoverPrev))
            shopHoverPrev.OneSwitchSelected();
    }

    private void ResetLast()
    {
        int length = targetableRightNow.Count - 1;
        prevIndex = 0;

        if (targetableRightNow[length].TryGetComponent(out CardDisplay cardDisplayLastReset))
            cardDisplayLastReset.MouseExit();
        if (targetableRightNow[length].TryGetComponent(out ChoiceButton choiceButtonLastReset))
            choiceButtonLastReset.OneSwitchHoverChoice();
        if (targetableRightNow[length].TryGetComponent(out PurchaseCard purchaseCardLastReset))
            purchaseCardLastReset.OneSwitchHover();
        if (targetableRightNow[length].TryGetComponent(out ReturnButton returnButtonLastReset))
            returnButtonLastReset.OneSwitchButtonSelect();
        if (targetableRightNow[length].TryGetComponent(out ShopHover shopHoverLastReset))
            shopHoverLastReset.OneSwitchSelected();
        eventSystem.SetSelectedGameObject(null);
    }

    private void ChangeOneSwitch()
    {
        CheckWhatIsActive();

        switch (oneSwitchActiveNow)
        {
            case WhatShouldBeOneSwitch.Normal:
                targetableRightNow = thingsToTargetInNormalSituation.ToList();
                break;
            case WhatShouldBeOneSwitch.Choice:
                ChoiceAllternatives();
                break;
            case WhatShouldBeOneSwitch.Shop:
                targetableRightNow = thingsToTargetShop.ToList();
                break;
            case WhatShouldBeOneSwitch.CardTarget:
                targetableRightNow = thingsToTargetWithCard.ToList();
                break;
            case WhatShouldBeOneSwitch.OptionMenu:
                targetableRightNow = thingsToTargetOptionsMenu.ToList();
                break;
            case WhatShouldBeOneSwitch.RuleBook:
                break;
            case WhatShouldBeOneSwitch.Settings:
                targetableRightNow = thingsToTargetSettingsMenu.ToList();
                break;
            case WhatShouldBeOneSwitch.EnemyTurn:
                targetableRightNow.Clear();
                break;
        }

        oneSwitchActivePrevious = oneSwitchActiveNow;
    }
    

    private void CurrentTarget()
    {

        ChangeOneSwitch();

        if (targetableRightNow.Count <= 0) return;

        index++;

        for (; index < targetableRightNow.Count; index++)
        {
            if (targetableRightNow[index].TryGetComponent(out CardDisplay cardDisplay))
            {
                if (cardDisplay.card == null) continue;
                if (!cardDisplay.cardDisplayAttributes.cardPlayableEffect.activeSelf) continue;
            }

            break;
        }

        if (index >= targetableRightNow.Count)
            index = 0;

        if (index == 0 && !firstTime)
            ResetLast();
        
        HideTarget();
        ShowTarget();
        
        firstTime = false;       
    }

    private void UsedCard(CardDisplay cardDisplay)
    {
        if (playCardManager.CanCardBePlayed(cardDisplay))
        {
            if (playCardManager.TauntCard()) return;

            else if (!cardDisplay.card.targetable)
            {
                playCardManager.PlayCard(TypeOfCardTargeting.UnTargeted, null);
            }
            else
            {
                print("GoesThere");
                CancelInvoke();
                clicked = true;

                ListEnum lE = new ListEnum();
                lE.opponentLandmarks = true;
                lE.opponentChampions = true;

                choice.ChoiceMenu(lE, 1, WhichMethod.OneSwitchTarget, cardDisplay.card);
                oneSwitchActiveNow = WhatShouldBeOneSwitch.Choice;
                InvokeRepeating(nameof(CurrentTargetWithCard), 1f, 1f);
                return;
            }
        }
        
    }

    private void UsedCardWithTarget()
    {
        if (thingsToTargetWithCard[indexTargets].gameObject.TryGetComponent(out AvailableChampion availableChampion))
            playCardManager.PlayCard(TypeOfCardTargeting.Targeted, availableChampion.gameObject);
        
        else if (thingsToTargetWithCard[indexTargets].gameObject.TryGetComponent(out LandmarkDisplay landmark))
            playCardManager.PlayCard(TypeOfCardTargeting.Targeted, landmark.gameObject);
        
        clicked = false;
        CancelInvoke();
        InvokeRepeating(nameof(CurrentTarget), 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (!clicked)
            {
                if (targetableRightNow[index].TryGetComponent(out CardDisplay cardDisplay))
                    UsedCard(cardDisplay);

                if (targetableRightNow[index].gameObject.name.Equals("ConfirmButton"))
                {
                    index = -1;
                    prevIndex = 0;
                   // ChoiceAllternatives();
                }
                else if (targetableRightNow[index].gameObject.name.Equals("EndTurn"))
                    oneSwitchActiveNow = WhatShouldBeOneSwitch.EnemyTurn;
            }
            else
            {
                UsedCardWithTarget();
            }
        }
    }

    private void CurrentTargetWithCard()
    {
        if (oneSwitchActivePrevious != oneSwitchActiveNow)
            ChangeOneSwitch();

        indexTargets++;

        for (; indexTargets < thingsToTargetWithCard.Length; indexTargets++)
        {
            if (thingsToTargetWithCard[indexTargets].TryGetComponent(out LandmarkDisplay landmarkDisplay))
            {
                if (landmarkDisplay.card == null) continue;
            }
                
            break;
        }

        if (indexTargets >= thingsToTargetWithCard.Length)
            indexTargets = 0;

        if (indexTargets == 0 && !firstTime)
            ResetLast();

        HideTarget();
        ShowTarget();

    }

    private void ChoiceAllternatives()
    {
        if (!whatToCheck[0].activeSelf)
            oneSwitchActiveNow = WhatShouldBeOneSwitch.Normal;
        targetableRightNow = choice.buttonHolder.GetComponentsInChildren<Targetable>().ToList();
        targetableRightNow.Add(choice.confirmMenuButton.GetComponent<Targetable>());
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnEnable()
    {
        Invoke(nameof(DelayedEnable), 0.5f);
    }

    private void CheckWhatIsActive()
    {
        if (whatToCheck[0].activeInHierarchy)
            oneSwitchActiveNow = WhatShouldBeOneSwitch.Choice;
        else if (whatToCheck[1].activeInHierarchy)
            oneSwitchActiveNow = WhatShouldBeOneSwitch.Shop;
        else if (whatToCheck[2].activeInHierarchy)
            oneSwitchActiveNow = WhatShouldBeOneSwitch.EnemyTurn;
        else if (whatToCheck[3].activeInHierarchy)
            oneSwitchActiveNow = WhatShouldBeOneSwitch.Settings;
        else if (whatToCheck[4].activeInHierarchy)
            oneSwitchActiveNow = WhatShouldBeOneSwitch.OptionMenu;
        else
            oneSwitchActiveNow = WhatShouldBeOneSwitch.Normal;
    }

    private void DelayedEnable()
    {
        choice = Choice.Instance;
        //Choice
        CheckWhatIsActive();


        InvokeRepeating(nameof(CurrentTarget), 1f, 1f);
    }

    public enum WhatShouldBeOneSwitch
    {
        Choice,
        Normal,
        CardTarget,
        Shop,
        OptionMenu,
        RuleBook,
        Settings,
        EnemyTurn,
    }
}
