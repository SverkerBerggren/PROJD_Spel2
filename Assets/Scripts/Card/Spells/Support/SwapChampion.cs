using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/SwapChampion")]
public class SwapChampion : Spells
{
    public override void PlaySpell()
    {
        ListEnum listEnum = new ListEnum();
        listEnum.myChampions = true;

        if(GameState.Instance.playerChampions.Count > 1)
            Choise.Instance.ChoiceMenu(listEnum, 1, WhichMethod.switchChampion);
    }
}
