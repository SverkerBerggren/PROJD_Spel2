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
    [SerializeField] private Button stopBuilding;
	[SerializeField] private TMP_InputField cardSearch;
	[SerializeField] private TMP_InputField deckNameField;
    [SerializeField] private List<Button> deckSlotButtons = new List<Button>();
    [SerializeField] private CardFilterButtons cardFilterButtons;

	[NonSerialized] public string deckName;
	[NonSerialized] public CardFilter cardFilter = CardFilter.ManaCost;
	[NonSerialized] public CardType cardTypeFilter = CardType.Attack;

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
        Dictionary<Card, Transform> cardObjects = new Dictionary<Card, Transform>();
		Dictionary<Champion, Transform> championObjects = new Dictionary<Champion, Transform>();
		foreach (Transform t in buttonHolder.GetComponentInChildren<Transform>(true))
        {
			DeckbuilderCardButton deckBuilderCard;
            if (t.TryGetComponent(out deckBuilderCard))
            {
                if (deckBuilderCard.card != null)
                    cardObjects.Add(deckBuilderCard.card, t);
                else if (deckBuilderCard.champion != null)
                    championObjects.Add(deckBuilderCard.champion, t);
			}
		}

        List<Card> cards = new List<Card>();
		List<Champion> champions = new List<Champion>();

        cards.AddRange(cardObjects.Keys);
		champions.AddRange(championObjects.Keys);

		cards.Sort(new CardComparer(cardFilter));
		champions.Sort(new ChampionComparer(cardFilter));

		int j = 0;
		for (int i = 0; i < champions.Count + cards.Count; i++)
        {
            Transform transform = null;
            Champion champion = null;
            Card card = null;

            if (i < champions.Count)
                champion = champions[i];
            else
                card = cards[i - champions.Count];

            if ((card != null && cardObjects.TryGetValue(card, out transform)) 
            || (champion != null && championObjects.TryGetValue(champion, out transform)))
            {
				transform.gameObject.SetActive(true);
				transform.SetSiblingIndex(j);
                j++;
            }
            
            if (!string.IsNullOrEmpty(cardSearch.text))
            {
                if ((card != null && !card.cardName.Contains(cardSearch.text, StringComparison.OrdinalIgnoreCase))
                || (champion != null && !champion.championName.Contains(cardSearch.text, StringComparison.OrdinalIgnoreCase)))
                {
					transform.gameObject.SetActive(false);
                    continue;
                }
            }

            if (cardFilterButtons.typeFilter)
            {
                if (champion != null || card.typeOfCard != cardTypeFilter)
                    transform.gameObject.SetActive(false);
            }
            else if (card != null && cardFilter == CardFilter.Health && card is not Landmarks)
            {
				transform.gameObject.SetActive(false);
			}
		}
	}

    public void UpdateDeckList()
    {
        if (setup == null)
        {
            setup = Setup.Instance;
        }

        decklist.text = "Deck: " + deckName + "\n\n";
        decklist.text += "Champions " + setup.myChampions.Count + "/3\n";
        foreach (string champion in setup.myChampions)
        {
            decklist.text += champion + "\n";
        }
        decklist.text += "\n";
        decklist.text += "Cards " + setup.currentDeckSize + "/" + setup.deckCount + "\n";
        foreach (Card card in setup.amountOfCards.Keys)
        {
            decklist.text += card.cardName + " x" + setup.amountOfCards[card] + "\n";
        }


        if (setup.currentDeckSize == setup.deckCount && setup.myChampions.Count == 3)
        {
            stopBuilding.interactable = true;
            saveDeck.interactable = false;
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
        {
            stopBuilding.interactable = false;
            saveDeck.interactable = false;
        }
    }
}
