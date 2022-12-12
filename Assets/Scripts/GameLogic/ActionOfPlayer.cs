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
	private Choice choice;
    private GameState gameState;
    private Graveyard graveyard;


    private int cardCost;
    [SerializeField] private TMP_Text manaText;
    public TMP_Text roundCounter;
    public Sprite backfaceCard;

    public Hand handPlayer;
    public Hand handOpponent;

    public int playerMana = 0;
    public int enemyMana = 0;
    public int currentMana = 0;
    public readonly int maxMana = 10;
    public int unspentMana = 0;
    public bool selectCardOption = false;

    private static ActionOfPlayer instance;

    public int unspentMana = 0;

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
        gameState = GameState.Instance;
    }

	private void Start()
	{
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
            choice.ChoiceMenu(lE, 1, WhichMethod.switchChampionPlayer, null);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState.hasPriority = true;
        }

    }

    public void UpdateUnspentMana()
    {
        unspentMana += currentMana;

    }

    public bool CheckIfCanPlayCard(CardDisplay cardDisplay, bool useMana)
    {
        cardCost = cardDisplay.manaCost;
        if (currentMana >= cardCost)
        {
            if (useMana)
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
        {
			hand = handPlayer;
            GameState.Instance.drawnCardsThisTurn += amountToDraw;
        }
		else
			hand = handOpponent;

		foreach (CardDisplay cardDisplay in hand.cardSlotsInHand)
		{
			if (cardDisplay.card != null) continue;

			if (!cardDisplay.gameObject.activeSelf)
			{
				if (drawnCards >= amountToDraw) break;

				if (!isPlayer)
				{
                    cardDisplay.SetBackfaceOnOpponentCards(backfaceCard);
				}

				if (specificCard == null)
					cardDisplay.card = Deck.Instance.WhichCardToDrawPlayer(isPlayer);
				else               
					cardDisplay.card = specificCard;


				if (cardDisplay.card != null)
				{
					cardDisplay.manaCost = cardDisplay.card.maxManaCost;
                    cardDisplay.gameObject.SetActive(true);
                    if (drawnCards == 0)
                    {
                        gameState.drawnCardsThisTurn -= 1;
						cardDisplay.firstCardDrawn = true;
                    }
					drawnCards++;
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
				Card c = Deck.Instance.WhichCardToDrawPlayer(isPlayer);
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
            

        int index = handPlayer.cardSlotsInHand.IndexOf(cardDisplay);
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
