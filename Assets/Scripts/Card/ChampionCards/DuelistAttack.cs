using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistAttack")]
public class DuelistAttack : Spells
{
    public int damage = 30;
    private GameState gameState;
    public override void PlaySpell()
    {
        gameState = GameState.Instance;

        gameState.CalculateBonusDamage(damage, this);
        gameState.SwapActiveChampion(this);
        gameState.CalculateBonusDamage(damage, this);
    }
}
