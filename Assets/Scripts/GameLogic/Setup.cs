using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build;
using UnityEngine;

public class Setup : MonoBehaviour
{
	private CardRegister cardRegister;
    private Deckbuilder deckbuilder;
    private string savePath;
    public List<Card> playerDeckList = new List<Card>();
    [System.NonSerialized] public List<string> opponentChampions = new List<string>();

	[SerializeField] private int maxCopies = 3;
    public List<string> myChampions = new List<string>();
    public Dictionary<Card, int> amountOfCards = new Dictionary<Card, int>();
	public int deckCount = 40;
    public int currentDeckSize = 0;

    public bool shouldStartGame = false;

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
        savePath = Application.dataPath + "/SavedDecks/";
		cardRegister = CardRegister.Instance;
	}

    // Start is called before the first frame update
    void Start()
    {
        deckbuilder = Deckbuilder.Instance;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDeckbuilder()
    {
		deckbuilder = Deckbuilder.Instance;
		/*
		foreach (string name in myChampions)
        {
            Champion champion = cardRegister.champRegister[name];
            AddCards(cardRegister.GetChampionCards(champion));
        }

        foreach (Card card in playerDeckList)
        {
            AddCard(card);
        }
        */
		LoadDeck("Basic");
		deckbuilder.UpdateDeckList();
	}

	public void StopDeckBuilder()
	{
        playerDeckList.Clear();
        List<Tuple<string, int>> cardNames = new List<Tuple<string, int>>();
        foreach (Card card in amountOfCards.Keys)
        {
            for (int i = 0; i < amountOfCards[card]; i++)
            {
                playerDeckList.Add(card);
            }
            cardNames.Add(Tuple.Create(card.cardName, amountOfCards[card]));
        }
        SaveDeck("Basic");
	}

    private void SaveDeck(string deckName)
    {
        List<string> cards = new List<string>();
        foreach (Card c in amountOfCards.Keys)
        {
            cards.Add(c.cardName + "|" + amountOfCards[c]);
        }
		SavedDeck savedDeck = new SavedDeck
		{
			name = deckName,
			champions = myChampions,
			cards = cards,
		};
		string json = JsonUtility.ToJson(savedDeck);
        File.WriteAllText(savePath + deckName + ".txt", json);
		UnityEngine.Debug.Log("Deck saved");
	}

    private void LoadDeck(string deckName)
    {
        if (!File.Exists(savePath + deckName + ".txt"))
        {
			UnityEngine.Debug.LogError("Couldnt load deck");
            return;
        }

        try
        {
            string readDeck = File.ReadAllText(savePath + deckName + ".txt");
            SavedDeck loadedDeck = JsonUtility.FromJson<SavedDeck>(readDeck);
            myChampions.Clear();
            playerDeckList.Clear();
            int cardsCount = 0;

            foreach (string s in loadedDeck.cards)
            {
                string[] split = s.Split("|");

                if (int.Parse(split[1]) > maxCopies || int.Parse(split[1]) < 1) throw new InvalidDataException();

                cardsCount += int.Parse(split[1]);
                amountOfCards.Add(cardRegister.cardRegister[split[0]], int.Parse(split[1]));
            }

            if (cardsCount != deckCount && myChampions.Count != 3) throw new InvalidDataException();

            foreach (string s in loadedDeck.champions)
            {
                myChampions.Add(s);
                Champion c = cardRegister.champRegister[s];
                List<Card> champCards = new List<Card>(cardRegister.GetChampionCards(c));
				for (int i = 0; i < champCards.Count; i++)
                {
                    if (amountOfCards.ContainsKey(champCards[i]))
                    { 
                        champCards.Remove(champCards[i]);
                        i--;
                    }
                }
                if (champCards.Count != 0) throw new InvalidDataException();
			}
			currentDeckSize = deckCount;
        }
        catch (Exception e)
        {
			ClearDeck();
			UnityEngine.Debug.LogError(e.ToString());
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
        if (myChampions.Contains(champion.championName))
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
        if (currentDeckSize + 1 <= deckCount)
        {
            if (amountOfCards.ContainsKey(card))
			{
                if (amountOfCards[card] < maxCopies)
                {
				    amountOfCards[card]++;
				    currentDeckSize++;
                }
			}
			else
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

public class SavedDeck
{
    public string name;
    public List<string> champions;
    public List<string> cards;
}
