using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Builder")]
public class BuilderCard : Spells
{
    public BuilderCard()
    {
        championCard = true;
        championCardType = ChampionCardType.Builder;
    }
    public override void PlaySpell()
    {
        GameState gameState = GameState.Instance;
        int calculated = damage;
        foreach (LandmarkDisplay display in gameState.playerLandmarks)
        {
            if (display.card == null) continue;

            calculated += damage;
            gameState.DrawCard(1, null);
        }

        gameState.CalculateAndDealDamage(calculated, this);
    }
}
