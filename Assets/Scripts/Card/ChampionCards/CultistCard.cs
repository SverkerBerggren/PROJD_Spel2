using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Cultist")]
public class CultistCard : Spells
{
    public bool deluge = false;
    public bool ritualSactifice = false;

    private GameState gameState;
    public CultistCard()
    {
        championCard = true;
        championCardType = ChampionCardType.Cultist;
    }
    public override void PlaySpell()
    {
        gameState = GameState.Instance;
        if (deluge)
        {
            Deluge();
        }
        else if (ritualSactifice)
        {
            RitualSacrifice();
        }

        
        
    }

    private void Deluge()
    {
        Target = gameState.opponentChampion.champion;
        gameState.CalculateAndDealDamage(damage, this);
        Target = null;

        /* MÅste fixa så att den targetar landmarks */
        foreach (LandmarkDisplay landmark in gameState.opponentLandmarks)
        {
            if (landmark.card == null) continue;

            LandmarkTarget = landmark;
            gameState.CalculateAndDealDamage(damage, this);
        }
    }
    private void RitualSacrifice()
    {
        Target = gameState.playerChampion.champion;
        gameState.CalculateAndDealDamage(damage, this);
    }
}
