using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Choise : MonoBehaviour
{
    public ListEnum listEnum;

    public List<TargetInfo> chosenTargets = new List<TargetInfo>();
    public int amountOfTargets = 0;
    public GameObject choiceButtonPrefab;

    public TMP_Text descriptionText;
    public GameObject buttonHolder;

    private GameState gameState;

    public WhichMethod whichMethod;

    private static Choise instance;

    public List<GameObject> buttonsToDestroy = new List<GameObject>();

    public static Choise Instance { get { return instance; } set { instance = value; } }

    public void ShowChoiceMenu(ListEnum listEnum, int amountToTarget, WhichMethod theMethod)
    {
        gameObject.SetActive(true);
        whichMethod = theMethod;
        amountOfTargets = amountToTarget;
        if (listEnum.myChampions)
        {
            for (int i = 0; i < gameState.playerChampions.Count; i++)
            {
                AvailableChampion champ = gameState.playerChampions[i];
                if (champ == gameState.playerChampion) continue;

                GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);

                gO.GetComponent<Image>().sprite = champ.champion.artwork;

                gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, i);

                buttonsToDestroy.Add(gO);
            }
        }
    }

    public void AddTargetInfo(TargetInfo targetInfo)
    {
        chosenTargets.Add(targetInfo);

        if (chosenTargets.Count == amountOfTargets)
        {
            switch(whichMethod)
            {
                case WhichMethod.switchChampion:
                    SwitchChamp(false);                   
                    break;

                case WhichMethod.switchChampionDied:
                    SwitchChamp(true);                    
                    break;
            }
        }
        ResetChoice();
    }

    private void ResetChoice()
    {   
        amountOfTargets = 0;
        chosenTargets.Clear();
        foreach(GameObject obj in buttonsToDestroy)
        {
            Destroy(obj);
        }
        buttonsToDestroy.Clear();
        gameObject.SetActive(false);
    }

    private void SwitchChamp(bool died)
    {
        gameState.SwitchMyChampions(chosenTargets[0]);

        if (gameState.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(chosenTargets[0]);
            request.whichPlayer = ClientConnection.Instance.playerId;

            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameState = GameState.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum WhichMethod
{
    switchChampion, switchChampionDied
}
