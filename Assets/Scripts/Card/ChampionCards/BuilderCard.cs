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

        for (int i = 0; i < gameState.playerLandmarks.Count; i++)
        {
            if (gameState.playerLandmarks == null) continue;
            damage += 10;
            gameState.DrawCard(1, null);
        }

        gameState.CalculateAndDealDamage(damage, this);
    }
}
