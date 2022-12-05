using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Choice : MonoBehaviour
{
    private List<TargetInfo> chosenTargets = new List<TargetInfo>();
    private int amountOfTargets = 0;

    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject closeMenuButton;
    [SerializeField] private GameObject confirmMenuButton;
    [SerializeField] private GameObject buttonHolder;

    private GameState gameState;
    private ActionOfPlayer actionOfPlayer;
    private Graveyard graveyard;
    private WhichMethod whichMethod;
    private static Choice instance;
    private GameObject choiceMenu;
    private GameObject choiceOpponentMenu;
    private List<GameObject> buttonsToDestroy = new List<GameObject>();
    private bool isChoiceActive = false;
    private Card cardUsed;
    private List<Tuple<WhichMethod, IEnumerator>> waitRoom = new List<Tuple<WhichMethod, IEnumerator>>();


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
            choiceMenu.SetActive(false);
        else if(isChoiceActive)
            choiceMenu.SetActive(true);
    }

    public void ShowOpponentThinking()
    {
        choiceOpponentMenu.SetActive(true);
    }
    public void HideOpponentThinking()
    {
        choiceOpponentMenu.SetActive(false);
    }

    private IEnumerator ShowChoiceMenu(ListEnum listEnum, int amountToTarget, WhichMethod theMethod, Card cardUsed, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(cardUsed != null)
            this.cardUsed = cardUsed;

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

                MakeButtonOfChampion(champ.champion.artwork, listEnum, i);
            }
        }

		if (listEnum.opponentChampions)
		{
			for (int i = 0; i < gameState.opponentChampions.Count; i++)
			{
				Sprite champSprite = gameState.opponentChampions[i].champion.artwork;

                MakeButtonOfChampion(champSprite, listEnum, i);
            }
		}

        if(listEnum.myHand)
        {
            print("kommer den till discard delen");
            descriptionText.text = "Choose a card to discard";
            for (int i = 0; i < actionOfPlayer.handPlayer.cardsInHand.Count; i++)
            {
                CardDisplay cardDisplay = actionOfPlayer.handPlayer.cardsInHand[i].GetComponent<CardDisplay>();
                if (amountOfTargets == -1)
                    confirmMenuButton.SetActive(true);
                MakeButtonOfCard(cardDisplay.card, listEnum, i);
            }
        }

        if (listEnum.myGraveyard)
        {
            descriptionText.text = "Show graveyard";
            for (int i = 0; i < graveyard.graveyardPlayer.Count; i++)
            {
                MakeButtonOfCard(graveyard.graveyardPlayer[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }

        if (listEnum.myDeck && whichMethod == WhichMethod.SeersShack)
        {
			descriptionText.text = "Seers Shack";
            SeersShack seersShack = (SeersShack)cardUsed;
			for (int i = 0; i < seersShack.cardsShown; i++)
			{
                if (actionOfPlayer.handPlayer.deck.deckPlayer[i] != null)
                {
				    MakeButtonOfCard(actionOfPlayer.handPlayer.deck.deckPlayer[i], listEnum, i);
				    closeMenuButton.SetActive(true);
                }
			}
		}
        else if (listEnum.myDeck)
        {
            descriptionText.text = "Show Deck";
            for (int i = 0; i < actionOfPlayer.handPlayer.deck.deckPlayer.Count; i++)
            {
                MakeButtonOfCard(actionOfPlayer.handPlayer.deck.deckPlayer[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }

        if (listEnum.myLandmarks)
        {
            descriptionText.text = "Show landmarks";
            for (int i = 0; i < gameState.playerLandmarks.Count; i++)
            {
                MakeButtonOfCard(gameState.playerLandmarks[i].card, listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }


        if (listEnum.opponentLandmarks)
        {
            descriptionText.text = "Show opponent landmarks";
            for (int i = 0; i < gameState.opponentLandmarks.Count; i++)
            {
                if (gameState.opponentLandmarks[i].landmarkEnabled)
                {
                    MakeButtonOfCard(gameState.opponentLandmarks[i].card, listEnum, i);
                    closeMenuButton.SetActive(true);
                }
            }
        }
    }

    private void MakeButtonOfCard(Card card, ListEnum listEnum, int index)
    {      
        GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);
        CardDisplayAtributes cardDisplayAtributes = gO.GetComponentInChildren<CardDisplayAtributes>();
        cardDisplayAtributes.UpdateTextOnCardWithCard(card);

        gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);
        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }

    private void MakeButtonOfChampion(Sprite championSprite, ListEnum listEnum, int index)
    {
        GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);       
        gO.GetComponent<Image>().enabled = true;
        gO.GetComponent<Image>().sprite = championSprite;
        gO.transform.localScale = new Vector3(1.3f, 1, 0.4f);

        gO.GetComponentInParent<GridLayoutGroup>().spacing = new Vector2(100, -100);

        print(gO.transform.childCount);
        gO.transform.GetChild(0).gameObject.SetActive(false);

        gO.GetComponent<ChoiceButton>().targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);
        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }



    public void AddTargetInfo(TargetInfo targetInfo)
    {
        chosenTargets.Add(targetInfo);

        if (amountOfTargets == -1) return;


        if (chosenTargets.Count == amountOfTargets)
        {
            switch(whichMethod)
            {
                case WhichMethod.switchChampionPlayer:
                    SwitchChamp(false);                   
                    break;

                case WhichMethod.switchChampionDied:
                    SwitchChamp(true);                    
                    break;
                case WhichMethod.switchChampionEnemy:
                    SwitchChamp(false);
                    break;

                case WhichMethod.discardCard:
                    DiscardCard();
                    break;

                case WhichMethod.discardXCardsInMyHand:

                    break;

                case WhichMethod.ShowGraveyard:

                    break;

                case WhichMethod.ShowDeck:
                    print("Card 1: " + actionOfPlayer.handPlayer.deck.deckPlayer[chosenTargets[0].index] + "  Card 2: " + actionOfPlayer.handPlayer.deck.deckPlayer[chosenTargets[1].index]);
                    break;

                case WhichMethod.ShowLandmarks:
                    gameState.DestroyLandmark(chosenTargets[0]);
                    break;

                case WhichMethod.DisableOpponentLandmark:
                    DisableChosenLandmark(chosenTargets[0]);
                    break;

                case WhichMethod.SeersShack:
                    SeersShackAbility(chosenTargets[0]);
                    break;
            }
            ResetChoice();
            gameState.Refresh();

			waitRoom.Remove(waitRoom[0]);
			if (waitRoom.Count > 0)
			{
				NextInWaitRoom(waitRoom[0]);
			}
		}

    }

	private void SeersShackAbility(TargetInfo targetInfo)
	{
        Deck deck = Deck.Instance;
        Card card = deck.deckPlayer[targetInfo.index];
        deck.deckPlayer.RemoveAt(targetInfo.index);
        deck.deckPlayer.Add(card);
	}

	private void DisableChosenLandmark(TargetInfo chosenTargets)
	{
        gameState.ChangeLandmarkStatus(chosenTargets, false);
        if (cardUsed is DisableCardLandmark)
        {
            DisableCardLandmark card = (DisableCardLandmark)cardUsed;
            if (chosenTargets.whichList.opponentLandmarks)
            {
                Landmarks landmark = (Landmarks)GameState.Instance.opponentLandmarks[chosenTargets.index].card;
                card.disabledLandmark = landmark;
            }
        }

		if (gameState.isOnline)
		{
            //Ny request
		}
    }

	public void PressedConfirmButton()
    {

        if (whichMethod == WhichMethod.discardXCardsInMyHand)
        {
            DiscardXCards();
            ShankerAttack shankAttack = (ShankerAttack)cardUsed;
            shankAttack.WaitForChoices(chosenTargets.Count);
        }

    }

    public int HowManyChoicesWhereMade()
    {
        return chosenTargets.Count;
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

    private void DiscardXCards()
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

    private bool CheckIfChoice(WhichMethod theMethod)
    {
        switch (theMethod)
        {
            case WhichMethod.switchChampionPlayer:
                if (gameState.playerChampions.Count <= 1 || !gameState.canSwap)
                {
                    return false;
                }
                break;

            case WhichMethod.switchChampionEnemy:
                if (gameState.opponentChampions.Count <= 1)
				{
					return false;
				}
                break;

            case WhichMethod.switchChampionDied:
                if (gameState.playerChampions.Count <= 1)
                {
                    return false;
                }
                break;

            case WhichMethod.discardCard:
                if(actionOfPlayer.handPlayer.cardsInHand.Count <= 0)
                    return false;
                break;
            case WhichMethod.discardXCardsInMyHand:
                if (actionOfPlayer.handPlayer.cardsInHand.Count <= 0)
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

			case WhichMethod.DisableOpponentLandmark:
                bool ifLandmarkExist = false;
                foreach (LandmarkDisplay landmarks in GameState.Instance.opponentLandmarks)
                {
                    if (landmarks.card != null && landmarks.landmarkEnabled)
                        ifLandmarkExist = true;
                }
                if (!ifLandmarkExist)
                    return false;
                break;
        }
        return true;
    }

    public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod, Card cardUsed)
    {
        
        IEnumerator enumerator = ShowChoiceMenu(list, amountToTarget, theMethod, cardUsed, 0.01f);
        Tuple<WhichMethod, IEnumerator> tuple = new Tuple<WhichMethod, IEnumerator>(theMethod, enumerator);
        waitRoom.Add(tuple);
        if (waitRoom[0] == tuple)
        {
            NextInWaitRoom(tuple);
        }
        //M�ste l�gga in om choicen failar checkifchoice att den ska passa priority om den ska g�ra det
        print("vad blir checken " + CheckIfChoice(theMethod));

    }

    private void NextInWaitRoom(Tuple<WhichMethod, IEnumerator> choice)
    {
        if (CheckIfChoice(choice.Item1))
        {
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            /* KAN SKAPA PROPLEM S� JAG F�RS�KER G�RA DEN L�TT ATT SE*/
            ResetChoice();
            StartCoroutine(choice.Item2);
        }
        else
        {
            waitRoom.Remove(choice);
            if (waitRoom.Count > 0)
            {
                NextInWaitRoom(waitRoom[0]);
			}
		}
	}
}

public enum WhichMethod
{
    switchChampionPlayer,
	switchChampionEnemy,
	switchChampionDied, 
    switchChampionDiedDiedDied, 
    discardCard,
    discardXCardsInMyHand,
    ShowGraveyard,
    ShowDeck,
    ShowLandmarks,
    DisableOpponentLandmark,
	SeersShack
}
