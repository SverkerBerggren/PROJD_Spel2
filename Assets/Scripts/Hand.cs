using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<GameObject> cardSlotsInHand = new List<GameObject>();
    public Deck deck;
    public List<GameObject> cardsInHand = new List<GameObject>();

    private void FixedUpdate()
    {


    }

    public void FixCardOrderInHand()
    {
        foreach (GameObject cardSlot in cardSlotsInHand)
        {
            CardDisplay cardDisplay = cardSlot.GetComponent<CardDisplay>();
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
        Graveyard.Instance.AddCardToGraveyard(cardDisplay.card);
        Card c = cardDisplay.card;
        cardDisplay.card = null;
        return c;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
