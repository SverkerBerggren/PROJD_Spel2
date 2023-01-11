using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
	private CardRegister cardRegister;
    private Deckbuilder deckbuilder;

	[SerializeField] private int maxCopies = 3;

	[System.NonSerialized] public static string savePath;
    [System.NonSerialized] public List<string> opponentChampions = new List<string>();
	[System.NonSerialized] public bool shouldStartGame = false;

    public List<Card> playerDeckList = new List<Card>();
    public List<string> myChampions = new List<string>();
    public Dictionary<Card, int> amountOfCards = new Dictionary<Card, int>();
	public int deckCount = 40;
    public int currentDeckSize = 0;


	private static Setup instance;
    public static Setup Instance { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(Instance);

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
        playerDeckList.Clear();
	}

    public void StopDeckbuilding()
    {
        foreach (Card card in amountOfCards.Keys)
        {
            for (int i = 0; i < amountOfCards[card]; i++)
            {
                playerDeckList.Add(card);
            }
        }
    }

    public void SaveDeckToFile(string deckName)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        List<string> cards = new List<string>();
        foreach (Card c in amountOfCards.Keys)
        {
            cards.Add(c.CardName + "|" + amountOfCards[c]);
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

    public bool LoadDeckToFile(string deckName)
    {
        if (string.IsNullOrEmpty(deckName) || !File.Exists(savePath + deckName + ".txt"))
        {
			UnityEngine.Debug.LogError("Couldnt load deck " + deckName);
            return false;
        }
        try
        {
            string readDeck = File.ReadAllText(savePath + deckName + ".txt"); // The whole method checks if the deck can be used for play
            SavedDeck loadedDeck = JsonUtility.FromJson<SavedDeck>(readDeck);
			List<Card> champCardsIncluded = new List<Card>();
			ClearDeck();

			deckbuilder.DeckName = loadedDeck.name;
			foreach (string championName in loadedDeck.champions) // Checks how many champions
            {
                myChampions.Add(championName);
                Champion champion = cardRegister.champRegister[championName];
                champCardsIncluded.AddRange(cardRegister.champCards[champion]);
			}

            FindCardName(loadedDeck, champCardsIncluded);

            // The last check with deckcount, and champion cards
            if (currentDeckSize != deckCount || myChampions.Count != 3 || champCardsIncluded.Count != 0) throw new InvalidDataException();
		}
        catch (Exception e)
        {
			ClearDeck(); // Clears deck when there is a error
			UnityEngine.Debug.LogError("Not allowed deck, clearing! \n" + e.ToString());
			return false;
		}
        return true;
    }

    private void FindCardName(SavedDeck loadedDeck, List<Card> champCardsIncluded)
    {
		foreach (string cardName in loadedDeck.cards) // Checks cards
		{
	        string[] split = cardName.Split("|");

            if (!cardRegister.cardRegister.ContainsKey(split[0])) throw new InvalidDataException();

			Card card = cardRegister.cardRegister[split[0]];
			int cardAmount = int.Parse(split[1]);

	        if (cardAmount > maxCopies || cardAmount < 1) throw new InvalidDataException();

	        amountOfCards.Add(card, cardAmount);
	        currentDeckSize += cardAmount;

	        if (card.ChampionCard) // Checks championcards
	        {
		        if (champCardsIncluded.Contains(card))
		        {
			        int removedAmount = champCardsIncluded.RemoveAll(x => x == card);
			        if (removedAmount != cardAmount) throw new InvalidDataException();
		        }
	        }
		}
	}

	public void AddChampion(Champion champion)
    {
        // Checks if the champion can be in the deck, plus the cards attached
        if (!myChampions.Contains(champion.ChampionName) && myChampions.Count < 3 && currentDeckSize + cardRegister.champCards[champion].Count <= deckCount)
        {
            myChampions.Add(champion.ChampionName);
            AddCards(cardRegister.champCards[champion]);
            deckbuilder.UpdateDeckList();
        }
    }

    public void RemoveChampion(Champion champion)
    {
        if (myChampions.Contains(champion.ChampionName))
        {
		    myChampions.Remove(champion.ChampionName);
            RemoveCards(cardRegister.champCards[champion]);
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
                deckbuilder.CheckCardBanner(card);
				amountOfCards.Remove(card);
			}
            deckbuilder.UpdateDeckList();
		}
    }

    public void ClearDeck()
    {
        amountOfCards.Clear();
        myChampions.Clear();
        deckbuilder.ClearAllBanners();
        currentDeckSize = 0;
        deckbuilder.DeckName = "";
    	deckbuilder.UpdateDeckList();
    }
}

public class SavedDeck // Class with saveable information
{
    public string name;
    public List<string> champions;
    public List<string> cards;
}
