using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update
    private int playerFatigue = 0;
    private TargetAndAmount fatigueTarget;
    [SerializeField] private int fatigueDamage = 10;

    public List<Card> DeckPlayer = new List<Card>();
    public List<Card> DeckOpponent = new List<Card>();

    private static Deck instance;
    public static Deck Instance { get { return instance; } }

    private void Awake()
    {   
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if(!GameState.Instance.IsOnline) // Creates decks for offline play
        {
            List<Card> copy = new List<Card>();
            copy.AddRange(DeckPlayer);
            while (DeckPlayer.Count < 40)
            {
                foreach (Card card in copy)
                {
                    DeckPlayer.Add(card);
                    if (DeckPlayer.Count >= 40) break;
                }
            }
            Shuffle(DeckPlayer);
            DeckOpponent.Clear();
            DeckOpponent.AddRange(DeckPlayer);
        }

		TargetInfo targetInfo = new TargetInfo();
        targetInfo.whichList.myChampions = true;
        targetInfo.index = 0;
        fatigueTarget = new TargetAndAmount(targetInfo, fatigueDamage);
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

    public void CreateDecks(List<Card> importedDeck) // Creates decks for online play
	{
        DeckPlayer.Clear();
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
        DeckPlayer.AddRange(importedDeck);
        Shuffle(DeckPlayer);

        DeckOpponent.Clear();
        DeckOpponent.AddRange(importedDeck);
    }

    public void ShuffleDeck()
    {
        Shuffle(DeckPlayer);
    }

    public void RemoveCardFromDeck(Card card)
    {
        DeckPlayer.Remove(card);
    }

    public void AddCardToDeckPlayer(Card cardToAdd)
    {
        DeckPlayer.Add(cardToAdd);
    }

    public void AddCardToDeckOpponent(Card cardToAdd)
    {
        DeckOpponent.Add(cardToAdd);
    }

    public Card WhichCardToDrawPlayer(bool isPlayer)
    {
        if (isPlayer)
        {
            if (DeckPlayer.Count > 0)
            {
                Card card = DeckPlayer[0];
                DeckPlayer.RemoveAt(0);
                return card;
            }
            else
            {
                GameState.Instance.DealDamage(fatigueTarget);
                fatigueTarget.amount *= 2;
            }
        }
        else
        {
            if (DeckOpponent.Count > 0)
            {
                Card card = DeckOpponent[0];
                DeckOpponent.RemoveAt(0);
                return card;
            }
        }
        return null;
    }
}
