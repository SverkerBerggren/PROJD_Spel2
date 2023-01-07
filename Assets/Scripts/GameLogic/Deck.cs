using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Card> deckPlayer = new List<Card>();
    public List<Card> deckOpponent = new List<Card>();


    private static Deck instance;
    public static Deck Instance { get { return instance; } }

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

        if(!GameState.Instance.isOnline)
        {
            List<Card> copy = new List<Card>();
            copy.AddRange(deckPlayer);
            while (deckPlayer.Count < 40)
            {
                foreach (Card card in copy)
                {
                    deckPlayer.Add(card);
                    if (deckPlayer.Count >= 40) break;
                }
            }
            Shuffle(deckPlayer);
            deckOpponent.Clear();
            deckOpponent.AddRange(deckPlayer);
        }        
    }

    public void CreateDecks(List<Card> importedDeck)
    {
        deckPlayer.Clear();
        List<Card> copy = new List<Card>();
        copy.AddRange(importedDeck);
        while (importedDeck.Count < 40)
        {
            foreach (Card card in copy)
            {
                importedDeck.Add(card);

                if(importedDeck.Count >= 40) break;               
            }
        }
        deckPlayer.AddRange(importedDeck);
        Shuffle(deckPlayer);

        deckOpponent.Clear();
        deckOpponent.AddRange(importedDeck);
    }

    public void ShuffleDeck()
    {
        Shuffle(deckPlayer);
    }

	private static void Shuffle(List<Card> list)
    {
        int n = list.Count;
        while (n > 1) //Randomizes the list
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public void RemoveCardFromDeck(Card card)
    {
        deckPlayer.Remove(card);
    }

    public void AddCardToDeckPlayer(Card cardToAdd)
    {
        deckPlayer.Add(cardToAdd);
    }

    public void AddCardToDeckOpponent(Card cardToAdd)
    {
        deckOpponent.Add(cardToAdd);
    }

    public Card WhichCardToDrawPlayer(bool isPlayer)
    {
        if (isPlayer)
        {
            if (deckPlayer.Count > 0)
            {
                Card card = deckPlayer[0];
                deckPlayer.RemoveAt(0);
                return card;
            }
        }
        else
        {
            if (deckOpponent.Count > 0)
            {
                Card card = deckOpponent[0];
                deckOpponent.RemoveAt(0);
                return card;
            }
        }
        

        //GameState.Instance.Defeat();
        return null;
    }
}
