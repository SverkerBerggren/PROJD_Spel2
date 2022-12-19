using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.ConstrainedExecution;

public class CardRegister : MonoBehaviour
{

    [Header("Cards")]
    [SerializeField] private List<Card> cards = new List<Card>();
    public Dictionary<string, Card> cardRegister = new Dictionary<string, Card>();

	[Header("Effects")]
	[SerializeField] private List<Effects> effects = new List<Effects>();
    public Dictionary<string, Effects> effectRegister = new Dictionary<string, Effects>();

	[Header("Champions")]
	[SerializeField] private List<Champion> champions = new List<Champion>();
    public Dictionary<string, Champion> champRegister = new Dictionary<string, Champion>();
	public Dictionary<ChampionCardType, Champion> championTypeRegister = new Dictionary<ChampionCardType, Champion>();
	private Dictionary<Champion, List<Card>> champCards = new Dictionary<Champion, List<Card>>();

	[Header("Type of cards")]
	public Dictionary<string, Landmarks> landmarkRegister = new Dictionary<string, Landmarks>();
    public Dictionary<string, Card> attackCardRegister = new Dictionary<string, Card>();
    public Dictionary<string, Card> supportCardRegister = new Dictionary<string, Card>();
    // Start is called before the first frame update

    private static CardRegister instance;
    public static CardRegister Instance { get; set; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        #if UNITY_EDITOR  
            SpreadsheetUpdater updater = new SpreadsheetUpdater();
            Task tasket =  updater.UpdateCardReferences(cards);
        #endif

        InstantiateRegister();
        DontDestroyOnLoad(this);
    }


	private void InstantiateRegister()
    {
		cards.Sort(new CardComparer(CardFilter.ManaCost));
        champions.Sort(new ChampionComparer(CardFilter.Name));
        foreach (Card card in cards)
        {
			cardRegister.Add(card.cardName, card);

            switch (card.typeOfCard)
            {
                case CardType.Landmark:
                    landmarkRegister.Add(card.cardName, (Landmarks)card);
                    break;

                case CardType.Attack:
                    attackCardRegister.Add(card.cardName, card);
                    break;

                case CardType.Spell:
                    supportCardRegister.Add(card.cardName, card);
                    break;

            }
		}

		foreach (Effects effect in effects)
        {
            effectRegister.Add(effect.name, effect);
        }

        foreach (Champion champion in champions)
        {
            champRegister.Add(champion.championName, champion);
            champCards.Add(champion, AddChampionsCards(champion));
            AddChampionType(champion);
        }    
    }

    private List<Card> AddChampionsCards(Champion champion)
    {
        List<Card> cards = new List<Card>();
        foreach (Card card in cardRegister.Values)
        {
            if (card.championCard && card.championCardType == champion.championCardType)
            {
                cards.Add(card);
            }
        }
        return cards;
    }

    private void AddChampionType(Champion champion)
    {
		ChampionCardType type = ChampionCardType.None;
		switch (champion)
		{
			case Cultist:
				type = ChampionCardType.Cultist;
				break;

			case Duelist:
				type = ChampionCardType.Duelist;
				break;

			case Graverobber:
				type = ChampionCardType.Graverobber;
				break;

			case TheOneWhoDraws:
				type = ChampionCardType.TheOneWhoDraws;
				break;

			case Shanker:
				type = ChampionCardType.Shanker;
				break;

			case Builder:
				type = ChampionCardType.Builder;
				break;

			default:
				Debug.LogError("The Champion doesnt exist in the register");
				break;
		}
		championTypeRegister.Add(type, champion);
	}
    public List<Card> GetChampionCards(Champion champion)
    {
        List<Card> cards = champCards[champion];
		foreach (Card c in champCards[champion])
        {
            if (c.typeOfCard == CardType.Attack)
            {
                cards.Add(c);
                break;
            }
        }
        return cards;
    }
}
