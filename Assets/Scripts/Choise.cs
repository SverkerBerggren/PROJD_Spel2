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
        transform.GetChild(0).gameObject.SetActive(true);
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
              //  gameState.hasPriority = !gameState.hasPriority;
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

    public void ResetChoice()
    {   
        amountOfTargets = 0;
        chosenTargets.Clear();
        foreach(GameObject obj in buttonsToDestroy)
        {
            Destroy(obj);
        }
        buttonsToDestroy.Clear();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void SwitchChamp(bool died)
    {
        Champion champion = null;
        print(chosenTargets[0].index);
        if (chosenTargets[0].whichList.myChampions)
        {
            champion = gameState.playerChampions[chosenTargets[0].index].champion;
        }
        else
        {
            champion = gameState.opponentChampions[chosenTargets[0].index].champion;
        }

        champion.WhenInactiveChampion();
        gameState.SwitchMyChampions(chosenTargets[0]);
        champion.WhenCurrentChampion();

        if (gameState.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(chosenTargets[0]);
            request.whichPlayer = ClientConnection.Instance.playerId;
            request.championDied = died;

            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);

            if(died)
            {
                print("kommer till choise delen");
                RequestPassPriority requestPassPriority = new RequestPassPriority();
                requestPassPriority.whichPlayer = ClientConnection.Instance.playerId;
                ClientConnection.Instance.AddRequest(requestPassPriority, gameState.RequestEmpty);
            }
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
