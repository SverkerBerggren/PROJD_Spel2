using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/GraverobberAttack")]

public class GraverobberAttack : Spells
{
    //public int amountOfCardsToReturn;
    public int topCardsInGraveyard = 0;
    //public bool graveGrief = false;

    public GraverobberAttack()
    {
        championCard = true;
        championCardType = ChampionCardType.Graverobber;
    }

    public override void PlaySpell()
    {
        Graveyard graveyard = Graveyard.Instance;

        if (topCardsInGraveyard > 0)
        {
            int damage = 0;
            int startLoop = graveyard.graveyardPlayer.Count - 1;

            for (int i = startLoop; i > 0; i++)
            {
                if (startLoop - topCardsInGraveyard == i || graveyard.graveyardPlayer[i] == null) break;
                
                Card cardToCheck = graveyard.graveyardPlayer[i];
                if (cardToCheck.typeOfCard == CardType.Attack)
                {
                    damage += 20;
                }
            }

            GameState.Instance.CalculateAndDealDamage(damage, this);
        }

    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nTopCardInGraveyard: " + topCardsInGraveyard;
        return lineToWriteOut;
    }
}
