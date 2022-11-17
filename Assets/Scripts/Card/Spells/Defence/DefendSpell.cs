using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/DefenceSpell")]
public class DefendSpell : Spells
{
    public int defence;
    public bool allChampions;

    public override void PlaySpell()
    {
        if (allChampions)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                Target = champ.champion;
                GameState.Instance.CalculateShield(defence, this);
            }
        }
        else
        {
            Target = GameState.Instance.playerChampion.champion;
            GameState.Instance.CalculateShield(defence,this);
        }       
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nDefence: " + defence;
        return lineToWriteOut;
    }
}
