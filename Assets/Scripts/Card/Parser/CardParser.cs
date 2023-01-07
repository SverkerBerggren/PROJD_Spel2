using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardParser : MonoBehaviour
{
    private CardDisplayAttributes cardDisplayAttributes;
    private Dictionary<string, Func<string>> actions = new Dictionary<string, Func<string>>();
    [SerializeField] private string[] keywords;


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

	private string Attack()
    {
		return "Deal <b><color=#2564FF>" + cardDisplayAttributes.damageShow + "</color></b> damage";
    }

	private string Shield()
    {
		return "<b><color=#2564FF>" + cardDisplayAttributes.amountToShieldShow + "</color></b> shield";
    }

	private string Heal()
    {
        return "Heal <b><color=#2564FF>" + cardDisplayAttributes.amountToHealShow + "</color></b>";
    }

	private string Draw()
    {
        int amountOfCardsToDraw = cardDisplayAttributes.amountOfCardsToDrawShow;
        if (amountOfCardsToDraw == 1)
        {
            return "Draw a card";
        }
        return "Draw <b><color=#2564FF>" + amountOfCardsToDraw + "</color></b> cards";
    }

    private string Discard()
    {
        int amountOfCardsToDiscard = cardDisplayAttributes.amountOfCardsToDiscardShow;
        if (amountOfCardsToDiscard == 1)
        {
            return "Discard a card";
        }
        return "Discard <b><color=#2564FF>" + amountOfCardsToDiscard + "</color></b> cards";
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

}