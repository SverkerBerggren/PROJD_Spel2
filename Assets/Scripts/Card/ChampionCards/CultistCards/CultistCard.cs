using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Cultist")]
public class CultistCard : Spells
{
    public bool Deluge = false;
    public bool RitualSactifice = false;

    private GameState gameState;
    public CultistCard()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Cultist;
    }
    public override void PlaySpell()
    {
        gameState = GameState.Instance;
        if (Deluge)
        {
            DelugeActivate();
        }
        else if (RitualSactifice)
        {
            RitualSacrificeActivate();
        }

        
        
    }

    private void DelugeActivate()
    {
        Target = gameState.opponentChampion.champion;
        gameState.CalculateAndDealDamage(Damage, this);
        Target = null;

        foreach (LandmarkDisplay landmark in gameState.opponentLandmarks)
        {
            if (landmark.card == null) continue;

            LandmarkTarget = landmark;
            gameState.CalculateAndDealDamage(Damage, this);
        }
    }
    private void RitualSacrificeActivate()
    {
        Target = gameState.playerChampion.champion;
        gameState.CalculateAndDealDamage(Damage, this);
    }
}
