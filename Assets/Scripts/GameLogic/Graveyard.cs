using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graveyard : MonoBehaviour
{
    public List<Card> GraveyardPlayer = new List<Card>();
    public List<Card> GraveyardOpponent = new List<Card>();

    private static Graveyard instance;
    public static Graveyard Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCardToGraveyard(Card cardToAdd)
    {
        GraveyardPlayer.Add(cardToAdd);
    }

    public Tuple<Card,int> RandomizeCardFromGraveyard()
    {
        if (GraveyardPlayer.Count <= 0) return null;

        int index = UnityEngine.Random.Range(0, GraveyardPlayer.Count);
        return new Tuple<Card, int>(FindAndRemoveCardInGraveyard(GraveyardPlayer[index]), index);
    }

    public Card FindAndRemoveCardInGraveyard(Card cardToReturn)
    {
        if (GraveyardPlayer.Contains(cardToReturn))
        {
            GraveyardPlayer.Remove(cardToReturn);
            return cardToReturn;
        }
        return null;
    }

    public void AddCardToGraveyardOpponent(Card cardToAdd)
    {
        GraveyardOpponent.Add(cardToAdd);
    }

    public Card RandomizeCardFromOpponent()
    {
        return FindAndRemoveCardFromOpponent(GraveyardOpponent[UnityEngine.Random.Range(0, GraveyardOpponent.Count)]);
    }

    public Card FindAndRemoveCardFromOpponent(Card cardToReturn)
    {
        foreach (Card card in GraveyardOpponent)
        {
            if (card == cardToReturn)
            {
                GraveyardOpponent.Remove(card);
                return card;
            }
        }
        return null;
    }
}
