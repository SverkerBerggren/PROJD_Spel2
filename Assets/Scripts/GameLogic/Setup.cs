using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    private CardRegister cardRegister;
    private Deckbuilder deckbuilder;

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

    public void AddChampion(Champion champion)
    {
        myChampions.Add(champion.championName);
        playerDeckList.AddRange(cardRegister.GetChampionCards(champion));
        deckbuilder.UpdateDeckList();
    }

    public void RemoveChampion(Champion champion)
    {
        myChampions.Remove(champion.championName);
        playerDeckList.RemoveAll(x => x == cardRegister.GetChampionCards(champion).Contains(x));
        deckbuilder.UpdateDeckList();
    }

    public void AddCard(Card card)
    {
        playerDeckList.Add(card);
        deckbuilder.UpdateDeckList();
    }

    public void RemoveCard(Card card)
    {
        playerDeckList.Remove(card);
        deckbuilder.UpdateDeckList();
    }

    public void ClearDeck()
    {
        playerDeckList.Clear();
        myChampions.Clear();
        deckbuilder.UpdateDeckList();
    }
}
