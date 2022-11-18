using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/HealAndShieldChampion")]
public class HealAndShieldChampion : Spells
{
    public int amountToHeal;
    public int amountToDefence;
    public bool allChampions;
    public override void PlaySpell()
    {
        if (amountToHeal > 0)
            Heal();
        if (amountToDefence > 0)
            Shield();
    }

    private void Heal()
    {
        if (allChampions)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                Target = champ.champion;
                GameState.Instance.CalculateAndHeal(amountToHeal, this);
            }
        }
        else
        {
            GameState.Instance.CalculateAndHeal(amountToHeal, this);
        }
    }
    private void Shield()
    {
        if (allChampions)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                Target = champ.champion;
                GameState.Instance.CalculateAndShield(amountToDefence, this);
            }
        }
        else
        {
            Target = GameState.Instance.playerChampion.champion;
            GameState.Instance.CalculateAndShield(amountToDefence, this);
        }
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nAmountToHeal: " + amountToHeal + "\nDefence: " + amountToDefence;
        return lineToWriteOut;
    }
}
