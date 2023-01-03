using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenCover.Framework.Model;
using System.IO;
using System;

public class Deckbuilder : MonoBehaviour
{
    private Setup setup;
    private CardRegister register;
    private Dictionary<Card, GameObject> cardButtons = new Dictionary<Card, GameObject>();
    private Dictionary<Champion, GameObject> championsButtons = new Dictionary<Champion, GameObject>();
    private TMP_Text decklist;
    [NonSerialized] public string deckName;

    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject cardButton;
    [SerializeField] private Button stopBuilding;
    [SerializeField] private TMP_InputField deckNameField;
    [SerializeField] private List<Button> deckSlotButtons = new List<Button>();

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
