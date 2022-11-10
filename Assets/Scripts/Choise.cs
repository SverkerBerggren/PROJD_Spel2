using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    private GameObject choiceMenu;
    private GameObject choiceOpponentMenu;

    public List<GameObject> buttonsToDestroy = new List<GameObject>();

    public static Choise Instance { get { return instance; } set { instance = value; } }

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
        choiceMenu = transform.GetChild(0).gameObject;
        choiceOpponentMenu = transform.GetChild(1).gameObject;
    }

    public void ShowOpponentThinking()
    {
        choiceOpponentMenu.SetActive(true);
    }
    public void HideOpponentThinking()
    {
        choiceOpponentMenu.SetActive(false);
    }

    private IEnumerator ShowChoiceMenu(ListEnum listEnum, int amountToTarget, WhichMethod theMethod, float delay)
    {
        yield return new WaitForSeconds(delay);
        //yield return new WaitUntil(() => gameState.hasPriority && gameState.isItMyTurn);

        choiceMenu.SetActive(true);

        

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

		if (listEnum.opponentChampions)
		{
			for (int i = 0; i < gameState.opponentChampions.Count; i++)
			{
				AvailableChampion champ = gameState.opponentChampions[i];
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
        gameState.SwitchMyChampions(chosenTargets[0]);

        if (gameState.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(chosenTargets[0]);
            request.whichPlayer = ClientConnection.Instance.playerId;
            request.championDied = died;

            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);

            if(died)
            {
                gameState.RemoveChampion(gameState.playerChampions[chosenTargets[0].index].champion);
                print("kommer till choise delen");
                RequestPassPriority requestPassPriority = null;
                if (gameState.playerChampion.name.Equals("Duelist"))
                    requestPassPriority = new RequestPassPriority(false);
                else
                    requestPassPriority = new RequestPassPriority(true);
                requestPassPriority.whichPlayer = ClientConnection.Instance.playerId;
                ClientConnection.Instance.AddRequest(requestPassPriority, gameState.RequestEmpty);
            }
        }
        print(gameState.hasPriority);
        if (chosenTargets[0].whichList.myChampions && gameState.hasPriority)
            gameState.playerChampion.champion.WhenCurrentChampion();

    }



    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChoiceMenu(ListEnum listEnum, int amountToTarget, WhichMethod theMethod)
    {
        IEnumerator enumerator = ShowChoiceMenu(listEnum, amountToTarget, theMethod, 0.01f);
        StartCoroutine(enumerator);
    }
}

public enum WhichMethod
{
    switchChampion, switchChampionDied
}
