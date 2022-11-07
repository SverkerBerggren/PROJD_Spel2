using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsSupport")]
public class TheOneWhoDrawsSupport : Spells
{
    public override void PlaySpell()
    {
        GameState.Instance.SwapActiveChampion(this);
    }
}
