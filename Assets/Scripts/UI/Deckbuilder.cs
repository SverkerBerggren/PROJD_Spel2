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
	[SerializeField] private GameObject championButton;

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
        decklist = GetComponentInChildren<TMP_Text>();
        UpdateDeckList();
        register = CardRegister.Instance;
		setup.StartDeckbuilder();
		foreach (Champion champion in register.champRegister.Values)
		{
			MakeButtonsChampions(champion);
		}
        foreach (Card card in register.cardRegister.Values)
        {
            if (!card.championCard)    
            MakeButtonsCards(card);
        }
	}

	private void MakeButtonsCards(Card card)
    {
        GameObject gO = Instantiate(cardButton, buttonHolder.transform);
        CardDisplayAttributes cardDisplayAtributes = gO.transform.GetChild(0).GetComponent<CardDisplayAttributes>();

        cardDisplayAtributes.previewCard = true;
        cardDisplayAtributes.UpdateTextOnCardWithCard(card);

        gO.GetComponent<DeckbuilderCardButton>().card = card;

        

        cardButtons.Add(card, gO);
    }

    private void MakeButtonsChampions(Champion champion)
    {
        GameObject gO = Instantiate(championButton, buttonHolder.transform);
        gO.GetComponent<Image>().enabled = true;
        gO.GetComponent<Image>().sprite = champion.artwork;        
        gO.GetComponentInChildren<DeckbuilderChampionButton>().champion = champion;
        championsButtons.Add(champion, gO);
    }

    public void SortDeckBuilder(CardFilter cardFilter, CardFilter championFilter)
    {

        foreach (Champion champion in championsButtons.Keys)
        {

        }

	}

    public void UpdateDeckList()
    {
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
    }
}
