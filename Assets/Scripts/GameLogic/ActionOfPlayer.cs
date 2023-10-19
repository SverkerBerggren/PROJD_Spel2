using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ActionOfPlayer : MonoBehaviour
{
	private Choice choice;
    private GameState gameState;
    private Graveyard graveyard;
    private int cardCost;
	private double timer = 0;
	private NewOneSwitch newOneSwitch;

    public TMP_Text RoundCounter;
    public Sprite BackfaceCard;

    public Hand HandPlayer;
    public Hand HandOpponent;

	public bool OneSwitchOn = false;

    public int PlayerMana = 0;
    public int EnemyMana = 0;
    public int CurrentMana = 0;
    public readonly int MaxMana = 10;
    public int UnspentMana = 0;
    public bool SelectCardOption = false;



    private static ActionOfPlayer instance;

    public static ActionOfPlayer Instance { get { return instance; } set { instance = value; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        gameState = GameState.Instance;
    }

	private void Start()
	{
        choice = Choice.Instance;
		graveyard = Graveyard.Instance;
		newOneSwitch = GetComponent<NewOneSwitch>();

    }

	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
            timer = 0;       
		}
        if (Input.GetKey(KeyCode.Space))
        {
			timer += Time.deltaTime;
            if (timer > 3)
            {
				OneSwitchOn = !OneSwitchOn;
                newOneSwitch.enabled = !newOneSwitch.enabled;
                timer = 0;
            }
        }

        if (Input.GetKey(KeyCode.M))
            PlayerMana++;

        if (Input.GetKey(KeyCode.D))
            GameState.Instance.DrawCard(1, null);

        if (Input.GetKeyDown(KeyCode.P))
            gameState.HasPriority = true;

        if (Input.GetKeyDown(KeyCode.W))
        {
            ListEnum lE = new ListEnum();
            lE.myChampions = true;
            choice.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionPlayer, null);
        }
    }

    private int DrawDrawnCards(Hand hand, bool isPlayer, int amountToDraw, Card specificCard)
    {
		int drawnCards = 0;
		foreach (CardDisplay cardDisplay in hand.cardSlotsInHand)
		{
			if (drawnCards >= amountToDraw) break;
			if (cardDisplay.Card != null) continue;

			if (!cardDisplay.gameObject.activeSelf)
			{
				if (!isPlayer)
					cardDisplay.SetBackfaceOnOpponentCards(BackfaceCard);

				if (specificCard == null)
					cardDisplay.Card = Deck.Instance.WhichCardToDrawPlayer(isPlayer);
				else
					cardDisplay.Card = specificCard;

                if (CheckCardDrawn(cardDisplay, drawnCards) == -1) break;

				drawnCards++;
			}
		}
        return drawnCards;
	}

    private int CheckCardDrawn(CardDisplay cardDisplay, int drawnCards)
    {
        if (cardDisplay.Card != null)
        {
            cardDisplay.gameObject.SetActive(true);
            if (drawnCards == 0)
            {
                gameState.DrawnCardsThisTurn -= 1;
                cardDisplay.firstCardDrawn = true;
            }
            drawnCards++;
        }
        else
            return -1;

        return drawnCards;
    }

    private void DiscardOverdrawnCards(int drawnCards, int amountToDraw, bool isPlayer)
    {
        if (drawnCards < amountToDraw) //Discards overdrawn cards
        {
            List<string> cardNames = new List<string>();
            for (; drawnCards < amountToDraw; drawnCards++)
            {
                Card c = Deck.Instance.WhichCardToDrawPlayer(isPlayer);

                if (isPlayer)
                    graveyard.AddCardToGraveyard(c);
                else if(!isPlayer && !gameState.IsOnline)
					graveyard.AddCardToGraveyardOpponent(c);

				cardNames.Add(c.CardName);
            }

            if (gameState.IsOnline && isPlayer)
            {
                RequestDiscardCard requesten = new RequestDiscardCard();
                requesten.whichPlayer = ClientConnection.Instance.playerId;
                requesten.listOfCardsDiscarded = cardNames;
                requesten.discardCardToOpponentGraveyard = false;
                requesten.listEnum.myDeck = true;
                ClientConnection.Instance.AddRequest(requesten, GameState.Instance.RequestEmpty);

            }
        }
    }

    private void MoveCards(CardDisplay cardDisplay, bool isPlayer) // Cards moves to their right place in the hand
	{
		int index = HandPlayer.cardSlotsInHand.IndexOf(cardDisplay);
		CardDisplay cardDisplayToSwapTo;
        CardDisplay cardDisplayToSwapFrom;
        for (int i = index + 1; i < HandPlayer.cardSlotsInHand.Count; i++)
		{
			cardDisplayToSwapFrom = HandPlayer.cardSlotsInHand[i].GetComponent<CardDisplay>();

            if (cardDisplayToSwapFrom.Card == null || i - 1 < 0) continue;

			cardDisplayToSwapTo = HandPlayer.cardSlotsInHand[i - 1].GetComponent<CardDisplay>();

			if (isPlayer)
			{
				cardDisplayToSwapTo.ManaCost = cardDisplayToSwapFrom.ManaCost;

				if (cardDisplayToSwapFrom.Card.TypeOfCard == CardType.Landmark)
					cardDisplayToSwapTo.cardDisplayAttributes.hpText.text = cardDisplayToSwapFrom.cardDisplayAttributes.hpText.text;
			}

			cardDisplayToSwapTo.Card = cardDisplayToSwapFrom.Card;
			cardDisplayToSwapFrom.Card = null;
		}
	}

	public void UpdateUnspentMana()
	{
		UnspentMana += CurrentMana;
	}

	public bool CheckIfCanPlayCard(CardDisplay cardDisplay, bool useMana)
	{
		cardCost = cardDisplay.ManaCost;
		if (CurrentMana >= cardCost)
		{
			if (useMana)
				CurrentMana -= cardCost;
			return true;
		}
		else
		{
			Debug.Log("You don't have enough Mana");
			return false;
		}
	}

	public void DrawCardPlayer(int amountToDraw, Card specificCard, bool isPlayer)
	{
		Hand hand;
		if (isPlayer)
		{
			hand = HandPlayer;
			GameState.Instance.DrawnCardsThisTurn += amountToDraw;
		}
		else
			hand = HandOpponent;

		int drawnCards = DrawDrawnCards(hand, isPlayer, amountToDraw, specificCard);
		DiscardOverdrawnCards(drawnCards, amountToDraw, isPlayer);
	}

	public void IncreaseMana()
	{
		if (PlayerMana < MaxMana)
			PlayerMana++;

		CurrentMana = PlayerMana;
	}

	public string DiscardWhichCard(bool yourself)
	{
		string discardedCard;

		if (yourself)
			discardedCard = HandPlayer.DiscardRandomCardInHand().CardName;
		else
			discardedCard = HandOpponent.DiscardRandomCardInHand().CardName;

		return discardedCard;
	}

	public void ChangeCardOrder(bool isPlayer, CardDisplay cardDisplay)
	{
		if (isPlayer)
			HandPlayer.FixCardOrderInHand();
		else
		{
			cardDisplay.Card = null;
			HandOpponent.FixCardOrderInHand();
			return;
		}

		cardDisplay.Card = null;

		if (SelectCardOption)
			cardDisplay.gameObject.GetComponent<CardMovement>().ClickedOnCard = false;

		MoveCards(cardDisplay, isPlayer);
		HandPlayer.FixCardOrderInHand();
	}
}
