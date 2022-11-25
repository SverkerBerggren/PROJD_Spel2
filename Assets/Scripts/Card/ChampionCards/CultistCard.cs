using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Cultist")]
public class CultistCard : Spells
{
    [Header("Deluge")]
    public bool damageToAllOpponentCards;


    public CultistCard()
    {
        championCard = true;
        championCardType = ChampionCardType.Cultist;
    }
    public override void PlaySpell()
    {
        GameState gameState = GameState.Instance;
        Target = gameState.playerChampion.champion;
        gameState.CalculateAndDealDamage(damage, this);

        if (damageToAllOpponentCards)
        {
            Target = gameState.opponentChampion.champion;
            gameState.CalculateAndDealDamage(damage, this);

            /* MÅste fixa så att den targetar landmarks */
            foreach (LandmarkDisplay landmark in gameState.opponentLandmarks)
            {
                gameState.CalculateAndDealDamage(damage, this);
            }           
        }
    }
}
