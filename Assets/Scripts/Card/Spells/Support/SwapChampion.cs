using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/SwapChampion")]
public class SwapChampion : Spells
{
    public override void PlaySpell()
    {
        GameState.Instance.SwapActiveChampion();
    }
}
