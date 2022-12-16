using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<CardDisplay> cardSlotsInHand = new List<CardDisplay>();
    public List<CardDisplay> cardsInHand = new List<CardDisplay>();

    [SerializeField] private Vector3 playerHandStartPos;

    private void Start()
    {
        // Anledningen �r load order
        Invoke(nameof(InvokeRefresh), 0.1f);
    }
    
    private void InvokeRefresh()
    {
        GameState.Instance.Refresh();
        FixCardOrderInHand();
    }

    public void FixCardOrderInHand()
    {
        cardsInHand.Clear();
        if (!cardSlotsInHand[0].opponentCard)
            transform.position = playerHandStartPos;
        for (int i = 0; i < cardSlotsInHand.Count; i++)
        {
            CardDisplay cardDisplay = cardSlotsInHand[i];
           
            if (cardDisplay.card == null)
            {
                cardDisplay.HideUnusedCard();
                continue;
            }
   
            if (!cardsInHand.Contains(cardDisplay))
               cardsInHand.Add(cardDisplay);  
            
            cardDisplay.UpdateTextOnCard();

            if (!cardDisplay.opponentCard)
            {
                transform.position += new Vector3(-4f, 0, 0);
                cardDisplay.transform.localPosition = new Vector3(-1.75f + (i * 1.75f), -0.5f, -1.55f - (i * 0.05f));
            }
        }
    }


    public Card DiscardRandomCardInHand()
    {
        if (cardsInHand.Count <= 0)
            return null;
        int cardIndex = UnityEngine.Random.Range(0, cardsInHand.Count);
        return CardToDiscard(cardsInHand[cardIndex]);
    }

    public string DiscardSpecificCardWithIndex(int index)
    {
        return CardToDiscard(cardsInHand[index]).cardName;
    }

    public List<string> DiscardCardListWithIndexes(List<int> cardIndexes)
    {
        List<string> cards = new List<string>();
        for (int i = 0; i < cardIndexes.Count; i++)
        {
            for (int j = i + 1; j < cardIndexes.Count; j++)
            {
                if (cardIndexes[j] > cardIndexes[i])
                {
                    cardIndexes[j]--;
                }
            }
        }
        for (int i = 0; i < cardIndexes.Count; i++)
        {
            cards.Add(CardToDiscard(cardsInHand[cardIndexes[i]]).cardName);
        }      
        return cards;
    }

	public void FixMulligan(List<int> cardIndexes)
	{
		for (int i = 0; i < cardIndexes.Count; i++)
		{
			for (int j = i + 1; j < cardIndexes.Count; j++)
			{
				if (cardIndexes[j] > cardIndexes[i])
				{
					cardIndexes[j]--;
				}
			}
		}
        Deck deck = Deck.Instance;
        for (int i = 0; i < cardIndexes.Count; i++)
        {
            Card card = cardsInHand[cardIndexes[i]].card;

			ActionOfPlayer.Instance.ChangeCardOrder(true, cardsInHand[cardIndexes[i]]);
            deck.AddCardToDeckPlayer(card);
		}
        deck.ShuffleDeck();
        ActionOfPlayer.Instance.DrawCardPlayer(cardIndexes.Count, null, true);
	}

	private Card CardToDiscard(CardDisplay cardDisplay)
    {
        Graveyard.Instance.AddCardToGraveyard(cardDisplay.card);
        Card c = cardDisplay.card;

        ActionOfPlayer.Instance.ChangeCardOrder(true, cardDisplay);
        return c;
    }
}
