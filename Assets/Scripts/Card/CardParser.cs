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
    public Dictionary<string, Func<Displays, string>> actions = new Dictionary<string, Func<Displays, string>>();

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
			Func<Displays, string> action = null;
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

    public void CheckKeyword(Displays display)
    {
/*        if (display is CardDisplay)       
            display = (CardDisplay)display;
        
        else if (display is LandmarkDisplay)      
            display = (LandmarkDisplay)display;*/

        string[] s = display.cardDisplayAtributes.description.text.Split(" ");
        string returnString = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (actions.Keys.Contains(s[i]))
            {   
                s[i] = actions[s[i]](display);
            }
            returnString += s[i];
            if(i + 1 != s.Length)
                returnString += " ";
        }
        display.cardDisplayAtributes.description.text = returnString;
    }

    public string Attack(Displays display)
    {
		return "Deal <b><color=#2564FF>" + display.damageShow.ToString() + "</color></b> damage";
    }

    public string Shield(Displays display)
    {
		return "<b><color=#2564FF>" + display.amountToShieldShow.ToString() + "</color></b> shield";
    }

    public string Heal(Displays display)
    {
        return "Heal <b><color=#2564FF>" + display.amountToHealShow.ToString() + "</color></b>";
    }

    public string Draw(Displays display)
    {
        int amountOfCardsToDraw = display.amountOfCardsToDrawShow;
        if (amountOfCardsToDraw == 1)
        {
            return "Draw a card";
        }
        return "Draw <b><color=#2564FF>" + amountOfCardsToDraw.ToString() + "</color></b> cards";
    }

    public string Discard(Displays display)
    {
        int amountOfCardsToDiscard = display.amountOfCardsToDiscardShow;
        if (amountOfCardsToDiscard == 1)
        {
            return "Discard a card";
        }
        return "Discard <b><color=#2564FF>" + amountOfCardsToDiscard.ToString() + "</color></b> cards";
    }
}