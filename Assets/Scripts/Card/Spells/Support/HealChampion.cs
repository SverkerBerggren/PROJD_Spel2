using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/HealChampion")]
public class HealChampion : Spells
{
    public int amountToHeal;
    public bool allChampions;
    public override void PlaySpell()
    {
        if (allChampions)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                Target = champ.champion;
                GameState.Instance.CalculateHealing(amountToHeal, this);
            }
        }
        else
        {
            GameState.Instance.CalculateHealing(amountToHeal, this);
        }
    }

    public override string ToString()
    {
        string lineToWriteOut = base.ToString();
        lineToWriteOut = "\nAmountToHeal: " + amountToHeal;
        return lineToWriteOut;
    }
}
