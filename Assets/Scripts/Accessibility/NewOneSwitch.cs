using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class NewOneSwitch : MonoBehaviour
{
    private PlayCardManager playCardManager;
    private Choice choice;
    private GameState gameState;
    private bool canClick = false;
    private int i = 0;
    private float timer = 0;

    public float delay;
    [NonSerialized] public bool initialClick = false;

    [NonSerialized] public bool options = false;
    [NonSerialized] public bool settings = false;
    [NonSerialized] public bool shop = false;
    [NonSerialized] public bool targetWithCard = false;
    [NonSerialized] public bool choiceMenuActive = false;
    [NonSerialized] public bool tutorialMenu = false;
    [NonSerialized] public bool tutorialMenuIsOpen = false;
    [NonSerialized] public bool outerLoop = true;

    [NonSerialized] public List<float> speeds = new List<float>();


    [SerializeField] private float scaleOfSelectedObject;

    [SerializeField] private Transform contentChoiceMenu;
    [SerializeField] private GameObject optionsMenuOpen;
    [SerializeField] private GameObject settingsMenuOpen;
    [SerializeField] private GameObject shopMenuOpen;
    [SerializeField] private GameObject tutortialMenuOpen;
    [SerializeField] private GameObject tutortialStepsOpen;

    public Button tutorialCloseButton;

    public GameObject ShowSelected;
    [Header("Diffrent types of targets")]
    [SerializeField] private Targetable[] thingsToTargetInNormalSituationInnerLoop;
    [SerializeField] private Targetable[] thingsToTargetInNormalSituationOuterLoop;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    [SerializeField] private Targetable[] thingsToTargetShop;
    [SerializeField] private Targetable[] thingsToTargetSettingsMenu;
    [SerializeField] private Targetable[] thingsToTargetOptionsMenu;
    [SerializeField] private Targetable[] thingsToTargetTutorialMenu;

    [SerializeField] private List<GameObject> thingsToTargetWithChoiceMenu;

    [SerializeField] private List<GameObject> attackIndicators;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] whatToCheck;

    private Coroutine loopStart;
    private CardDisplay cardToUse;

    private TextAsset textFile;

    private static NewOneSwitch instance;
    public static NewOneSwitch Instance { get { return instance; } set { instance = value; } }

    // Start is called before the first frame update
    void Start()
    {
        playCardManager = PlayCardManager.Instance;
        choice = Choice.Instance;
        gameState = GameState.Instance;

        ReadFile();
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

    private void ReadFile()
    {
        StreamReader reader = new StreamReader("Assets/OneSwitchSpeedController/SpeedController.txt");
        string fileContent = reader.ReadToEnd();
        string[] splitFileContent = fileContent.Split(char.Parse("\n"));
        foreach (string line in splitFileContent)
        {        
            string numberIsolation = line.Split(':')[1];

            float number = float.Parse(numberIsolation);

            speeds.Add(number);
            Debug.Log(number);
        }
        delay = speeds[1];
        reader.Close();
    }

    private void OnEnable()
    {
        Invoke("StartLoopWithDelay", 0.1f);
    }

    private void OnDisable()
    {
        ResetBools();
        StopCoroutine(loopStart);
    }

    private void StartLoopWithDelay()
    {
        loopStart = StartCoroutine(LoopStart());
        canClick = true;
    }

    IEnumerator LoopStart()
    {   
        while (i < thingsToTargetInNormalSituationInnerLoop.Length)
        {
            while (tutorialMenuIsOpen)
            {
                StartCoroutine(ScaleSelected(tutorialCloseButton.gameObject, thingsToTargetInNormalSituationInnerLoop[i].particleSystem));
                yield return new WaitForSeconds(delay);
                canClick = true;
            }

            while (tutorialMenu)
            {
                StartCoroutine(ScaleSelected(thingsToTargetTutorialMenu[i].gameObject, thingsToTargetTutorialMenu[i].particleSystem));

                yield return new WaitForSeconds(delay);
                canClick = true;
                i++;
                if (i == thingsToTargetWithChoiceMenu.Count)
                    i = 0;
            }


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
                
                StartCoroutine(ScaleSelected(thingsToTargetWithChoiceMenu[i], null));

                yield return new WaitForSeconds(delay);
                canClick = true;
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
                attackIndicators[i].SetActive(true);
                StartCoroutine(ScaleSelected(thingsToTargetWithCard[i].gameObject, thingsToTargetWithCard[i].particleSystem));
                yield return new WaitForSeconds(delay);
                attackIndicators[i].SetActive(false);
                canClick = true;
                i++;
                if (i == thingsToTargetWithCard.Length)
                    i = 0;
            }

            while (settings) // Accessiblity Screen
            {
                StartCoroutine(ScaleSelected(thingsToTargetSettingsMenu[i].gameObject, thingsToTargetSettingsMenu[i].particleSystem));

                yield return new WaitForSeconds(delay);
                canClick = true;
                i++;
                if (i == thingsToTargetSettingsMenu.Length)
                    i = 0;
            }

            while (options)
            {
                StartCoroutine(ScaleSelected(thingsToTargetOptionsMenu[i].gameObject, thingsToTargetOptionsMenu[i].particleSystem));
               
                yield return new WaitForSeconds(delay);
                canClick = true;
                i++;
                if (i == thingsToTargetOptionsMenu.Length)
                    i = 0;
            }

            while (shop) // Accessiblity Screen
            {
                StartCoroutine(ScaleSelected(thingsToTargetShop[i].gameObject, thingsToTargetShop[i].particleSystem));

                yield return new WaitForSeconds(delay);
                canClick = true;
                i++;
                if (i == thingsToTargetShop.Length)
                    i = 0;
            }

            while (outerLoop)
            {
                StartCoroutine(ScaleSelected(thingsToTargetInNormalSituationOuterLoop[i].gameObject, thingsToTargetInNormalSituationOuterLoop[i].particleSystem));

                yield return new WaitForSeconds(delay);
                canClick = true;
                i++;
                if (i == thingsToTargetInNormalSituationOuterLoop.Length)
                    i = 0;
            }

            // If Card
            if (thingsToTargetInNormalSituationInnerLoop[i].TryGetComponent(out CardDisplay cardDisplay))
            {
                //IF there is no card
                if (cardDisplay.Card == null || !cardDisplay.GetComponentInChildren<CardDisplayAttributes>().cardPlayableEffect.activeSelf)
                {
                    i++;
                    continue;
                }
                cardDisplay.MouseEnter();

                cardDisplay.MouseExitOnDelay(delay);

            }
            // Else other
            else
            {
                StartCoroutine(ScaleSelected(thingsToTargetInNormalSituationInnerLoop[i].gameObject, thingsToTargetInNormalSituationInnerLoop[i].particleSystem));
            }




            yield return new WaitForSeconds(delay);
            canClick = true;
            i++;

            if (i == thingsToTargetInNormalSituationInnerLoop.Length)
                i = 0;
        }

        
    }

    IEnumerator ScaleSelected(GameObject gameObjectToChangeBack, GameObject particleSystem)
    {
        gameObjectToChangeBack.transform.localScale = Vector3.Scale(gameObjectToChangeBack.transform.localScale, new Vector3(scaleOfSelectedObject, scaleOfSelectedObject, scaleOfSelectedObject));
        if (particleSystem != null)
            particleSystem.SetActive(true);

        yield return new WaitForSeconds(delay);

        if (gameObjectToChangeBack != null)
        {
            float scaleDownAmount = 1 / scaleOfSelectedObject;
            gameObjectToChangeBack.transform.localScale = Vector3.Scale(gameObjectToChangeBack.transform.localScale, new Vector3(scaleDownAmount, scaleDownAmount, scaleDownAmount));

            if (particleSystem != null)
                particleSystem.SetActive(false);
        }
    }

    public void SwitchLoop()
    {
        if (outerLoop)
        {
            outerLoop = false;
        }
        else
        {
            outerLoop = true;
        }
    }

    public void ResetBools()
    {
        i = 0;
        options = false;
        settings = false;
        shop = false;
        targetWithCard = false;
        choiceMenuActive = false;
        tutorialMenu = false;
        tutorialMenuIsOpen = false;
        outerLoop = false;
        thingsToTargetWithChoiceMenu.Clear();
}

    // Update is called once per frame
    void Update()
    {

        if ((!choiceMenuActive) && contentChoiceMenu.childCount > 0)      
            choiceMenuActive = true;
        else if ((!settings) && settingsMenuOpen.activeSelf)
            settings = true;
        else if ((!options) && optionsMenuOpen.activeSelf)
            options = true;
        else if ((!shop) && shopMenuOpen.activeSelf)
            shop = true;
        else if (tutortialMenuOpen.activeSelf)
        {
            foreach (Transform child in tutortialStepsOpen.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    tutorialCloseButton = child.GetChild(1).GetComponent<Button>();
                    tutorialMenuIsOpen = true;
                    break;
                }
            }
            tutorialMenu = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && canClick && initialClick)
        {
            canClick = false;

            if (settings) // Accessibility Screenen
                thingsToTargetSettingsMenu[i].GetComponent<Button>().onClick.Invoke();
            
            else if(options)
                thingsToTargetOptionsMenu[i].GetComponent<Button>().onClick.Invoke();

            else if (shop)
            {
                thingsToTargetShop[i].GetComponent<Button>().onClick.Invoke();
                ResetBools();
            }
            else if (targetWithCard)
            {               
                playCardManager.PlayCard(TypeOfCardTargeting.Targeted, thingsToTargetWithCard[i].gameObject);
                ResetBools();
            }
            else if (choiceMenuActive)
                thingsToTargetWithChoiceMenu[i].GetComponent<Button>().onClick.Invoke();

            else if (tutorialMenuIsOpen)
                tutorialCloseButton.onClick.Invoke();

            else if (tutorialMenu)
            {
                thingsToTargetTutorialMenu[i].GetComponent<Button>().onClick.Invoke();
                                     
                if (i == 7)
                    ResetBools();
                else
                {
                    canClick = true;
                    tutorialCloseButton = thingsToTargetTutorialMenu[i].GetComponent<OpenTutorialMenu>().tutorialPanel.transform.GetChild(1).GetComponent<Button>();
                    tutorialMenuIsOpen = true;
                    StopCoroutine(loopStart);
                    return;
                }

            }
            else if (outerLoop)
            {
                thingsToTargetInNormalSituationOuterLoop[i].GetComponent<Button>().onClick.Invoke();
            }
            else // Normal
            {
                if (thingsToTargetInNormalSituationInnerLoop[i].TryGetComponent(out CardDisplay cardDisplay)) //Chose which Card to use
                {
                    playCardManager.card = cardDisplay.Card;
                    playCardManager.cardDisplay = cardDisplay;
                    ActionOfPlayer.Instance.CheckIfCanPlayCard(cardDisplay, true);
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
                    thingsToTargetInNormalSituationInnerLoop[i].GetComponent<Button>().onClick.Invoke();
                }
                
            }
            i = 0;
            StopCoroutine(loopStart);
            Invoke("StartLoopWithDelay", 0.1f);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !initialClick)
        {
            initialClick = true;
        }
    }

}


