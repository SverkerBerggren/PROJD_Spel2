using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Builder")]
public class BuilderCard : Spells
{
    public BuilderCard()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Builder;
    }
	protected override void PlaySpell()
    {
        GameState gameState = GameState.Instance;
        int calculated = Damage;
        foreach (LandmarkDisplay display in gameState.playerLandmarks)
        {
            if (display.Card == null) continue;

            calculated += Damage;
            gameState.DrawCard(1, null);
        }

        gameState.CalculateAndDealDamage(calculated, this);
    }
}
