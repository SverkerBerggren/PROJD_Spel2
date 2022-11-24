using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using TMPro;
using System.Linq;
using UnityEngine.Networking.Types;

public class ActionOfPlayer : MonoBehaviour
{
    public Hand handPlayer;
    public Hand handOpponent;

    [SerializeField] private TMP_Text manaText;
	public Sprite backfaceCard;

	private Choice choice;

    private int cardCost;
    public int playerMana = 0;
    public int currentMana = 0;
    public readonly int maxMana = 10;
    public bool selectCardOption = false;

    private GameState gameState;
    private Graveyard graveyard;
    private static ActionOfPlayer instance;

    public static ActionOfPlayer Instance { get { return instance; } set { instance = value; } }

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
        choice = Choice.Instance;
		graveyard = Graveyard.Instance;
	}



	private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            playerMana++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            GameState.Instance.DrawCard(1, null);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ListEnum lE = new ListEnum();
            lE.myChampions = true;
            choice.ChoiceMenu(lE, 1, WhichMethod.switchChampion);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState.hasPriority = true;
        }

    }

    private void FixedUpdate()
    {
        manaText.text = "Mana: " + currentMana.ToString();
    }

    public bool CheckIfCanPlayCard(CardDisplay cardDisplay)
    {
        cardCost = cardDisplay.manaCost;
        if (gameState.factory > 0)        
            if (gameState.playerLandmarks.Count >= 3)
                cardCost -= (2 * gameState.factory);

        if (currentMana >= cardCost)
        {
            currentMana -= cardCost;
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
		int drawnCards = 0;
		Hand hand;
		if (isPlayer)
			hand = handPlayer;
		else
			hand = handOpponent;

		foreach (GameObject cardSlot in hand.cardSlotsInHand)
		{
			CardDisplay cardDisplay = cardSlot.GetComponent<CardDisplay>();
			if (cardDisplay.card != null) continue;

			if (!cardSlot.activeSelf)
			{
				if (drawnCards >= amountToDraw) break;

				if (!isPlayer)
				{
                    cardDisplay.SetBackfaceOnOpponentCards(backfaceCard);
				}

				if (specificCard == null)
					cardDisplay.card = hand.deck.WhichCardToDrawPlayer();
				else
					cardDisplay.card = specificCard;

				if (cardDisplay.card != null)
				{
					cardDisplay.manaCost = cardDisplay.card.maxManaCost;
					cardSlot.SetActive(true);
					drawnCards++;
					//if (isPlayer)
						//gameState.playerChampion.champion.DrawCard(cardDisplay);
				}
				else
				{
					print("Deck is empty or the drawn card is null!!!");
					//gameState.Defeat();
					break;
				}
			}
		}

		if (drawnCards < amountToDraw)
		{
			for (; drawnCards < amountToDraw; drawnCards++)
			{
				Card c = hand.deck.WhichCardToDrawPlayer();
				if (isPlayer)
					graveyard.AddCardToGraveyard(c);
				else
					graveyard.AddCardToGraveyardOpponent(c);
			}
		}

	}

	public void IncreaseMana()
    {
        if(playerMana < maxMana)
            playerMana++;

        currentMana = playerMana;
    }

	public string DiscardWhichCard(bool yourself)
	{
		string discardedCard = "";
		if (yourself)
			discardedCard = handPlayer.DiscardRandomCardInHand().cardName;
		else
			discardedCard = handOpponent.DiscardRandomCardInHand().cardName;
		return discardedCard;
	}

	public void ChangeCardOrder(bool isPlayer, CardDisplay cardDisplay)
    {
        if (isPlayer)
        {
            handPlayer.FixCardOrderInHand();
        }
        else
        {
            cardDisplay.card = null;
            handOpponent.FixCardOrderInHand();
            return;
        }
            

        int index = handPlayer.cardSlotsInHand.IndexOf(cardDisplay.gameObject);
        CardDisplay cardDisplayToSwapTo;
        CardDisplay cardDisplayToSwapFrom = null;
        cardDisplay.card = null;
        if (selectCardOption) cardDisplay.gameObject.GetComponent<CardMovement>().clickedOnCard = false;
        for (int i = index + 1; i < handPlayer.cardSlotsInHand.Count; i++)
        {
            cardDisplayToSwapFrom = handPlayer.cardSlotsInHand[i].GetComponent<CardDisplay>();
            if (cardDisplayToSwapFrom.card != null)
            {
                if (i - 1 < 0) continue;

                cardDisplayToSwapTo = handPlayer.cardSlotsInHand[i - 1].GetComponent<CardDisplay>();
                

                if (isPlayer)
                {
                    cardDisplayToSwapTo.manaCost = cardDisplayToSwapFrom.manaCost;

                    if (cardDisplayToSwapFrom.card.typeOfCard == CardType.Landmark)
                    {
                        cardDisplayToSwapTo.cardDisplayAtributes.hpText.text = cardDisplayToSwapFrom.cardDisplayAtributes.hpText.text;
                    }
                }

                cardDisplayToSwapTo.card = cardDisplayToSwapFrom.card;
                cardDisplayToSwapFrom.card = null;
            }
        }
        handPlayer.FixCardOrderInHand();
    }
}
