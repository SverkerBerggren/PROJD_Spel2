using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRegister : MonoBehaviour
{
    private static CardRegister instance;
    public static CardRegister Instance { get; set; }

    [SerializeField] private List<Card> cards = new List<Card>();
    public Dictionary<string, Card> cardRegister = new Dictionary<string, Card>();


    [SerializeField] private List<Champion> champions = new List<Champion>();
    public Dictionary<string, Champion> champRegister = new Dictionary<string, Champion>();
    // Start is called before the first frame update

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

        foreach (Card card in cards)
        {
            cardRegister.Add(card.cardName, card);
        }

        foreach (Champion champion in champions)
        {
            champRegister.Add(champion.name, champion);
        }

        DontDestroyOnLoad(this);
    }
}
