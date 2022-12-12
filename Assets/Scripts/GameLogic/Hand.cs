using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<CardDisplay> cardSlotsInHand = new List<CardDisplay>();
    public Deck deck;
    public List<CardDisplay> cardsInHand = new List<CardDisplay>();

    private void Start()
    {
        // Anledningen är load order
        Invoke(nameof(InvokeRefresh), 0.1f);
    }
    
    private void InvokeRefresh()
    {
        GameState.Instance.Refresh();
    }

    public void FixCardOrderInHand()
    {
        cardsInHand.Clear();
        foreach (CardDisplay cardDisplay in cardSlotsInHand)
        {
            if (cardDisplay.card == null) continue;          
   
            if (!cardsInHand.Contains(cardDisplay))
               cardsInHand.Add(cardDisplay);  
            
            cardDisplay.UpdateTextOnCard();
		}
    }

    public Card DiscardRandomCardInHand()
    {
        int cardIndex = Random.Range(0, cardsInHand.Count);
        return CardToDiscard(cardsInHand[cardIndex]);
    }

    public string DiscardSpecificCardWithIndex(int index)
    {
        return CardToDiscard(cardsInHand[index]).cardName;
    }

    private Card CardToDiscard(CardDisplay cardDisplay)
    {
        Graveyard.Instance.AddCardToGraveyard(cardDisplay.card);
        Card c = cardDisplay.card;
        cardDisplay.card = null;
        return c;
    }
}
