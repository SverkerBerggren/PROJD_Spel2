using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Builder")]
public class BuilderCard : Spells
{
    public int damage = 0;

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
            damage += 10;
            gameState.DrawCard(1, null);
        }       
        if (Target != null)
            //Target.TakeDamage(damage);
        if (LandmarkTarget != null)
            LandmarkTarget.TakeDamage(damage);       
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut = "\nDamage: " + damage;
        return lineToWriteOut;
    }
}
