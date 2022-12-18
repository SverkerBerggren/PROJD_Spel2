using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    private CardRegister cardRegister;
    private Deckbuilder deckbuilder;
    public List<Card> playerDeckList = new List<Card>();
    [System.NonSerialized] public List<string> opponentChampions = new List<string>();

	[SerializeField] private int maxCopies = 3;
    public List<string> myChampions = new List<string>();
    public Dictionary<Card, int> amountOfCards = new Dictionary<Card, int>();
	public int deckCount = 40;
    public int currentDeckSize = 0;


	private static Setup instance;
    public static Setup Instance { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        deckbuilder = Deckbuilder.Instance;
        cardRegister = CardRegister.Instance;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDeckbuilder()
    {
		deckbuilder = Deckbuilder.Instance;
		foreach (string name in myChampions)
        {
            Champion champion = cardRegister.champRegister[name];
            AddCards(cardRegister.GetChampionCards(champion));
        }

        foreach (Card card in playerDeckList)
        {
            AddCard(card);
        }
		deckbuilder.UpdateDeckList();
	}

	public void StopDeckBuilder()
	{
        playerDeckList.Clear();
        foreach (Card card in amountOfCards.Keys)
        {
            for (int i = 0; i < amountOfCards[card]; i++)
            {
                playerDeckList.Add(card);
            }
        }
	}

	public void AddChampion(Champion champion)
    {
        if (!myChampions.Contains(champion.championName) && myChampions.Count < 3 && currentDeckSize + cardRegister.GetChampionCards(champion).Count <= deckCount)
        {
            myChampions.Add(champion.championName);
            AddCards(cardRegister.GetChampionCards(champion));
            deckbuilder.UpdateDeckList();
        }
    }

    public void RemoveChampion(Champion champion)
    {
        if (!myChampions.Contains(champion.championName))
        {
		    myChampions.Remove(champion.championName);
            RemoveCards(cardRegister.GetChampionCards(champion));
            deckbuilder.UpdateDeckList();
        }
    }

    public void AddCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            AddCard(card);
        }
    }

	public void RemoveCards(List<Card> cards)
	{
		foreach (Card card in cards)
		{
			RemoveCard(card);
		}
	}

	public void AddCard(Card card)
    {
        if (currentDeckSize + 1 < deckCount)
        {
            if (amountOfCards.ContainsKey(card) && amountOfCards[card] < maxCopies)
			{
				amountOfCards[card]++;
				currentDeckSize++;
			}
			else if (!amountOfCards.ContainsKey(card))
			{
				amountOfCards.Add(card, 1);
			    currentDeckSize++;
			}
			deckbuilder.UpdateDeckList();
        }
    }

    public void RemoveCard(Card card)
    {
		if (amountOfCards.ContainsKey(card))
		{
            amountOfCards[card]--;
            currentDeckSize--;
            if (amountOfCards[card] <= 0)
			{
				amountOfCards.Remove(card);
			}
            deckbuilder.UpdateDeckList();
		}
    }

    public void ClearDeck()
    {
        amountOfCards.Clear();
        playerDeckList.Clear();
        myChampions.Clear();
        currentDeckSize = 0;

        deckbuilder.UpdateDeckList();
    }
}
