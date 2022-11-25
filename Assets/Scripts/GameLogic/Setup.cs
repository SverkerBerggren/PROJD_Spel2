using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    private CardRegister cardRegister;
    private Deckbuilder deckbuilder;
    [SerializeField] private int maxCopies = 3;
    [SerializeField] private int deckCount = 40;

    [System.NonSerialized] public List<string> opponentChampions = new List<string>();
    public List<string> myChampions = new List<string>();
    public List<Card> playerDeckList = new List<Card>();


    private static Setup instance;
    public static Setup Instance { get { return instance; } set { instance = value; } }
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
    // Start is called before the first frame update
    void Start()
    {
        deckbuilder = Deckbuilder.Instance;
        cardRegister = CardRegister.Instance;
        DontDestroyOnLoad(gameObject);
    }

    public void AddChampion(string championName)
    {
        Champion champion = cardRegister.champRegister[championName];
        myChampions.Add(championName);
        playerDeckList.AddRange(cardRegister.GetChampionCards(champion));
        deckbuilder.UpdateDeckList();
    }

    public void RemoveChampion(string championName)
    {
        Champion champion = cardRegister.champRegister[championName];
        myChampions.Remove(championName);
        playerDeckList.RemoveAll(x => x == cardRegister.GetChampionCards(champion).Contains(x));
        deckbuilder.UpdateDeckList();
    }

    public void AddCard(string cardName)
    {
        if (cardRegister.cardRegister.ContainsKey(cardName) && playerDeckList.Count < deckCount)
        {
            playerDeckList.Add(cardRegister.cardRegister[cardName]);
            deckbuilder.UpdateDeckList();
        }
    }

    public void RemoveCard(string cardName)
    {
        if (cardRegister.cardRegister.ContainsKey(cardName) && playerDeckList.Contains(cardRegister.cardRegister[cardName]))
        {
            playerDeckList.Remove(cardRegister.cardRegister[cardName]);
            deckbuilder.UpdateDeckList();
        }
    }

    public void ClearDeck()
    {
        playerDeckList.Clear();
        myChampions.Clear();
        deckbuilder.UpdateDeckList();
    }
}
