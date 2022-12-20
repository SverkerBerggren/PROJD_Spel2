using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Choice : MonoBehaviour
{
    private List<TargetInfo> chosenTargets = new List<TargetInfo>();
    private int amountOfTargets = 0;


    private GameState gameState;
    private ActionOfPlayer actionOfPlayer;
    private Graveyard graveyard;
    private WhichMethod whichMethod;
    private Deck deck;
    private Card cardUsed;

    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject closeMenuButton;
    [SerializeField] private GameObject choiceMenu;
    [SerializeField] private GameObject choiceOpponentMenu;
    public bool isChoiceActive;

    private List<GameObject> buttonsToDestroy = new List<GameObject>();
    private List<Tuple<WhichMethod, IEnumerator>> waitRoom = new List<Tuple<WhichMethod, IEnumerator>>();

    public GameObject confirmMenuButton;
    public GameObject buttonHolder;

    private static Choice instance;
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
        gameState = GameState.Instance;
    }
    private void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
        graveyard = Graveyard.Instance;
        deck = Deck.Instance;

        choiceMenu = transform.GetChild(0).gameObject;
        choiceOpponentMenu = transform.GetChild(1).gameObject;
    }
    private void FixedUpdate()
    {
        if ((!gameState.hasPriority && gameState.isItMyTurn) || whichMethod == WhichMethod.SwitchChampionMulligan && !gameState.hasPriority)
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
        if (!isChoiceActive)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        this.cardUsed = cardUsed;
        choiceMenu.SetActive(true);
        isChoiceActive = true;

        whichMethod = theMethod;
        amountOfTargets = amountToTarget;

        if (listEnum.opponentChampions && listEnum.opponentLandmarks)
        {
            MakeButtonOfChampion(gameState.opponentChampion.champion, listEnum, 0);
            listEnum.opponentChampions = false;
            for (int i = 0; i < gameState.opponentLandmarks.Count; i++)
            {
                LandmarkDisplay landmarkDisplay = gameState.opponentLandmarks[i];
                if (landmarkDisplay.card == null) continue;
                
                MakeButtonOfCard(landmarkDisplay.card, listEnum, i + 1);
            }
            yield return null;
        }

        if (listEnum.myChampions)
        {
            for (int i = 0; i < gameState.playerChampions.Count; i++)
            {
                AvailableChampion champ = gameState.playerChampions[i];

                if (whichMethod != WhichMethod.SwitchChampionMulligan && champ == gameState.playerChampion) continue;
                MakeButtonOfChampion(champ.champion, listEnum, i);
            }
        }

		if (listEnum.opponentChampions)
		{
			for (int i = 0; i < gameState.opponentChampions.Count; i++)
			{
                MakeButtonOfChampion(gameState.opponentChampions[i].champion, listEnum, i);
            }
		}

        if (listEnum.myHand && whichMethod == WhichMethod.TransformChampionCard)
        {
            print("kommer den till Transform delen");
            descriptionText.text = "Choose a card to Transform";
            for (int i = 0; i < actionOfPlayer.handPlayer.cardsInHand.Count; i++)
            {               
                CardDisplay cardDisplay = actionOfPlayer.handPlayer.cardsInHand[i];
                if (cardDisplay.card.championCard && cardDisplay.card.championCardType != ChampionCardType.All)
                    MakeButtonOfCard(cardDisplay.card, listEnum, i);
            }
        }
        else if(listEnum.myHand)
        {
            for (int i = 0; i < actionOfPlayer.handPlayer.cardsInHand.Count; i++)
            {
                CardDisplay cardDisplay = actionOfPlayer.handPlayer.cardsInHand[i];

                choiceButtonPrefab.SetActive(true);
                MakeButtonOfCard(cardDisplay.card, listEnum, i);
            }
        }

        if (listEnum.myGraveyard)
        {
            for (int i = 0; i < graveyard.graveyardPlayer.Count; i++)
            {
                MakeButtonOfCard(graveyard.graveyardPlayer[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }
        if (listEnum.opponentGraveyard)
        {
            for (int i = 0; i < graveyard.graveyardOpponent.Count; i++)
            {
                MakeButtonOfCard(graveyard.graveyardOpponent[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }

        if (listEnum.myDeck && whichMethod == WhichMethod.SeersShack)
        {
			descriptionText.text = "Seers Shack";
            SeersShack seersShack = (SeersShack)cardUsed;
			for (int i = 0; i < seersShack.cardsShown; i++)
			{
                if (Deck.Instance.deckPlayer[i] != null)
                {
				    MakeButtonOfCard(deck.deckPlayer[i], listEnum, i);
				    closeMenuButton.SetActive(true);
                }
			}
		}
        else if (listEnum.myDeck)
        {
            descriptionText.text = "Show Deck";
            for (int i = 0; i < deck.deckPlayer.Count; i++)
            {
                MakeButtonOfCard(deck.deckPlayer[i], listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }

        if (listEnum.myLandmarks)
        { 
            for (int i = 0; i < gameState.playerLandmarks.Count; i++)
            {
                MakeButtonOfCard(gameState.playerLandmarks[i].card, listEnum, i);
                closeMenuButton.SetActive(true);
            }
        }


        if (listEnum.opponentLandmarks)
        {
            for (int i = 0; i < gameState.opponentLandmarks.Count; i++)
            {
                if (gameState.opponentLandmarks[i].landmarkEnabled && gameState.opponentLandmarks[i].card != null)
                {
                    MakeButtonOfCard(gameState.opponentLandmarks[i].card, listEnum, i);
                    closeMenuButton.SetActive(true);
                }
            }
        }

        if (amountOfTargets == -1)
            confirmMenuButton.SetActive(true);
    }

    private void MakeButtonOfCard(Card card, ListEnum listEnum, int index)
    {      
        GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();
        choiceButton.cardPrefab.SetActive(true);
        gO.GetComponentInChildren<CardDisplayAttributes>().UpdateTextOnCardWithCard(card);

        choiceButton.targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);

        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }

    private void MakeButtonOfChampion(Champion champion, ListEnum listEnum, int index)
    {
        GameObject gO = Instantiate(choiceButtonPrefab, buttonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();

        //gO.GetComponentInParent<GridLayoutGroup>().spacing = new Vector2(100, -100);
        ChampionAttributes championAttributes = choiceButton.championPrefab.GetComponent<ChampionAttributes>();
        championAttributes.UpdateChampionCard(champion);
        choiceButton.championPrefab.SetActive(true);

        choiceButton.targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);

        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }

    public void RemoveTargetInfo(TargetInfo targetInfo)
    {
        chosenTargets.Remove(targetInfo);
    }

    public void AddTargetInfo(TargetInfo targetInfo)
    {
        chosenTargets.Add(targetInfo);

        if (amountOfTargets == -1) return;


        if (chosenTargets.Count == amountOfTargets)
        {
            switch(whichMethod)
            {
                case WhichMethod.SwitchChampionMulligan:
					SwitchChamp(false);
					break;

				case WhichMethod.SwitchChampionPlayer:
                    SwitchChamp(false);                   
                    break;

                case WhichMethod.SwitchChampionEnemy:
                    SwitchChamp(false);
                    break;

                case WhichMethod.SwitchChampionDied:
                    SwitchChamp(true);                    
                    break;

                case WhichMethod.DiscardCard:
                    DiscardCard();
                    break;

                case WhichMethod.ShowDeck:
                    print("Card 1: " + deck.deckPlayer[chosenTargets[0].index] + "  Card 2: " + deck.deckPlayer[chosenTargets[1].index]);
                    break;

                case WhichMethod.DestroyLandmarkPlayer:
                    gameState.DestroyLandmark(chosenTargets[0]);
                    break;

                case WhichMethod.DisableOpponentLandmark:
                    DisableChosenLandmark();
                    break;

                case WhichMethod.TransformChampionCard:
                    TransformCard();
                    break;
                case WhichMethod.OneSwitchTarget:
                    OneSwitchTarget();
                    break;
                case WhichMethod.DestroyLandmarkEnemy:
                    gameState.opponentLandmarks[chosenTargets[0].index].DestroyLandmark();
                    break;
            }
            cardUsed = null;
            whichMethod = WhichMethod.Null;
			ResetChoice();
            gameState.Refresh();
			//waitRoom.Remove(waitRoom[0]);
			NextInWaitRoom();			
		}
    }

    private void OneSwitchTarget()
    {
        if (chosenTargets[0].whichList.opponentChampions)
            cardUsed.Target = gameState.opponentChampion.champion;
        else
            cardUsed.LandmarkTarget = gameState.opponentLandmarks[chosenTargets[0].index - 1];
    }

    private void TransformCard()
    {
        CardDisplay cardToTransform = actionOfPlayer.handPlayer.cardsInHand[chosenTargets[0].index];
        cardToTransform.card.championCardType = ChampionCardType.All;
    }

	private void SeersShackAbility()
	{
        Deck deck = Deck.Instance;
        Card card;
        for (int i = 0; i < chosenTargets.Count; i++)
        {
            card = deck.deckPlayer[chosenTargets[i].index - i];
            deck.deckPlayer.RemoveAt(chosenTargets[i].index - i);
            deck.deckPlayer.Add(card);
        }
	}

	private void DisableChosenLandmark()
	{
        gameState.ChangeLandmarkStatus(chosenTargets[0], false);
        if (cardUsed is DisableCardLandmark)
        {
            DisableCardLandmark card = (DisableCardLandmark)cardUsed;
            if (chosenTargets[0].whichList.opponentLandmarks)
            {
                Landmarks landmark = (Landmarks)GameState.Instance.opponentLandmarks[chosenTargets[0].index].card;
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
        switch (whichMethod)
        {
            case WhichMethod.DiscardXCards:
            DiscardCard();
            ShankerAttack shankAttack = (ShankerAttack)cardUsed;
            shankAttack.WaitForChoices(chosenTargets.Count);
                break;

            case WhichMethod.Mulligan:
                Mulligan();
            break;

            case WhichMethod.SeersShack:
                SeersShackAbility();
                break;

        }
        
        ResetChoice();
        gameState.Refresh();
        
        confirmMenuButton.SetActive(false);
        cardUsed = null;
        whichMethod = WhichMethod.Null;
        NextInWaitRoom();
    }

    private void Mulligan()
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < chosenTargets.Count; i++)
        {
            int card = chosenTargets[i].index;
            indexes.Add(card);
        }
        actionOfPlayer.handPlayer.FixMulligan(indexes);
        isChoiceActive = false;

        if(!gameState.isItMyTurn)
	        gameState.PassPriority();
	}

    public void ResetChoice()
    {   
        if(waitRoom.Count > 0)
            waitRoom.Remove(waitRoom[0]);
        
        closeMenuButton.SetActive(false);
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
        gameState.SwapChampionWithTargetInfo(chosenTargets[0], died);

        if (gameState.isOnline)
        {
            RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(chosenTargets[0]);
            request.whichPlayer = ClientConnection.Instance.playerId;
            request.championDied = died;

            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
        }

        if (cardUsed is DuelistAttack)
        {
            DuelistAttack duelistAttack = (DuelistAttack)cardUsed;
            duelistAttack.WaitForChoice();
        }

        PriorityForSwap();
        
        if (chosenTargets[0].whichList.myChampions)
            gameState.playerChampion.champion.WhenCurrentChampion();

        if (whichMethod == WhichMethod.SwitchChampionMulligan && gameState.playerChampion.champion is not Duelist)
            gameState.PassPriority();

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
        }
        else if (gameState.opponentChampion.health <= 0 || (whichMethod == WhichMethod.Mulligan && gameState.playerChampion.champion is Duelist))
            gameState.PassPriority();

        if (chosenTargets[0].whichList.opponentChampions && gameState.opponentChampion.champion.championName.Equals("Duelist"))
        {
            gameState.PassPriority();
        }   
    }

    private void DiscardCard()
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < chosenTargets.Count; i++)
        {
            int card = chosenTargets[i].index;
            indexes.Add(card);
        }

        List<string> cards = actionOfPlayer.handPlayer.DiscardCardListWithIndexes(indexes);

        if (gameState.isOnline)
        {
            RequestDiscardCard request = new RequestDiscardCard(cards, false);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, gameState.RequestEmpty);
        }
    }

    private bool CheckIfChoice(WhichMethod theMethod)
    {
        switch (theMethod)
        {
            case WhichMethod.SwitchChampionMulligan:
                descriptionText.text = "Choose your starting champion";
            break;

            case WhichMethod.SwitchChampionPlayer:
                descriptionText.text = "Swap your champion";
                if (gameState.playerChampions.Count <= 1 || !gameState.canSwap)
                {
                    return false;
                }
                break;

            case WhichMethod.SwitchChampionEnemy:
                descriptionText.text = "Swap the opponent champion";
                if (gameState.opponentChampions.Count <= 1)
				{
					return false;
				}
                break;
            
            case WhichMethod.SwitchChampionDied:
                descriptionText.text = "Your champion died, Swap your champion";
                if (gameState.playerChampions.Count <= 1)
                {
                    gameState.Defeat();
                    return false;
                }
                break;

            case WhichMethod.DiscardCard:
                descriptionText.text = "Choose a card to discard";
                if (actionOfPlayer.handPlayer.cardsInHand.Count <= 0)
                    return false;
                break;
            case WhichMethod.DiscardXCards:
                descriptionText.text = "Choose X cards to discard and deal bonus damage based on the amount of cards discarded";
                if (actionOfPlayer.handPlayer.cardsInHand.Count <= 0)
                    return false;
                break;

            case WhichMethod.ShowGraveyard:
                descriptionText.text = "Player Graveyard";
                if (graveyard.graveyardPlayer.Count <= 0)
                    return false;
                break;

            case WhichMethod.ShowOpponentGraveyard:
                descriptionText.text = "Opponent Graveyard";
                if (graveyard.graveyardOpponent.Count <= 0)
                    return false;
                break;

            case WhichMethod.ShowDeck:
                descriptionText.text = "Player Deck";
                if (deck.deckPlayer.Count <= 0)
                    return false;
                break;

            case WhichMethod.DestroyLandmarkPlayer:
                descriptionText.text = "Choose a landmark to sacrifice";
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
                descriptionText.text = "Choose which landmark to disable";
                bool ifLandmarkExist = false;
                foreach (LandmarkDisplay landmarks in GameState.Instance.opponentLandmarks)
                {
                    if (landmarks.card != null && landmarks.landmarkEnabled)
                        ifLandmarkExist = true;
                }
                if (!ifLandmarkExist)
                    return false;
                break;
            case WhichMethod.TransformChampionCard:
                descriptionText.text = "Choose a card to Transform to a non champion card";
                List<CardDisplay> cardsInHand = ActionOfPlayer.Instance.handPlayer.cardsInHand;
                bool thereIsAChampionCardToTransform = false;
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    Card card = cardsInHand[i].card;
                    if (card.championCard && card.championCardType != ChampionCardType.All)
                    {
                        thereIsAChampionCardToTransform = true;
                        break;
                    }
                }
                if (!thereIsAChampionCardToTransform)
                {
                    print("No Champion Card");
                    return false;
                }
                break;

                case WhichMethod.SeersShack:
                descriptionText.text = "Choose which cards to put at the bottom of the deck";
                break;

            case WhichMethod.Mulligan:
				descriptionText.text = "Mulligan";
				break;
            case WhichMethod.DestroyLandmarkEnemy:
                descriptionText.text = "Choose an opponent landmark to destroy";
                bool anyLandmarksToDestory = false;
                foreach (LandmarkDisplay landmarks in GameState.Instance.opponentLandmarks)
                {
                    if (landmarks.card != null)
                        anyLandmarksToDestory = true;
                }
                if (!anyLandmarksToDestory)
                    return false;
                break;
        }
        return true;
    }

    public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod, Card cardUsed)
    {
        ChoiceMenu(list,amountToTarget,theMethod,cardUsed, 0.01f);
    }

    public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod, Card cardUsed, float delay)
    {
        IEnumerator enumerator = ShowChoiceMenu(list, amountToTarget, theMethod, cardUsed, delay);

        Tuple<WhichMethod, IEnumerator> tuple = new Tuple<WhichMethod, IEnumerator>(theMethod, enumerator);
        waitRoom.Add(tuple);
        
        if (waitRoom[0].Item2 == tuple.Item2)       
            NextInWaitRoom();       
        else     
            print("Choice not First");
        
        //M�ste l�gga in om choicen failar checkifchoice att den ska passa priority om den ska g�ra det
    }

    private void NextInWaitRoom()
    {
        if (waitRoom.Count == 0)
        {
            isChoiceActive = false;
            if (!gameState.isItMyTurn)
                gameState.PassPriority();
            return;
        }

        if (CheckIfChoice(waitRoom[0].Item1))
        {
            //isChoiceActive = true;
            StartCoroutine(waitRoom[0].Item2);
        }
        else
        {
            waitRoom.Remove(waitRoom[0]);
            NextInWaitRoom();			
		}
	}
}

public enum WhichMethod
{
    Null,
    SwitchChampionMulligan,
    SwitchChampionPlayer,
	SwitchChampionEnemy,
	SwitchChampionDied,
    DiscardCard,
    DiscardXCards,
    ShowGraveyard,
    ShowDeck,
    DestroyLandmarkPlayer,
    DisableOpponentLandmark,
	SeersShack,
    TransformChampionCard,
    Mulligan,
    OneSwitchTarget,
    DestroyLandmarkEnemy,
    ShowOpponentGraveyard
}
