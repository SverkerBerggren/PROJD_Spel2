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
    }



    void Shuffle(List<Card> list)
    {
        int n = list.Count;
        while (n > 1) //Randomizes the list
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }


    private void Start()
    {
        while (deckOfCardsPlayer.Count < 30)
        {
            foreach (Card card in deckPlayer)
            {
                deckOfCardsPlayer.Push(card);
            }
        }
        Shuffle(deckPlayer);

        print(deckOfCardsPlayer.Count);

        while (deckOfCardsOpponent.Count < 30)
        {
            foreach (Card card in deckOpponent)
            {
                deckOfCardsOpponent.Push(card);
            }
        }
        Shuffle(deckOpponent);

        print(deckOfCardsOpponent.Count);
    }

	private void Update()
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



	public void AddCardToDeckPlayer(Card cardToAdd)
    {
        deckOfCardsPlayer.Push(cardToAdd);
    }

    public void AddCardToDeckOpponent(Card cardToAdd)
    {
        deckOfCardsOpponent.Push(cardToAdd);
    }

    public Card WhichCardToDrawPlayer()
    {
        if(deckOfCardsPlayer.Count > 0)
        return deckOfCardsPlayer.Pop();

        GameState.Instance.Defeat();
        return null;
    }

    public Card WhichCardToDrawOpponent()
    {
        if (deckOfCardsOpponent.Count > 0)
            return deckOfCardsOpponent.Pop();

        GameState.Instance.Victory();
        return null;
    }
}
