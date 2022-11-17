using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Cultist")]
public class CultistCard : Spells
{
    [Header("RitualSacrifice")]
    public int selfInflictDamage = 0;
    [Header("Deluge")]
    public bool damageToAllOpponentCards;
    public int damageToDealToAllOpponent = 0;

    public CultistCard()
    {
        championCard = true;
        championCardType = ChampionCardType.Cultist;
    }
    public override void PlaySpell()
    {
        GameState gameState = GameState.Instance;
        Target = gameState.playerChampion.champion;
        gameState.CalculateAndDealDamage(selfInflictDamage, this);

        if (damageToAllOpponentCards)
        {
            Target = gameState.opponentChampion.champion;
            gameState.CalculateAndDealDamage(damageToDealToAllOpponent, this);

            /* MÅste fixa så att den targetar landmarks */
            foreach (LandmarkDisplay landmark in gameState.opponentLandmarks)
            {
                gameState.CalculateAndDealDamage(damageToDealToAllOpponent, this);
            }           
        }
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nSelfInflictDamage: " + selfInflictDamage + "\nDamageToDealToAllOpponents: " + damageToDealToAllOpponent;
        return lineToWriteOut;
    }
}
