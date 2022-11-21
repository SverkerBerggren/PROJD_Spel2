using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
            Func<CardDisplay, string> action = Attack;
            actions.Add(keyword, action);
        }
    }

    public void CheckKeyword(CardDisplay card)
    {
        string[] s = card.description.text.Split(" ");
        string returnString = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (actions.Keys.Contains(s[i]))
            {
                s[i] = actions[s[i]](card);
            }
            returnString += s[i];
            returnString += " ";
        }
        returnString += "\b";
        card.description.text = returnString;
    }

    public string Attack(CardDisplay cardDisplay)
    {
        AttackSpell card = (AttackSpell)cardDisplay.card;
        return (card.damage+1).ToString();
    }
}