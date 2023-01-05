using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenCover.Framework.Model;
using System.IO;
using System;
using System.Globalization;
using System.Reflection;

public class Deckbuilder : MonoBehaviour
{
    private Setup setup;
    private CardRegister register;
    private TMP_Text decklist;


    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject cardButton;
    [SerializeField] private GameObject cardBannerPrefab;
    [SerializeField] private Button stopBuilding;
	[SerializeField] private CardFilterButtons cardFilterButtons;
	[SerializeField] private TMP_InputField cardSearch;
	[SerializeField] private TMP_InputField deckNameField;
    [SerializeField] private List<Button> deckSlotButtons = new();

    [NonSerialized] public Dictionary<Card, GameObject> cardBanners = new();
	[NonSerialized] public string deckName;
	[NonSerialized] public CardFilter cardFilter = CardFilter.ManaCost;
	[NonSerialized] public CardType cardTypeFilter = CardType.Attack;
	[NonSerialized] public int maxCardBanners = 50;

	public Button saveDeck;

    private static Deckbuilder instance;
    public static Deckbuilder Instance { get { return instance; } set { instance = value; } }
    // Start is called before the first frame update

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
        decklist = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        setup = Setup.Instance;
        register = CardRegister.Instance;
        foreach (Champion champion in register.champRegister.Values)
        {
            MakeButtonOfChampion(champion);
        }
        foreach (Card card in register.cardRegister.Values)
        {
            if (!card.championCard)
                MakeButtonOfCard(card);
        }
        setup.StartDeckbuilder();
        UpdateDeckList();
        deckNameField.characterLimit = 12;
        FixDeckButtons();
        FilterCards(cardFilter);
	}

    private void MakeButtonOfCard(Card card)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();
        choiceButton.cardPrefab.SetActive(true);
        CardDisplayAttributes cardDisplayAttributes = gO.GetComponentInChildren<CardDisplayAttributes>();
        cardDisplayAttributes.previewCard = true;
        cardDisplayAttributes.UpdateTextOnCardWithCard(card);
        gO.GetComponent<DeckbuilderCardButton>().card = card;
    }

    private void MakeButtonOfChampion(Champion champion)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();

        ChampionAttributes championAttributes = choiceButton.championPrefab.GetComponent<ChampionAttributes>();
        championAttributes.UpdateChampionCard(champion);
        choiceButton.championPrefab.SetActive(true);
        gO.GetComponent<DeckbuilderCardButton>().champion = champion;
    }

    private void FixDeckButtons()
    {
        foreach (string filePath in Directory.GetFiles(Setup.savePath))
        {
            string deckFile = Path.GetFileName(filePath);
            if (!deckFile.EndsWith(".txt")) continue;

            deckFile = deckFile.Remove(deckFile.Length - 4, 4);
            if (setup.LoadDeckToFile(deckFile))
                AddDeckToButton(deckFile);
        }
        setup.ClearDeck();
    }

    public void AddDeckToButton(string deckName)
    {
        for (int i = 0; i < deckSlotButtons.Count; i++)
        {
            Button button = deckSlotButtons[i];
            if (!button.interactable)
            {
                button.GetComponent<DeckSlot>().SetDeck(deckName);
                UpdateDeckList();
                return;
            }
        }
        Debug.LogError("Too many decks");
    }

    public void SaveDeck()
    {
        if (string.IsNullOrEmpty(deckName))
            deckNameField.text = "Deck";

        setup.SaveDeckToFile(deckNameField.text);
        AddDeckToButton(deckNameField.text);
    }

    public void SearchFilter()
    {
        FilterCards(cardFilter);
    }

    public void FilterCards(CardFilter newCardFilter)
    {
        cardFilter = newCardFilter;
        Dictionary<Card, Transform> cardObjects = new();
		Dictionary<Champion, Transform> championObjects = new();
		foreach (Transform t in buttonHolder.GetComponentInChildren<Transform>(true))
        {
			if (t.TryGetComponent(out DeckbuilderCardButton deckBuilderCard))
			{
				if (deckBuilderCard.card != null)
					cardObjects.Add(deckBuilderCard.card, t);
				else if (deckBuilderCard.champion != null)
					championObjects.Add(deckBuilderCard.champion, t);
			}
		}

        List<Card> cards = new(AddCardsToFilter(cardObjects));
		List<Champion> champions = new(championObjects.Keys);

		cards.Sort(new CardComparer(cardFilter));
		champions.Sort(new ChampionComparer(cardFilter));

		int j = 0;
		for (int i = 0; i < champions.Count + cards.Count; i++)
        {
			Champion champion = null;
			Card card = null;

            if (i < champions.Count)
                champion = champions[i];
            else
                card = cards[i - champions.Count];

			if ((card != null && cardObjects.TryGetValue(card, out Transform transform))
			|| (champion != null && championObjects.TryGetValue(champion, out transform)))
			{
				transform.gameObject.SetActive(true);
				transform.SetSiblingIndex(j);
				j++;
			}
			else continue;

			if (champion == null)
                FilterCard(card, cardObjects[card]);
            else
                FilterChampion(champion, championObjects[champion]);
		}
	}

    private List<Card> AddCardsToFilter(Dictionary<Card, Transform> cardObjects)
    {
        List<Card> cards = new(cardObjects.Keys);
		if (cardFilter == CardFilter.Health)
		{
			for (int i = 0; i < cards.Count; i++)
			{
				Card c = cards[i];
				if (c is not Landmarks)
				{
					cardObjects[c].gameObject.SetActive(false);
					cards.Remove(c);
					i--;
				}
			}
		}
        return cards;
	}

    private void FilterCard(Card card, Transform transform)
    {
        if (!string.IsNullOrEmpty(cardSearch.text) && card != null && !card.cardName.Contains(cardSearch.text, StringComparison.OrdinalIgnoreCase))
            transform.gameObject.SetActive(false);

        else if (cardFilterButtons.typeFilter && card.typeOfCard != cardTypeFilter)
			transform.gameObject.SetActive(false);
	}

	private void FilterChampion(Champion champion, Transform transform)
	{
        if (!string.IsNullOrEmpty(cardSearch.text) && !champion.championName.Contains(cardSearch.text, StringComparison.OrdinalIgnoreCase))
            transform.gameObject.SetActive(false);

        else if (cardFilterButtons.typeFilter)
            transform.gameObject.SetActive(false);
	}

	public void UpdateDeckList()
    {
        if (setup == null)
            setup = Setup.Instance;

        decklist.text = "Deck: " + deckName + "\n\n";
        decklist.text += "Champions " + setup.myChampions.Count + "/3\n";
        foreach (string champion in setup.myChampions)
        {
            decklist.text += champion + "\n";
        }
        decklist.text += "\n";
        decklist.text += "Cards " + setup.currentDeckSize + "/" + setup.deckCount + "\n";
        List<Card> cards = new(setup.amountOfCards.Keys);
        cards.Sort(new CardComparer(CardFilter.ManaCost));
		for (int i = 0; i < cards.Count; i++)
        {
			CheckCardBanner(cards[i], i);
        }

        saveDeck.interactable = false;

        if (setup.currentDeckSize == setup.deckCount && setup.myChampions.Count == 3)
        {
            stopBuilding.interactable = true;
            foreach (Button b in deckSlotButtons)
            {
                if (b.interactable == false)
                {
                    saveDeck.interactable = true;
                    break;
                }
            }
        }
        else
            stopBuilding.interactable = false;
    }

    public void CheckCardBanner(Card card)
    {
        CheckCardBanner(card, -1);
    }

	private void CheckCardBanner(Card card, int index)
	{
        CardBanner cardBanner;
		if (cardBanners.ContainsKey(card))
			cardBanner = cardBanners[card].GetComponent<CardBanner>();
		else
		{
			GameObject banner = Instantiate(cardBannerPrefab, decklist.transform);
            cardBanner = banner.GetComponent<CardBanner>();
            cardBanner.SetCard(card);
            cardBanners.Add(card, banner);
            List<Card> cards = new(cardBanners.Keys);
            if (cards.Count > maxCardBanners)
                ClearOldBanners(cards);
		}
        cardBanner.SetValue(setup.amountOfCards[card]);

        if (index > -1)
            cardBanners[card].transform.SetSiblingIndex(index);
	}

    private void ClearOldBanners(List<Card> cards)
    {
		for (int i = 0; i < cards.Count; i++)
		{
			Card temp = cards[i];
			if (!cardBanners[temp].activeSelf)
			{
                Destroy(cardBanners[temp]);
				cardBanners.Remove(temp);
			}
		}
	}

    public void ClearAllBanners()
    {
        foreach (Card card in cardBanners.Keys)
        {
            Destroy(cardBanners[card]);
        }
        cardBanners.Clear();
    }
}
