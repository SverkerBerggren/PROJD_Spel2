using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneSwitch : MonoBehaviour
{
    [SerializeField] private Targetable[] allThingsTargetable;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    private int index = 0;
    private int indexTargets = 0; 
    private bool clicked = false;


    private void CurrentTarget()
    {
        print("What to target: " + allThingsTargetable[index].gameObject);
        index++;

        if (index >= allThingsTargetable.Length)
            index = 0;

        for (; index < allThingsTargetable.Length; index++)
        {
            CardDisplay cardDisplay = null;
            if (allThingsTargetable[index].TryGetComponent(out cardDisplay))
            {
                if (cardDisplay.card == null)
                {
                    if (index >= allThingsTargetable.Length)
                        index = -1;

                    continue;
                }
            }

            break;
        }
    }

    private void UsedCard(CardDisplay cardDisplay)
    {
        if (PlayCardManager.Instance.CanCardBePlayed(cardDisplay))
        {
            if (PlayCardManager.Instance.TauntCard()) return;

            else if (!cardDisplay.card.targetable)
            {
                PlayCardManager.Instance.PlayCard(TypeOfCardTargeting.UnTargeted, null);
            }
            else
            {
                print("GoesThere");
                CancelInvoke();
                clicked = true;
                InvokeRepeating(nameof(CurrentTargetWithCard), 1f, 1f);
                return;
            }
        }
        
    }

    private void UsedCardWithTarget()
    {
        AvailableChampion availableChampion = null;
        LandmarkDisplay landmark = null;
        if (thingsToTargetWithCard[indexTargets].gameObject.TryGetComponent(out availableChampion))
        {
            PlayCardManager.Instance.PlayCard(TypeOfCardTargeting.Targeted, availableChampion.gameObject);
        }
        else if (thingsToTargetWithCard[indexTargets].gameObject.TryGetComponent(out landmark))
        {
            PlayCardManager.Instance.PlayCard(TypeOfCardTargeting.Targeted, landmark.gameObject);
        }
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
                CardDisplay cardDisplay = null;
                if (allThingsTargetable[index].gameObject.TryGetComponent(out cardDisplay))
                {
                    print(cardDisplay.card);

                    UsedCard(cardDisplay);
                }
            }
            else
            {
                UsedCardWithTarget();
            }
        }

    }

    private void CurrentTargetWithCard()
    {
        print("What to target with card: " + thingsToTargetWithCard[indexTargets].gameObject);
        indexTargets++;

        if (indexTargets >= thingsToTargetWithCard.Length)
            indexTargets = 0;

        for (; indexTargets < thingsToTargetWithCard.Length; indexTargets++)
        {
            LandmarkDisplay landmarkDisplay = null;
            if (thingsToTargetWithCard[indexTargets].TryGetComponent(out landmarkDisplay))
            {
                if (landmarkDisplay.card == null)
                {
                    if (indexTargets >= thingsToTargetWithCard.Length)
                        indexTargets = -1;

                    continue;
                }
            }
                
            break;
        }

    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(CurrentTarget), 1f, 1f);
    }
}
