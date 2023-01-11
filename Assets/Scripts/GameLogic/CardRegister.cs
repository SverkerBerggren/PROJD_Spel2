using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
	public Dictionary<Champion, List<Card>> champCards = new Dictionary<Champion, List<Card>>();

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
            Instance = this;
        else
            Destroy(Instance);

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
			cardRegister.Add(card.CardName, card);

            switch (card.TypeOfCard)
            {
                case CardType.Landmark:
                    landmarkRegister.Add(card.CardName, (Landmarks)card);
                    break;

                case CardType.Attack:
                    attackCardRegister.Add(card.CardName, card);
                    break;

                case CardType.Spell:
                    supportCardRegister.Add(card.CardName, card);
                    break;

            }
		}

		foreach (Effects effect in effects)
        {
            effectRegister.Add(effect.name, effect);
        }

        foreach (Champion champion in champions)
        {
			champRegister.Add(champion.ChampionName, champion);
			championTypeRegister.Add(champion.ChampionCardType, champion);
            champCards.Add(champion, GetChampionCards(champion));
        }    
    }


    private List<Card> GetChampionCards(Champion champion)
    {
        List<Card> tempC = new List<Card>();
	    foreach (Card card in cardRegister.Values)
        {
            if (!card.ChampionCard || card.ChampionCardType != champion.ChampionCardType) continue;

			tempC.Add(card);
            if (card.TypeOfCard == CardType.Attack)
                tempC.Add(card);
        }
        return tempC;
    }
}
