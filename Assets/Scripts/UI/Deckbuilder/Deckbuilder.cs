using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class Deckbuilder : MonoBehaviour
{
    private Setup setup;
    private CardRegister register;
    [SerializeField] private Transform cardHolders;
    [SerializeField] private TMP_Text decklistText;

    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject cardButton;
    [SerializeField] private GameObject cardBannerPrefab;
    [SerializeField] private Button stopBuilding;
	[SerializeField] private CardFilterButtons cardFilterButtons;
	[SerializeField] private TMP_InputField cardSearch;
	[SerializeField] private TMP_InputField deckNameField;
    [SerializeField] private List<Button> deckSlotButtons = new();

    [NonSerialized] public Dictionary<Card, GameObject> CardBanners = new();
	[NonSerialized] public string DeckName;
	[NonSerialized] public CardFilter CardFilter = CardFilter.ManaCost;
	[NonSerialized] public CardType CardTypeFilter = CardType.Attack;
	[NonSerialized] public int MaxCardBanners = 50;

	public Button SaveDeckButton;

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
    }

    void Start()
    {
        setup = Setup.Instance;
        register = CardRegister.Instance;
        foreach (Champion champion in register.champRegister.Values) // Adds all cards and champions to the deckbuilder
        {
            MakeButtonOfChampion(champion);
        }
        foreach (Card card in register.cardRegister.Values)
        {
            if (!card.ChampionCard)
                MakeButtonOfCard(card);
        }
        deckNameField.characterLimit = 12;
        setup.StartDeckbuilder();
        UpdateDeckList();
        FixDeckButtons();
        FilterCards(CardFilter);
	}

    private void MakeButtonOfCard(Card card)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();
        choiceButton.CardPrefab.SetActive(true);
        CardDisplayAttributes cardDisplayAttributes = gO.GetComponentInChildren<CardDisplayAttributes>();
        cardDisplayAttributes.previewCard = true;
        cardDisplayAttributes.UpdateTextOnCardWithCard(card);
        gO.GetComponent<DeckbuilderCardButton>().Card = card;
    }

    private void MakeButtonOfChampion(Champion champion)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        ChoiceButton choiceButton = gO.GetComponent<ChoiceButton>();

        ChampionAttributes championAttributes = choiceButton.ChampionPrefab.GetComponent<ChampionAttributes>();
        championAttributes.UpdateChampionCard(champion);
        choiceButton.ChampionPrefab.SetActive(true);
        gO.GetComponent<DeckbuilderCardButton>().Champion = champion;
    }

    private void FixDeckButtons()
    {
        foreach (string filePath in Directory.GetFiles(Setup.savePath))
        {
            string deckFile = Path.GetFileName(filePath);

            if (!deckFile.EndsWith(".txt")) continue; // Goes through all files and removes non textfiles

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
        if (string.IsNullOrEmpty(deckNameField.text))
            deckNameField.text = "Deck";

        if (File.Exists(Setup.savePath + deckNameField.text + ".txt")) return;

		setup.SaveDeckToFile(deckNameField.text);
        AddDeckToButton(deckNameField.text);
    }

    public void SearchFilter()
    {
        FilterCards(CardFilter);
    }

    public void FilterCards(CardFilter newCardFilter)
    {
        CardFilter = newCardFilter;
        Dictionary<Card, Transform> cardObjects = new();
		Dictionary<Champion, Transform> championObjects = new();
		foreach (Transform t in buttonHolder.GetComponentInChildren<Transform>(true)) //Adds all buttons to the two dictonaries
        {
			if (t.TryGetComponent(out DeckbuilderCardButton deckBuilderCard))
			{
				if (deckBuilderCard.Card != null)
					cardObjects.Add(deckBuilderCard.Card, t);
				else if (deckBuilderCard.Champion != null)
					championObjects.Add(deckBuilderCard.Champion, t);
			}
		}

        List<Card> cards = new(AddCardsToFilter(cardObjects));
		List<Champion> champions = new(championObjects.Keys);

		cards.Sort(new CardComparer(CardFilter));
		champions.Sort(new ChampionComparer(CardFilter));

		int j = 0;
		for (int i = 0; i < champions.Count + cards.Count; i++) // Goes through the both dictionaries
        {
			Champion champion = null;
			Card card = null;

            if (i < champions.Count)
                champion = champions[i];
            else
                card = cards[i - champions.Count];

			if ((card != null && cardObjects.TryGetValue(card, out Transform transform))
			|| (champion != null && championObjects.TryGetValue(champion, out transform))) // If it is a card or champion
			{
				transform.gameObject.SetActive(true); // Move the card in the deckbuilder
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
		if (CardFilter == CardFilter.Health) // Removes all non landmarks when health filter is on
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
        if (!string.IsNullOrEmpty(cardSearch.text) && card != null && !card.CardName.Contains(cardSearch.text, StringComparison.OrdinalIgnoreCase))
            transform.gameObject.SetActive(false);

        else if (cardFilterButtons.TypeFilter && card.TypeOfCard != CardTypeFilter)
			transform.gameObject.SetActive(false);
	}

	private void FilterChampion(Champion champion, Transform transform)
	{
        if (!string.IsNullOrEmpty(cardSearch.text) && !champion.ChampionName.Contains(cardSearch.text, StringComparison.OrdinalIgnoreCase))
            transform.gameObject.SetActive(false);

        else if (cardFilterButtons.TypeFilter)
            transform.gameObject.SetActive(false);
	}

	public void UpdateDeckList() // Shows the whole decklist
    {
        if (setup == null)
            setup = Setup.Instance;

        decklistText.text = "Deck: " + DeckName + "\n\n";
        decklistText.text += "Champions " + setup.myChampions.Count + "/3\n";
        foreach (string champion in setup.myChampions) // WIP
        {
            decklistText.text += champion + "\n";
        }
        decklistText.text += "\n";
        decklistText.text += "Cards " + setup.currentDeckSize + "/" + setup.deckCount + "\n";

        List<Card> cards = new(setup.amountOfCards.Keys);
        cards.Sort(new CardComparer(CardFilter.ManaCost));
		for (int i = 0; i < cards.Count; i++)
        {
			CheckCardBanner(cards[i], i);
        }
        ChangeButtonInteractability();
    }

    private void ChangeButtonInteractability()
    {
        SaveDeckButton.interactable = false;
		if (setup.currentDeckSize == setup.deckCount && setup.myChampions.Count == 3)
		{
			stopBuilding.interactable = true;
			foreach (Button b in deckSlotButtons)
			{
				if (b.interactable == false)
				{
					SaveDeckButton.interactable = true;
					break;
				}
			}
		}
		else
			stopBuilding.interactable = false;
	}

    public void CheckCardBanner(Card card)
    {
        CheckCardBanner(card, -1); // Does not move position
    }

	private void CheckCardBanner(Card card, int index) // Changes the cardsbanner object
	{
        CardBanner cardBanner;
		if (CardBanners.ContainsKey(card))
			cardBanner = CardBanners[card].GetComponent<CardBanner>();
		else
		{
			GameObject banner = Instantiate(cardBannerPrefab, cardHolders);
            cardBanner = banner.GetComponent<CardBanner>();
            cardBanner.SetCard(card);
            CardBanners.Add(card, banner);
            List<Card> cards = new(CardBanners.Keys);
            if (cards.Count > MaxCardBanners)
                ClearOldBanners(cards);
		}
        cardBanner.SetValue(setup.amountOfCards[card]);

        if (index > -1)
            CardBanners[card].transform.SetSiblingIndex(index);
	}

    private void ClearOldBanners(List<Card> cards)
    {
		for (int i = 0; i < cards.Count; i++)
		{
			Card temp = cards[i];
			if (!CardBanners[temp].activeSelf)
			{
                Destroy(CardBanners[temp]);
				CardBanners.Remove(temp);
			}
		}
	}

    public void ClearAllBanners()
    {
        foreach (Card card in CardBanners.Keys)
        {
            Destroy(CardBanners[card]);
        }
        CardBanners.Clear();
    }
}
