using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/Builder")]
public class BuilderCard : Spells
{
    public int damage = 0;
    public override void PlaySpell()
    {
        GameState gameState = GameState.Instance;

        for (int i = 0; i < gameState.playerLandmarks.Count; i++)
        {
            damage += 10;
            gameState.DrawCard(1);
        }       
        if (Target != null)
            Target.TakeDamage(damage);
        if (LandmarkTarget != null)
            LandmarkTarget.TakeDamage(damage);       
    }
}
