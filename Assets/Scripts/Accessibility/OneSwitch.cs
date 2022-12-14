using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OneSwitch : MonoBehaviour
{
    private int index = -1;
    private int prevIndex = 0;
    private bool clicked = false;
    private bool firstTime = true;
    private Color normalSelectColor;
    private bool changedList = false;

    private WhatShouldBeOneSwitch oneSwitchActivePrevious;
    private PlayCardManager playCardManager;
    private Choice choice;

    [Header("TargetsNow")]
    [SerializeField] private List<Targetable> targetableRightNow = new List<Targetable>();

    [Header("Diffrent types of targets")]
    [SerializeField] private Targetable[] thingsToTargetInNormalSituation;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    [SerializeField] private Targetable[] thingsToTargetShop;
    [SerializeField] private Targetable[] thingsToTargetSettingsMenu;
    [SerializeField] private Targetable[] thingsToTargetOptionsMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] whatToCheck;

    [SerializeField] private Color buttonSelectColor;

    

    public WhatShouldBeOneSwitch OneSwitchActiveNow;

    private void Start()
    {
        playCardManager = PlayCardManager.Instance;
        choice = Choice.Instance;
    }

    // Hides the previous target
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
        if (targetableRightNow[prevIndex].TryGetComponent(out OneSwitchSettingSelect settingsSelectHoverprev))
            settingsSelectHoverprev.OneSwitchHover();
        eventSystem.SetSelectedGameObject(null);
        prevIndex = index;
    }

    //Shows the target
    private void ShowTarget()
    {
        if (targetableRightNow[index].TryGetComponent(out CardDisplay cardDisplayShow))
            cardDisplayShow.MouseEnter();
        if (targetableRightNow[index].TryGetComponent(out ChoiceButton choiceButton))
            choiceButton.OneSwitchHoverChoice();
        if (targetableRightNow[index].TryGetComponent(out PurchaseCard purchaseCardPrev))
            purchaseCardPrev.OneSwitchHover();
        if (targetableRightNow[index].TryGetComponent(out ReturnButton returnButtonPrev))
            returnButtonPrev.OneSwitchButtonSelect();
        if (targetableRightNow[index].TryGetComponent(out ShopHover shopHoverPrev))
            shopHoverPrev.OneSwitchSelected();
        if (targetableRightNow[index].TryGetComponent(out OneSwitchSettingSelect settingsSelectHoverprev))
            settingsSelectHoverprev.OneSwitchHover();
        if (targetableRightNow[index].TryGetComponent(out Button confirmButton))
        {
            ColorBlock colors = confirmButton.colors;
            normalSelectColor = colors.selectedColor;
            colors.selectedColor = buttonSelectColor;

            confirmButton.colors = colors;

            confirmButton.Select();
        }
    }

    // Resets the last index when going starting over
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
        if (targetableRightNow[length].TryGetComponent(out OneSwitchSettingSelect settingsSelectHoverLastReset))
            settingsSelectHoverLastReset.OneSwitchHover();
        eventSystem.SetSelectedGameObject(null);
    }

    // Changes what it should target
    private void ChangeOneSwitch()
    {
        CheckWhatIsActive();
        changedList = false;

        switch (OneSwitchActiveNow)
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

        if (oneSwitchActivePrevious != OneSwitchActiveNow)
        {
            changedList = true;
            oneSwitchActivePrevious = OneSwitchActiveNow;
            index = -1;
            prevIndex = 0;
        }
    }
    
    // The loop that checks which target to show
    private void CurrentTarget()
    {
/*        if (OneSwitchActiveNow != oneSwitchActivePrevious)
        {
            index = -1;
            prevIndex = 0;
        }*/
        ChangeOneSwitch();

        if (targetableRightNow.Count <= 0) return;

        index++;
        if (index >= targetableRightNow.Count)
            index = 0;

        for (; index < targetableRightNow.Count; index++)
        {
            if (targetableRightNow[index].TryGetComponent(out CardDisplay cardDisplay))
            {
                if (cardDisplay.Card == null) continue;
                if (!cardDisplay.cardDisplayAttributes.cardPlayableEffect.activeSelf) continue;
            }

            break;
        }

        if (index == 0 && !firstTime)
            ResetLast();
        if (prevIndex < targetableRightNow.Count && !changedList)
            HideTarget();
        ShowTarget();
        
        firstTime = false;       
    }

    // if you play a card
    private void UsedCard(CardDisplay cardDisplay)
    {
        if (playCardManager.CanCardBePlayed(cardDisplay))
        {
            if (playCardManager.TauntCard()) return;

            else if (!cardDisplay.Card.Targetable)          
                playCardManager.PlayCard(TypeOfCardTargeting.UnTargeted, null);
            
            else
            {
                clicked = true;

                ListEnum lE = new ListEnum();
                lE.opponentLandmarks = true;
                lE.opponentChampions = true;

                choice.ChoiceMenu(lE, 1, WhichMethod.OneSwitchTarget, cardDisplay.Card);
                OneSwitchActiveNow = WhatShouldBeOneSwitch.Choice;
                return;
            }
        }
        
    }

    // If you try to target something with a card
    private void UsedCardWithTarget()
    {
        if (thingsToTargetWithCard[index].gameObject.TryGetComponent(out AvailableChampion availableChampion))
            playCardManager.PlayCard(TypeOfCardTargeting.Targeted, availableChampion.gameObject);
        
        else if (thingsToTargetWithCard[index].gameObject.TryGetComponent(out LandmarkDisplay landmark))
            playCardManager.PlayCard(TypeOfCardTargeting.Targeted, landmark.gameObject);
        
        clicked = false;
    }

    // Another loop that checks when you try to target something with a card
/*    private void CurrentTargetWithCard()
    {

        ChangeOneSwitch();

        indexTargets++;

        if (indexTargets >= thingsToTargetWithCard.Length)
            indexTargets = 0;

        if (indexTargets == 0 && !firstTime)
            ResetLast();

        if (prevIndex < targetableRightNow.Count && !changedList)
            HideTarget();
        ShowTarget();

    }*/

    private void ChoiceAllternatives()
    {
        if (!whatToCheck[0].activeSelf)
            OneSwitchActiveNow = WhatShouldBeOneSwitch.Normal;
        targetableRightNow = choice.ButtonHolder.GetComponentsInChildren<Targetable>().ToList();
        if (choice.ConfirmMenuButton.activeSelf)
            targetableRightNow.Add(choice.ConfirmMenuButton.GetComponent<Targetable>());
        else if (choice.closeMenuButton.activeSelf)
            targetableRightNow.Add(choice.closeMenuButton.GetComponent<Targetable>());
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
            OneSwitchActiveNow = WhatShouldBeOneSwitch.Choice;
        else if (whatToCheck[1].activeInHierarchy)
            OneSwitchActiveNow = WhatShouldBeOneSwitch.Shop;
        else if (whatToCheck[2].activeInHierarchy)
            OneSwitchActiveNow = WhatShouldBeOneSwitch.EnemyTurn;
        else if (whatToCheck[3].activeInHierarchy)
            OneSwitchActiveNow = WhatShouldBeOneSwitch.Settings;
        else if (whatToCheck[4].activeInHierarchy)
            OneSwitchActiveNow = WhatShouldBeOneSwitch.OptionMenu;
        else
            OneSwitchActiveNow = WhatShouldBeOneSwitch.Normal;
    }
    private void DelayedEnable()
    {
        choice = Choice.Instance;
        //Choice
        CheckWhatIsActive();


        InvokeRepeating(nameof(CurrentTarget), 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventSystem.SetSelectedGameObject(null);
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
                    OneSwitchActiveNow = WhatShouldBeOneSwitch.EnemyTurn;
            }
            else
            {
                UsedCardWithTarget();
            }
        }
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
