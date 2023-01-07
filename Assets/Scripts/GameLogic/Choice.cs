using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Choice : MonoBehaviour
{
    private int amountOfTargets = 0;
    private GameState gameState;
    private ActionOfPlayer actionOfPlayer;
    private Graveyard graveyard;
    private WhichMethod whichMethod;
    private Deck deck;
    private Card cardUsed;
    private bool dontPass;
    private List<TargetInfo> chosenTargets = new List<TargetInfo>();
    private List<GameObject> buttonsToDestroy = new List<GameObject>();
    private List<Tuple<WhichMethod, IEnumerator>> waitRoom = new List<Tuple<WhichMethod, IEnumerator>>();

    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private GameObject closeMenuButton;
    [SerializeField] private GameObject choiceMenu;
    [SerializeField] private GameObject choiceOpponentMenu;

    public bool IsChoiceActive;
    public GameObject ConfirmMenuButton;
    public GameObject ButtonHolder;

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
        if (!gameState.hasPriority)
        {
            if (gameState.isItMyTurn && (whichMethod == WhichMethod.SwitchChampionMulligan || whichMethod == WhichMethod.Mulligan))
                ShowOpponentThinking();
            else
                HideOpponentThinking();
        }
		else
			HideOpponentThinking();

		if (!gameState.hasPriority && IsChoiceActive)
            choiceMenu.SetActive(false);
        else if(IsChoiceActive)
            choiceMenu.SetActive(true);
    }

	private void ShowOpponentThinking()
    {
        choiceOpponentMenu.SetActive(true);
    }
    private void HideOpponentThinking()
    {
        choiceOpponentMenu.SetActive(false);
    }

    private IEnumerator ShowChoiceMenu(ListEnum listEnum, int amountToTarget, WhichMethod theMethod, Card cardUsed, float delay)
    {
        if (!IsChoiceActive)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        this.cardUsed = cardUsed;
        choiceMenu.SetActive(true);
        IsChoiceActive = true;

        whichMethod = theMethod;
        amountOfTargets = amountToTarget;

        if (listEnum.opponentChampions && listEnum.opponentLandmarks) // Is used by one switch
        {
            MakeButtonOfChampion(gameState.opponentChampion.Champion, listEnum, 0);
            listEnum.opponentChampions = false;
            for (int i = 0; i < gameState.opponentLandmarks.Count; i++)
            {
                LandmarkDisplay landmarkDisplay = gameState.opponentLandmarks[i];
                if (landmarkDisplay.Card == null) continue;
                
                MakeButtonOfCard(landmarkDisplay.Card, listEnum, i + 1);
            }
            yield return null;
        }

        // Uses no switch because listenum can have multiple true

        if (listEnum.myChampions)
            MakeButtonsOfChampionList(gameState.playerChampions, listEnum);

		if (listEnum.opponentChampions)
			MakeButtonsOfChampionList(gameState.opponentChampions, listEnum);

        if (listEnum.myHand)
            MakeButtonsOfHand(listEnum);

		if (listEnum.myGraveyard)
			MakeButtonsOfGraveyard(graveyard.graveyardPlayer, listEnum);

		if (listEnum.opponentGraveyard)
			MakeButtonsOfGraveyard(graveyard.graveyardOpponent, listEnum);

        if (listEnum.myDeck)
            MakeButtonsOfDeck(listEnum);

        if (listEnum.myLandmarks)
			MakeButtonsOfLandmarks(gameState.playerLandmarks, listEnum);

		if (listEnum.opponentLandmarks)
            MakeButtonsOfLandmarks(gameState.opponentLandmarks, listEnum);

        if (amountOfTargets == -1)
            ConfirmMenuButton.SetActive(true);
    }


    private void MakeButtonsOfChampionList(List<AvailableChampion> champions, ListEnum listEnum)
    {
        for (int i = 0; i < champions.Count; i++)
        {
            AvailableChampion champ = champions[i];

			if (whichMethod != WhichMethod.SwitchChampionMulligan && champ == gameState.playerChampion) continue;

			MakeButtonOfChampion(champ.Champion, listEnum, i);
		}
	}

    private void MakeButtonsOfHand(ListEnum listEnum)
    {
		for (int i = 0; i < actionOfPlayer.HandPlayer.cardsInHand.Count; i++)
		{
			CardDisplay cardDisplay = actionOfPlayer.HandPlayer.cardsInHand[i];

			if (whichMethod != WhichMethod.TransformChampionCard)
				choiceButtonPrefab.SetActive(true);
			else if (cardDisplay.Card.ChampionCard && cardDisplay.Card.ChampionCardType != ChampionCardType.All) continue;

			MakeButtonOfCard(cardDisplay.Card, listEnum, i);
		}
	}

    private void MakeButtonsOfDeck(ListEnum listEnum)
    {
		int count = deck.deckPlayer.Count;
		if (whichMethod == WhichMethod.SeersShack)
		{
			SeersShack seersShack = (SeersShack)cardUsed;
			count = seersShack.CardsShown;
		}
		for (int i = 0; i < count; i++)
		{
			if (Deck.Instance.deckPlayer[i] == null) break;

			MakeButtonOfCard(deck.deckPlayer[i], listEnum, i);
			closeMenuButton.SetActive(true);
		}
	}

    private void MakeButtonsOfGraveyard(List<Card> graveyardCards, ListEnum listEnum)
    {
		for (int i = 0; i < graveyardCards.Count; i++)
		{
			MakeButtonOfCard(graveyardCards[i], listEnum, i);
			closeMenuButton.SetActive(true);
		}
	}

    private void MakeButtonsOfLandmarks(List<LandmarkDisplay> landmarkDisplays, ListEnum listEnum)
    {
		for (int i = 0; i < landmarkDisplays.Count; i++)
		{
			if (landmarkDisplays[i].LandmarkEnabled && landmarkDisplays[i].Card != null)
			{
				MakeButtonOfCard(landmarkDisplays[i].Card, listEnum, i);
				closeMenuButton.SetActive(true);
			}
		}
	}

    private void MakeButtonOfCard(Card card, ListEnum listEnum, int index)
    {      
        GameObject gO = Instantiate(choiceButtonPrefab, ButtonHolder.transform);
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
        GameObject gO = Instantiate(choiceButtonPrefab, ButtonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();
        ChampionAttributes championAttributes = choiceButton.championPrefab.GetComponent<ChampionAttributes>();
        championAttributes.UpdateChampionCard(champion);
        choiceButton.championPrefab.SetActive(true);

        choiceButton.targetInfo = new TargetInfo(listEnum, index);
        buttonsToDestroy.Add(gO);

        if (amountOfTargets == 0)
            gO.GetComponent<Button>().interactable = false;
    }

    public void RemoveTargetInfo(TargetInfo targetInfo) // button press
    {
        chosenTargets.Remove(targetInfo);
    }

    public void AddTargetInfo(TargetInfo targetInfo) // button press
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
            cardUsed.Target = gameState.opponentChampion.Champion;
        else
            cardUsed.LandmarkTarget = gameState.opponentLandmarks[chosenTargets[0].index - 1];
    }

    private void TransformCard()
    {
        CardDisplay cardToTransform = actionOfPlayer.HandPlayer.cardsInHand[chosenTargets[0].index];
        cardToTransform.Card.ChampionCardType = ChampionCardType.All;
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
                Landmarks landmark = (Landmarks)GameState.Instance.opponentLandmarks[chosenTargets[0].index].Card;
                card.DisabledLandmark = landmark;
            }
        }

		if (gameState.isOnline)
		{
            // new request to disable chosen landmark
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
        
        ConfirmMenuButton.SetActive(false);
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
        actionOfPlayer.HandPlayer.FixMulligan(indexes);
        IsChoiceActive = false;
        gameState.PassPriority();
    }

    private void ResetChoice()
    {   
        if(waitRoom.Count > 0)
            waitRoom.Remove(waitRoom[0]);
        
        closeMenuButton.SetActive(false);
        IsChoiceActive = false;
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
        {
            gameState.playerChampion.Champion.WhenCurrentChampion();
            if (gameState.playerChampion.Champion is Duelist)
                dontPass = true;
        }

        if (whichMethod == WhichMethod.SwitchChampionMulligan && gameState.playerChampion.Champion is not Duelist)
            gameState.PassPriority();

    }

    private void PriorityForSwap()
    {
        if (!gameState.isItMyTurn)
        {
            if (chosenTargets[0].whichList.myChampions && !gameState.playerChampion.Champion.ChampionName.Equals("Duelist")) // If my champion is not duelist
                gameState.PassPriority();
        }
        else if (gameState.opponentChampion.Health <= 0 || (whichMethod == WhichMethod.SwitchChampionMulligan && gameState.playerChampion.Champion is Duelist)) // If I swap to duelist when my champion dies
			gameState.PassPriority();

        if (chosenTargets[0].whichList.opponentChampions && gameState.opponentChampion.Champion.ChampionName.Equals("Duelist")) // If I swap opponents chmampion to Duelist
            gameState.PassPriority();
    }

    private void DiscardCard()
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < chosenTargets.Count; i++)
        {
            int card = chosenTargets[i].index;
            indexes.Add(card);
        }

        List<string> cards = actionOfPlayer.HandPlayer.DiscardCardListWithIndexes(indexes);

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
                if (gameState.playerChampions.Count <= 1 || !gameState.canSwap) // Must swap if no other champion is available 
                    return false;
                break;

            case WhichMethod.SwitchChampionEnemy:
                descriptionText.text = "Swap the opponent champion";
                if (gameState.opponentChampions.Count <= 1)
					return false;
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
                if (actionOfPlayer.HandPlayer.cardsInHand.Count <= 0)
                    return false;
                break;

            case WhichMethod.DiscardXCards:
                descriptionText.text = "Choose X cards to discard and deal bonus damage based on the amount of cards discarded";
                if (actionOfPlayer.HandPlayer.cardsInHand.Count <= 0)
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
                foreach (LandmarkDisplay landmarks in GameState.Instance.playerLandmarks) // If Player got no landmarks
                {
                    if (landmarks.Card != null)
                        break;
                }
                return false;

			case WhichMethod.DisableOpponentLandmark:
                descriptionText.text = "Choose which landmark to disable";
                foreach (LandmarkDisplay landmarks in GameState.Instance.opponentLandmarks) // If Opponent got no landmarks
				{
                    if (landmarks.Card != null && landmarks.LandmarkEnabled)
                        break;
                }
                return false;

            case WhichMethod.TransformChampionCard:
                descriptionText.text = "Choose a card to Transform to a non champion card";
                List<CardDisplay> cardsInHand = ActionOfPlayer.Instance.HandPlayer.cardsInHand;
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    Card card = cardsInHand[i].Card;
                    if (card.ChampionCard && card.ChampionCardType != ChampionCardType.All)
                        break;
                }
                return false;

                case WhichMethod.SeersShack:
                descriptionText.text = "Choose which cards to put at the bottom of the deck";
				if (Deck.Instance.deckPlayer.Count < 2) // Seers shack card
					return false;
                break;

            case WhichMethod.Mulligan:
				descriptionText.text = "Mulligan";
				break;

            case WhichMethod.DestroyLandmarkEnemy:
                descriptionText.text = "Choose an opponent landmark to destroy";
                foreach (LandmarkDisplay landmarks in GameState.Instance.opponentLandmarks) // If opponent got landmark
                {
                    if (landmarks.Card != null)
                        break;
                }
                return false;
        }
        return true;
    }

    private void NextInWaitRoom()
    {
        if (waitRoom.Count == 0)
        {
            IsChoiceActive = false;
            if (!gameState.isItMyTurn && !dontPass)
                gameState.PassPriority();
            else
                dontPass = false;
            return;
        }

        if (CheckIfChoice(waitRoom[0].Item1)) // If choice is acceptable, make choice
            StartCoroutine(waitRoom[0].Item2);
        else
        {
            waitRoom.Remove(waitRoom[0]); // else check next choice
			NextInWaitRoom();			
		}
	}

	public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod, Card cardUsed)
	{
		ChoiceMenu(list, amountToTarget, theMethod, cardUsed, 0.01f);
	}

	public void ChoiceMenu(ListEnum list, int amountToTarget, WhichMethod theMethod, Card cardUsed, float delay)
	{
		IEnumerator enumerator = ShowChoiceMenu(list, amountToTarget, theMethod, cardUsed, delay);

		Tuple<WhichMethod, IEnumerator> tuple = new Tuple<WhichMethod, IEnumerator>(theMethod, enumerator);
		waitRoom.Add(tuple);

		if (waitRoom[0].Item2 == tuple.Item2)
			NextInWaitRoom();
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
