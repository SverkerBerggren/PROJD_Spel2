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
    public Dictionary<string, Func<string>> actions = new Dictionary<string, Func<string>>();

    private CardDisplayAttributes cardDisplayAttributes;

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
			Func<string> action = null;
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

    public string CheckKeyword(CardDisplayAttributes cardDisplayAttributes)
    {
        this.cardDisplayAttributes = cardDisplayAttributes;

        string[] s = cardDisplayAttributes.description.text.Split(" ");
        string returnString = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (actions.Keys.Contains(s[i]))
            {   
                s[i] = actions[s[i]]();
            }
            returnString += s[i];
            if(i + 1 != s.Length)
                returnString += " ";
        }
        return returnString;
    }

    public string Attack()
    {
		return "Deal <b><color=#2564FF>" + cardDisplayAttributes.damageShow + "</color></b> damage";
    }

    public string Shield()
    {
		return "<b><color=#2564FF>" + cardDisplayAttributes.amountToShieldShow + "</color></b> shield";
    }

    public string Heal()
    {
        return "Heal <b><color=#2564FF>" + cardDisplayAttributes.amountToHealShow + "</color></b>";
    }

    public string Draw()
    {
        int amountOfCardsToDraw = cardDisplayAttributes.amountOfCardsToDrawShow;
        if (amountOfCardsToDraw == 1)
        {
            return "Draw a card";
        }
        return "Draw <b><color=#2564FF>" + amountOfCardsToDraw + "</color></b> cards";
    }

    public string Discard()
    {
        int amountOfCardsToDiscard = cardDisplayAttributes.amountOfCardsToDiscardShow;
        if (amountOfCardsToDiscard == 1)
        {
            return "Discard a card";
        }
        return "Discard <b><color=#2564FF>" + amountOfCardsToDiscard + "</color></b> cards";
    }
}