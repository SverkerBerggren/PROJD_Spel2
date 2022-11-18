using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;

public class Choise : MonoBehaviour
{
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

    private bool isChoiceActive = false; 

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
    private void FixedUpdate()
    {
        if (!gameState.hasPriority && gameState.isItMyTurn)
                ShowOpponentThinking();
        else
                HideOpponentThinking();


        if (!gameState.hasPriority && isChoiceActive)
        {

            choiceMenu.SetActive(false);
        }
        else if(isChoiceActive)
        {
            choiceMenu.SetActive(true); 
        }
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
        isChoiceActive = true;


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
        gameState.Refresh();
    }

    public void ResetChoice()
    {
        isChoiceActive = false;
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
        if(died)
            gameState.RemoveChampion(gameState.playerChampions[chosenTargets[0].index].champion);

        if (gameState.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(chosenTargets[0]);
            request.whichPlayer = ClientConnection.Instance.playerId;
            request.championDied = died;

            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
        }

        PriorityForSwap();
        
        if (chosenTargets[0].whichList.myChampions)
            gameState.playerChampion.champion.WhenCurrentChampion();        
    }

    private void PriorityForSwap()
    {
        if (!gameState.isItMyTurn)
        {
            if (chosenTargets[0].whichList.myChampions && !gameState.playerChampion.champion.name.Equals("Duelist"))
            {
                print("Den passar priority via choice memyn");
                gameState.PassPriority();
            }
            if (gameState.hasPriority && chosenTargets[0].whichList.opponentChampions)
                gameState.PassPriority();
        }

        if (chosenTargets[0].whichList.opponentChampions && gameState.opponentChampion.champion.name.Equals("Duelist"))
        {
            gameState.PassPriority();
        }
        
    }

    private bool CheckIfChoice(WhichMethod theMethod, ListEnum list)
    {

        switch (theMethod)
        {
            case WhichMethod.switchChampion:
                if (list.myChampions == true && gameState.playerChampions.Count <= 1)
                {
                    return false;
                }
                else if (list.opponentChampions == true && gameState.opponentChampions.Count <= 1)
                {
                    return false;
                }
                break;
        }
        return true;
    }

    public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod)
    {
        //Måste lägga in om choicen failar checkifchoice att den ska passa priority om den ska göra det
        if (CheckIfChoice(theMethod, list))
        {
            IEnumerator enumerator = ShowChoiceMenu(list, amountToTarget, theMethod, 0.01f);
            StartCoroutine(enumerator);
        }
    }
}

public enum WhichMethod
{
    switchChampion, switchChampionDied
}
