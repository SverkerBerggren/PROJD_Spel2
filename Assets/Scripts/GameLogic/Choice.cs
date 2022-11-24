using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Choice : MonoBehaviour
{
    public List<TargetInfo> chosenTargets = new List<TargetInfo>();
    private int amountOfTargets = 0;
    public GameObject choiceButtonPrefab;

    public TMP_Text descriptionText;
    [SerializeField] private GameObject closeMenuButton;
    public GameObject buttonHolder;

    private GameState gameState;
    private ActionOfPlayer actionOfPlayer;
    private Graveyard graveyard;

    private WhichMethod whichMethod;

    private static Choice instance;

    private GameObject choiceMenu;
    private GameObject choiceOpponentMenu;

    private List<GameObject> buttonsToDestroy = new List<GameObject>();

    private bool isChoiceActive = false;

    private CardDisplayAtributes cardDisplayAtributes;

    public static Choice Instance { get { return instance; } set { instance = value; } }

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

    }
    private void Start()
    {
        gameState = GameState.Instance;
        actionOfPlayer = ActionOfPlayer.Instance;
        graveyard = Graveyard.Instance;
        

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

                MakeButtonsChampions(champ.champion.artwork, listEnum, i);
            }
        }

		if (listEnum.opponentChampions)
		{
			for (int i = 0; i < gameState.opponentChampions.Count; i++)
			{
				Sprite champSprite = gameState.opponentChampions[i].champion.artwork;

                MakeButtonsChampions(champSprite, listEnum, i);
            }
		}

        if(listEnum.myHand)
        {
            print("kommer den till discard delen");
            descriptionText.text = "Choose a card to discard";
            for (int i = 0; i < actionOfPlayer.handPlayer.cardsInHand.Count; i++)
            {
                CardDisplay cardDisplay = actionOfPlayer.handPlayer.cardsInHand[i].GetComponent<CardDisplay>();

                MakeButtonsCards(cardDisplay.card, listEnum, i);
            }
        }

        if (listEnum.myGraveyard)
        {
            descriptionText.text = "Show graveyard";
            for (int i = 0; i < graveyard.graveyardPlayer.Count; i++)
            {
                MakeButtonsCards(graveyard.graveyardPlayer[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }

        if (listEnum.myDeck)
        {
            descriptionText.text = "Show Deck";
            for (int i = 0; i < actionOfPlayer.handPlayer.deck.deckPlayer.Count; i++)
            {
                MakeButtonsCards(actionOfPlayer.handPlayer.deck.deckPlayer[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }

        if (listEnum.myLandmarks)
        {
            descriptionText.text = "Show landmarks";
            for (int i = 0; i < gameState.playerLandmarks.Count; i++)
            {
                MakeButtonsCards(gameState.playerLandmarks[i].card, listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }
    }

    private void MakeButtonsCards(Card card, ListEnum listEnum, int index)
    {      
        GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);
        CardDisplayAtributes cardDisplayAtributes = gO.GetComponentInChildren<CardDisplayAtributes>();
        cardDisplayAtributes.UpdateTextOnCardWithCard(card);

        gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);
        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }

    private void MakeButtonsChampions(Sprite championSprite, ListEnum listEnum, int index)
    {
        GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);       
        gO.GetComponent<Image>().enabled = true;
        gO.GetComponent<Image>().sprite = championSprite;

        gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);
        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
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

                case WhichMethod.discardCard:
                    DiscardCard();
                    break;
                case WhichMethod.ShowGraveyard:

                    break;
                case WhichMethod.ShowDeck:

                    break;
                case WhichMethod.ShowLandmarks:
                    
                    gameState.DestroyLandmark(chosenTargets[0]);
                    break;
            }
            ResetChoice();
            gameState.Refresh();
        }

    }

    public void ResetChoice()
    {
        isChoiceActive = false;
        closeMenuButton.SetActive(false);
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
            if (chosenTargets[0].whichList.myChampions && !gameState.playerChampion.champion.championName.Equals("Duelist"))
            {
                print("Den passar priority via choice memyn");
                gameState.PassPriority();
            }
            if (gameState.hasPriority && chosenTargets[0].whichList.opponentChampions)
                gameState.PassPriority();
        }
        else
        {
            if (gameState.opponentChampion.health <= 0)
                gameState.PassPriority();
        }

        if (chosenTargets[0].whichList.opponentChampions && gameState.opponentChampion.champion.championName.Equals("Duelist"))
        {
            gameState.PassPriority();
        }   
    }

    private void DiscardCard()
    {
        List<string> cards = new List<string>();
        for (int i = 0; i < chosenTargets.Count; i++)
        {
            string card = actionOfPlayer.handPlayer.DiscardSpecificCardWithIndex(chosenTargets[i].index);
            cards.Add(card);
        }

        if (gameState.isOnline)
        {
            RequestDiscardCard request = new RequestDiscardCard(cards);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
        }

        if (!gameState.isItMyTurn)
            gameState.PassPriority();
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

            case WhichMethod.discardCard:
                if(actionOfPlayer.handPlayer.cardsInHand.Count <= 0)
                    return false;
                break;

            case WhichMethod.ShowGraveyard:
                if (graveyard.graveyardPlayer.Count <= 0)
                    return false;
                break;
            case WhichMethod.ShowDeck:
                if (actionOfPlayer.handPlayer.deck.deckPlayer.Count <= 0)
                    return false;
                break;
            case WhichMethod.ShowLandmarks:
                bool checkIfLandmarkPlaced = false;
                foreach (LandmarkDisplay landmarks in GameState.Instance.playerLandmarks)
                {
                    if (landmarks.card != null)
                        checkIfLandmarkPlaced = true;
                }
                if (!checkIfLandmarkPlaced)
                    return false;
                break;
        }
        return true;
    }

    public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod)
    {
        
        //M�ste l�gga in om choicen failar checkifchoice att den ska passa priority om den ska g�ra det
        print("vad blir checken " + CheckIfChoice(theMethod, list));
        if (CheckIfChoice(theMethod, list))
        {
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            ResetChoice();
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/

            IEnumerator enumerator = ShowChoiceMenu(list, amountToTarget, theMethod, 0.01f);
            StartCoroutine(enumerator);
        }
    }
}

public enum WhichMethod
{
    switchChampion, 
    switchChampionDied, 
    switchChampionDiedDiedDied, 
    discardCard,
    ShowGraveyard,
    ShowDeck,
    ShowLandmarks
}
