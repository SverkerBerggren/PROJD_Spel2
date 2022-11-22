using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardParser : MonoBehaviour
{
    [SerializeField] private string[] keywords;
    public Dictionary<string, Func<CardDisplay, string>> actions = new Dictionary<string, Func<CardDisplay, string>>();

    private static CardParser instance;
    public static CardParser Instance { get; set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (string keyword in keywords)
        {
            Func<CardDisplay, string> action = null;
            switch (keyword)
            {
                case "*Attack*":
                action = Attack;
                break;

                case "*Heal*":
                action = Heal;
                break;

                case "*Shield*":
                action = Shield;
                break;

                case "*Draw*":
                action = Draw;
                break;

                case "*Discard*":
                action = Discard;
                break;
            }
            actions.Add(keyword, action);
        }
    }

    public void CheckKeyword(CardDisplay cardDisplay)
    {
        print(cardDisplay.card + "fasdfasd");
        string[] s = cardDisplay.description.text.Split(" ");
        string returnString = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (actions.Keys.Contains(s[i]))
            {   
                s[i] = actions[s[i]](cardDisplay);
            }
            returnString += s[i];
            if(i + 1 != s.Length)
                returnString += " ";
        }
        cardDisplay.description.text = returnString;
    }

    public string Attack(CardDisplay cardDisplay)
    {
        AttackSpell card = (AttackSpell)cardDisplay.card;
        return "Deal " + (card.damage).ToString() + " damage";
    }

    public string Shield(CardDisplay cardDisplay)
    {
        HealAndShieldChampion card = (HealAndShieldChampion)cardDisplay.card;
        return (card.amountToShield).ToString() + " shield";
    }

    public string Heal(CardDisplay cardDisplay)
    {
        HealAndShieldChampion card = (HealAndShieldChampion)cardDisplay.card;
        return "Heal" + (card.amountToHeal).ToString();
    }

    public string Draw(CardDisplay cardDisplay)
    {
        Card card = cardDisplay.card;
        if (card.amountOfCardsToDraw == 1)
        {
            return "Draw a card";
        }
        return ("Draw " + card.amountOfCardsToDraw).ToString() + " cards";
    }

    public string Discard(CardDisplay cardDisplay)
    {
        Card card = cardDisplay.card;
        if (card.amountOfCardsToDiscard == 1)
        {
            return "Draw a card";
        }
        return ("Draw " + card.amountOfCardsToDiscard).ToString() + " cards";
    }
}