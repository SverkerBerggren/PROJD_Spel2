using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class NewOneSwitch : MonoBehaviour
{
    private PlayCardManager playCardManager;
    private Choice choice;
    private bool clicked = false;

    public GameObject ShowSelected;
    [Header("Diffrent types of targets")]
    [SerializeField] private Targetable[] thingsToTargetInNormalSituation;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    [SerializeField] private Targetable[] thingsToTargetShop;
    [SerializeField] private Targetable[] thingsToTargetSettingsMenu;
    [SerializeField] private Targetable[] thingsToTargetOptionsMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] whatToCheck;

    

    // Start is called before the first frame update
    void Start()
    {
        playCardManager = PlayCardManager.Instance;
        choice = Choice.Instance;
    }

    private void OnEnable()
    {
        StartCoroutine(LoopStart());
    }

    IEnumerator LoopStart()
    {
        int i = 0;
        while (i < thingsToTargetInNormalSituation.Length)
        {
            print("The first part" + i);
            if (thingsToTargetInNormalSituation[i].TryGetComponent(out CardDisplay cardDisplay))
            {
                if (cardDisplay.Card == null)
                {
                    i++;
                    continue;
                }

                print("Card: " + cardDisplay.Card.CardName);
            }
            else
            {
                print("Other: " + thingsToTargetInNormalSituation[i].name);
            }
            

            yield return new WaitForSeconds(3);
            print("The second part" + i);
            i++;

            if (i == thingsToTargetInNormalSituation.Length)
                i = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clicked = true;
        }
    }
}
