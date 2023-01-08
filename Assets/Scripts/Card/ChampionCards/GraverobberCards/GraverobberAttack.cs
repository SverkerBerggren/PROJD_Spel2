using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/GraverobberAttack")]

public class GraverobberAttack : Spells
{
    public int TopCardsInGraveyard = 0;

    public GraverobberAttack()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Graverobber;
    }

	protected override void PlaySpell()
    {
        Graveyard graveyard = Graveyard.Instance;

        if (TopCardsInGraveyard > 0)
        {
            int calculation = 0;
            int startLoop = graveyard.GraveyardPlayer.Count - 1;

            for (int i = startLoop; i > 0; i--)
            {
                if (startLoop - TopCardsInGraveyard == i || graveyard.GraveyardPlayer[i] == null) break;
                
                Card cardToCheck = graveyard.GraveyardPlayer[i];
                if (cardToCheck.TypeOfCard == CardType.Attack)
                    calculation += Damage;
            }

            GameState.Instance.CalculateAndDealDamage(calculation, this);
        }

    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nTopCardInGraveyard: " + TopCardsInGraveyard;
        return lineToWriteOut;
    }
}
