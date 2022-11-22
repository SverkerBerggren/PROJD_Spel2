using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

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
        AddKeywords();
    }

    private void AddKeywords()
    {
		foreach (string keyword in keywords)
		{
			Func<CardDisplay, string> action = null;
			switch (keyword)
			{
				case "*Damage*":
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
		return "Deal " + cardDisplay.damageShow.ToString() + " damage";
    }

    public string Shield(CardDisplay cardDisplay)
    {
		return cardDisplay.amountToShieldShow.ToString() + " shield";
    }

    public string Heal(CardDisplay cardDisplay)
    {
        return "Heal " + cardDisplay.amountToHealShow.ToString();
    }

    public string Draw(CardDisplay cardDisplay)
    {
        int amountOfCardsToDraw = cardDisplay.amountOfCardsToDrawShow;
        if (amountOfCardsToDraw == 1)
        {
            return "Draw a card";
        }
        return "Draw " + amountOfCardsToDraw.ToString() + " cards";
    }

    public string Discard(CardDisplay cardDisplay)
    {
        int amountOfCardsToDiscard = cardDisplay.amountOfCardsToDiscardShow;
        if (amountOfCardsToDiscard == 1)
        {
            return "Discard a card";
        }
        return "Discard " + amountOfCardsToDiscard.ToString() + " cards";
    }
}