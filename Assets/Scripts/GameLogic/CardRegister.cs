using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRegister : MonoBehaviour
{
    private static CardRegister instance;
    public static CardRegister Instance { get; set; }

    [SerializeField] private List<Card> cards = new List<Card>();
    public Dictionary<string, Card> cardRegister = new Dictionary<string, Card>();

    [SerializeField] private List<Effects> effects = new List<Effects>();
    public Dictionary<string, Effects> effectRegister = new Dictionary<string, Effects>();

    [SerializeField] private List<Champion> champions = new List<Champion>();
    public Dictionary<string, Champion> champRegister = new Dictionary<string, Champion>();
    // Start is called before the first frame update

    private void Awake()
    {
        if (GameState.Instance != null)
        {
            if (GameState.Instance.isOnline)
                gameObject.SetActive(false);
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        foreach (Card card in cards)
        {
            cardRegister.Add(card.cardName, card);
        }

        foreach (Effects effect in effects)
        {
            effectRegister.Add(effect.name, effect);
        }

        foreach (Champion champion in champions)
        {
            champRegister.Add(champion.championName, champion);
        }

        DontDestroyOnLoad(this);
    }

    public List<Card> GetChampionCards(Champion champion)
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
}
