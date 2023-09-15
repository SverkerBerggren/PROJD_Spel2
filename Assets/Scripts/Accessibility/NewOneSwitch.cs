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
    private bool clicked = false;
    private int i = 0;

    public bool options = false;
    public bool settings = false;
    public bool shop = false;
    public bool targetWithCard = false;


    public GameObject ShowSelected;
    [Header("Diffrent types of targets")]
    [SerializeField] private Targetable[] thingsToTargetInNormalSituation;
    [SerializeField] private Targetable[] thingsToTargetWithCard;
    [SerializeField] private Targetable[] thingsToTargetShop;
    [SerializeField] private Targetable[] thingsToTargetSettingsMenu;
    [SerializeField] private Targetable[] thingsToTargetOptionsMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] whatToCheck;

    [SerializeField] private float delay = 3f;

    private static NewOneSwitch instance;
    public static NewOneSwitch Instance { get { return instance; } set { instance = value; } }

    // Start is called before the first frame update
    void Start()
    {
        playCardManager = PlayCardManager.Instance;
        choice = Choice.Instance;
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
        StartCoroutine(LoopStart());
    }

    

    IEnumerator LoopStart()
    {   
        while (i < thingsToTargetInNormalSituation.Length)
        {
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
            i++;

            if (i == thingsToTargetInNormalSituation.Length)
                i = 0;
        }

        
    }

    IEnumerator ScaleSelected(GameObject gameObjectToChangeBack)
    {
        gameObjectToChangeBack.transform.localScale = Vector3.Scale(gameObjectToChangeBack.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f));
        yield return new WaitForSeconds(delay);       
        gameObjectToChangeBack.transform.localScale = Vector3.Scale(gameObjectToChangeBack.transform.localScale, new Vector3(2/3f, 2/3f, 2/3f));
    }

    public void resetBools()
    {
        options = false;
        settings = false;
        shop = false;
        targetWithCard = false;
    }

    // Update is called once per frame
    void Update()
    {
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
                print("CLicked for target with card Situation");
            }
            else // Normal
            {
                if (thingsToTargetInNormalSituation[i].TryGetComponent(out CardDisplay cardDisplay))
                {
                    //Play Card
                }
                else
                {
                    thingsToTargetInNormalSituation[i].GetComponent<Button>().onClick.Invoke();
                    print("CLicked for Normal Situation");
                }
                
            }

            i = 0;
        }
    }

}


