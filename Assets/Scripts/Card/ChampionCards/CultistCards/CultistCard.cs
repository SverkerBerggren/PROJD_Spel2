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
	protected override void PlaySpell()
    {
        gameState = GameState.Instance;

        if (Deluge)
            DelugeActivate();
        else if (RitualSactifice)
            RitualSacrificeActivate();
    }

    private void DelugeActivate()
    {
        Target = gameState.OpponentChampion.Champion;
        gameState.CalculateAndDealDamage(Damage, this);
        Target = null;

        foreach (LandmarkDisplay landmark in gameState.OpponentLandmarks)
        {
            if (landmark.Card == null) continue;

            LandmarkTarget = landmark;
            gameState.CalculateAndDealDamage(Damage, this);
        }
    }

    private void RitualSacrificeActivate()
    {
		TargetInfo targetInfo = new TargetInfo();
		targetInfo.whichList.myChampions = true;
		targetInfo.index = 0;

        TargetAndAmount targetAndAmount = new TargetAndAmount(targetInfo, Damage);
        gameState.DealDamage(targetAndAmount);
    }
}
