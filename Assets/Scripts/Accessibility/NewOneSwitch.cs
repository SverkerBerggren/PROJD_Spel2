using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class NewOneSwitch : MonoBehaviour
{
    private PlayCardManager playCardManager;
    private Choice choice;
    private GameState gameState;
    private bool clicked = false;
    private int i = 0;

    public bool options = false;
    public bool settings = false;
    public bool shop = false;
    public bool targetWithCard = false;
    public bool choiceMenuActive = false;
    [SerializeField] private Transform contentChoiceMenu;

    public GameObject ShowSelected;
    [Header("Diffrent types of targets")]
    [SerializeField] private Targetable[] thingsToTargetInNormalSituation;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    [SerializeField] private Targetable[] thingsToTargetShop;
    [SerializeField] private Targetable[] thingsToTargetSettingsMenu;
    [SerializeField] private Targetable[] thingsToTargetOptionsMenu;

    [SerializeField] private List<GameObject> thingsToTargetWithChoiceMenu;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] whatToCheck;

    [SerializeField] private float delay = 3f;
    private Coroutine loopStart;
    private CardDisplay cardToUse;

    private static NewOneSwitch instance;
    public static NewOneSwitch Instance { get { return instance; } set { instance = value; } }

    // Start is called before the first frame update
    void Start()
    {
        playCardManager = PlayCardManager.Instance;
        choice = Choice.Instance;
        gameState = GameState.Instance;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        loopStart = StartCoroutine(LoopStart());
    }

    

    IEnumerator LoopStart()
    {   
        while (i < thingsToTargetInNormalSituation.Length)
        {
            while (choiceMenuActive)
            {
                if (thingsToTargetWithChoiceMenu.Count == 0)
                {
                    for (int j = 0; j < contentChoiceMenu.childCount; j++)
                    {
                        thingsToTargetWithChoiceMenu.Add(contentChoiceMenu.GetChild(j).gameObject);
                    }
                    if (contentChoiceMenu.parent.GetChild(3).gameObject.activeSelf)
                    {
                        thingsToTargetWithChoiceMenu.Add(contentChoiceMenu.parent.GetChild(3).gameObject);
                    }

                }
                
                StartCoroutine(ScaleSelected(thingsToTargetWithChoiceMenu[i]));

                yield return new WaitForSeconds(3);
                i++;
                if (i == thingsToTargetWithChoiceMenu.Count)
                    i = 0;
            }

            while (targetWithCard)
            {
                if (thingsToTargetWithCard[i].TryGetComponent(out LandmarkDisplay landmarkDisplay))
                {
                    if (landmarkDisplay.Card == null)
                    {
                        i++;
                        if (i == thingsToTargetWithCard.Length)
                            i = 0;
                        continue;
                    }
                }
                StartCoroutine(ScaleSelected(thingsToTargetWithCard[i].gameObject));
                print(i);
                yield return new WaitForSeconds(3);
                i++;
                if (i == thingsToTargetWithCard.Length)
                    i = 0;
            }

            while (options)
            {
                StartCoroutine(ScaleSelected(thingsToTargetOptionsMenu[i].gameObject));
               
                yield return new WaitForSeconds(3);
                i++;
                if (i == thingsToTargetOptionsMenu.Length)
                    i = 0;
            }

            while (settings) // Accessiblity Screen
            {
                StartCoroutine(ScaleSelected(thingsToTargetSettingsMenu[i].gameObject));

                yield return new WaitForSeconds(3);
                i++;
                if (i == thingsToTargetSettingsMenu.Length)
                    i = 0;
            }
            while (shop) // Accessiblity Screen
            {
                StartCoroutine(ScaleSelected(thingsToTargetShop[i].gameObject));

                yield return new WaitForSeconds(3);
                i++;
                if (i == thingsToTargetShop.Length)
                    i = 0;
            }

            // If Card
            if (thingsToTargetInNormalSituation[i].TryGetComponent(out CardDisplay cardDisplay))
            {
                //IF there is no card
                if (cardDisplay.Card == null || !cardDisplay.GetComponentInChildren<CardDisplayAttributes>().cardPlayableEffect.activeSelf)
                {
                    i++;
                    continue;
                }
                cardDisplay.MouseEnter();

                cardDisplay.MouseExitOnDelay(3f);

            }
            // Else other
            else
            {
                StartCoroutine(ScaleSelected(thingsToTargetInNormalSituation[i].gameObject));
            }


            yield return new WaitForSeconds(3);
            print(i);
            i++;

            if (i == thingsToTargetInNormalSituation.Length)
                i = 0;
        }

        
    }

    IEnumerator ScaleSelected(GameObject gameObjectToChangeBack)
    {
        gameObjectToChangeBack.transform.localScale = Vector3.Scale(gameObjectToChangeBack.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f));
        yield return new WaitForSeconds(delay);
        if (gameObjectToChangeBack != null)
            gameObjectToChangeBack.transform.localScale = Vector3.Scale(gameObjectToChangeBack.transform.localScale, new Vector3(2/3f, 2/3f, 2/3f));
    }

    public void ResetBools()
    {
        i = 0;
        options = false;
        settings = false;
        shop = false;
        targetWithCard = false;
        choiceMenuActive = false;
        thingsToTargetWithChoiceMenu.Clear();
}

    // Update is called once per frame
    void Update()
    {

        if ((!choiceMenuActive) && contentChoiceMenu.childCount > 0 ) 
        {
            choiceMenuActive = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            clicked = true;
            
            if (options)
            {
                thingsToTargetOptionsMenu[i].GetComponent<Button>().onClick.Invoke();
                print("CLicked for Options Situation");
            }
            else if (settings) // Accessibility Screenen
            {
                thingsToTargetSettingsMenu[i].GetComponent<Button>().onClick.Invoke();
                print("CLicked for Settings Situation");
            }
            else if (shop)
            {
                thingsToTargetShop[i].GetComponent<Button>().onClick.Invoke();
                print("CLicked for Shop Situation");
            }
            else if (targetWithCard)
            {               
                playCardManager.PlayCard(TypeOfCardTargeting.Targeted, thingsToTargetWithCard[i].gameObject);
                print("CLicked for target with card Situation");
                ResetBools();
            }
            else if (choiceMenuActive)
            {
                thingsToTargetWithChoiceMenu[i].GetComponent<Button>().onClick.Invoke();
            }
            else // Normal
            {
                if (thingsToTargetInNormalSituation[i].TryGetComponent(out CardDisplay cardDisplay)) //Chose which Card to use
                {
                    playCardManager.card = cardDisplay.Card;
                    playCardManager.cardDisplay = cardDisplay;
                    if (playCardManager.TauntCard()) // IF There is a landmark with taunt
                        ResetBools();
                    if (!cardDisplay.Card.Targetable) // if you don't need a target use the card
                    {
                        playCardManager.PlayCard(TypeOfCardTargeting.UnTargeted, null);
                        ResetBools();
                    }
                    else
                    {
                        targetWithCard = true;
                        cardToUse = cardDisplay;                
                    }

                }
                else
                {
                    thingsToTargetInNormalSituation[i].GetComponent<Button>().onClick.Invoke();
                    print("CLicked for Normal Situation");
                }
                
            }
            i = 0;
            StopCoroutine(loopStart);
            loopStart = StartCoroutine(LoopStart());
        }
    }

}


