using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public Stack<Card> deckOfCardsPlayer = new Stack<Card>();
    [SerializeField] public Stack<Card> deckOfCardsOpponent = new Stack<Card>();

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
            Shuffle(deckPlayer);
            while (deckOfCardsPlayer.Count < 30)
            {
                foreach (Card card in deckPlayer)
                {
                    deckOfCardsPlayer.Push(card);
                }
            }

            Shuffle(deckOpponent);
            while (deckOfCardsOpponent.Count < 30)
            {
                foreach (Card card in deckOpponent)
                {
                    deckOfCardsOpponent.Push(card);
                }
            }
            UpdateDecks();
        }        
    }

    public void CreateDecks(List<Card> playerDeck)
    {
        deckOfCardsPlayer.Clear();
        deckPlayer.Clear();

        Shuffle(playerDeck);
        while (deckOfCardsPlayer.Count < 30)
        {
            foreach (Card card in playerDeck)
            {
                deckOfCardsPlayer.Push(card);

                if(deckOfCardsPlayer.Count >= 30) break;               
            }
            if (deckOfCardsPlayer.Count >= 30) break;
        }
        Shuffle(deckOpponent);
        while (deckOfCardsOpponent.Count < 30)
        {
            foreach (Card card in deckOpponent)
            {
                deckOfCardsOpponent.Push(card);
                if (deckOfCardsOpponent.Count >= 30) break;
            }
             if(deckOfCardsOpponent.Count >= 30) break;
        }

        UpdateDecks();
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

    public void AddCardToDeckPlayer(Card cardToAdd)
    {
        deckOfCardsPlayer.Push(cardToAdd);
        deckPlayer.Add(cardToAdd);
    }

    public void AddCardToDeckOpponent(Card cardToAdd)
    {
        deckOfCardsOpponent.Push(cardToAdd);
        deckOpponent.Add(cardToAdd);
    }

    public Card WhichCardToDrawPlayer()
    {
        if (deckOfCardsPlayer.Count > 0)
        {
            Card c = deckOfCardsPlayer.Pop();
            deckOpponent.Remove(c);
            return c;
        }

        GameState.Instance.Defeat();
        return null;
    }

    public Card WhichCardToDrawOpponent()
    {
        if (deckOfCardsOpponent.Count > 0)
        {
            Card c = deckOfCardsOpponent.Pop();
            deckPlayer.Remove(c);
            return c;
        }

        GameState.Instance.Victory();
        return null;
    }

    private void UpdateDecks()
    {
        deckPlayer.Clear();
        foreach (Card c in deckOfCardsPlayer)
        {
            deckPlayer.Add(c);
        }

        deckOpponent.Clear();
        foreach (Card c in deckOfCardsOpponent)
        {
            deckOpponent.Add(c);
        }
    }
}
