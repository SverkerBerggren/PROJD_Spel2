using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<GameObject> cardSlotsInHand = new List<GameObject>();
    public Deck deck;
    public List<GameObject> cardsInHand = new List<GameObject>();

    private void Start()
    {
        // Anledningen är load order
        Invoke(nameof(InvokeRefresh), 0.1f);
    }
    
    private void InvokeRefresh()
    {
        GameState.Instance.Refresh();
    }


    private void FixedUpdate()
    {


    }

    public void FixCardOrderInHand()
    {
        cardsInHand.Clear();
        foreach (GameObject cardSlot in cardSlotsInHand)
        {
            CardDisplay cardDisplay = cardSlot.GetComponent<CardDisplay>();
            cardDisplay.UpdateTextOnCard();

            if (cardDisplay.card != null)
            {
                if (!cardsInHand.Contains(cardSlot))
                    cardsInHand.Add(cardSlot);
            }
            else
            {
                cardSlot.SetActive(false);
                if (cardsInHand.Contains(cardSlot))
                    cardsInHand.Remove(cardSlot);
            }

        }
    }

    public Card DiscardRandomCardInHand()
    {
        int cardIndex = Random.Range(0, cardsInHand.Count);
        CardDisplay cardDisplay = cardsInHand[cardIndex].GetComponent<CardDisplay>();
        return CardToDiscard(cardDisplay);
    }

    public string DiscardSpecificCardWithIndex(int index)
    {
        CardDisplay cardDisplay = cardsInHand[index].GetComponent<CardDisplay>();
        return CardToDiscard(cardDisplay).cardName;
    }

    private Card CardToDiscard(CardDisplay cardDisplay)
    {
        Graveyard.Instance.AddCardToGraveyard(cardDisplay.card);
        Card c = cardDisplay.card;
        cardDisplay.card = null;
        return c;
    }
}
