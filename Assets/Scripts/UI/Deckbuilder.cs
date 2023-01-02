using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deckbuilder : MonoBehaviour
{
    private Setup setup;
    private CardRegister register;
    private Dictionary<Card, GameObject> cardButtons = new Dictionary<Card, GameObject>();
	private Dictionary<Champion, GameObject> championsButtons = new Dictionary<Champion, GameObject>();
	private TMP_Text decklist;
    [SerializeField] private GameObject buttonHolder;
	[SerializeField] private GameObject cardButton;
	[SerializeField] private Button stopBuilding;

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

    public void SortDeckBuilder(CardFilter cardFilter, CardFilter championFilter)
    {

        foreach (Champion champion in championsButtons.Keys)
        {

        }

	}

    public void UpdateDeckList()
    {
        if (setup == null)
        {
            setup = Setup.Instance;
        }

        decklist.text = "Decklist\n\n";
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
			stopBuilding.interactable = true;
        else
			stopBuilding.interactable = false;
	}
}
