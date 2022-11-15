using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Gravedigging")]
public class Gravedigging : Spells
{
    public int amountOfCardsToReturn;
    public int topCardsInGraveyard = 0;

    public Gravedigging()
    {
        championCard = true;
        championCardType = ChampionCardType.Graverobber;
    }

    public override void PlaySpell()
    {
        Graveyard graveyard = Graveyard.Instance;
        for (int i = 0; i < amountOfCardsToReturn;i++)
        {
            graveyard.RandomizeCardFromGraveyard();
        }  

        if (topCardsInGraveyard > 0)
        {
            int damage = 0;
            for (int i = 0; i < topCardsInGraveyard;i++)
            {
                if (graveyard.graveyardPlayer[i] == null) return;
                Card cardToCheck = graveyard.graveyardPlayer[i];
                if (cardToCheck.GetType().Equals("AttackSpell"))
                {
                    damage += 20;
                }
            }

            GameState.Instance.CalculateBonusDamage(damage, this);
        }

    }
}
