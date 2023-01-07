using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/HealAndShieldChampion")]
public class HealAndShieldChampion : Spells
{
    public bool AllChampions;

    public override void PlaySpell()
    {
        if (AmountToHeal > 0)
            Heal();
        if (AmountToShield > 0)
            Shield();
    }

    private void Heal()
    {
        if (AllChampions)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                Target = champ.champion;
                GameState.Instance.CalculateAndHeal(AmountToHeal, this);
            }
        }
        else
        {
            GameState.Instance.CalculateAndHeal(AmountToHeal, this);
        }
    }
    private void Shield()
    {
        if (AllChampions)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                Target = champ.champion;
                GameState.Instance.CalculateAndShield(AmountToShield, this);
            }
        }
        else
        {
            Target = GameState.Instance.playerChampion.champion;
            GameState.Instance.CalculateAndShield(AmountToShield, this);
        }
    }
}
