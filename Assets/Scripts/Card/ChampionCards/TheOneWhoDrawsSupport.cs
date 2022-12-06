using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsSupport")]
public class TheOneWhoDrawsSupport : Spells
{
    public TheOneWhoDrawsSupport()
    {
        championCard = true;
        championCardType = ChampionCardType.TheOneWhoDraws;
    }
    public override void PlaySpell()
    {
        //GameState.Instance.SwapActiveChampion(this);

        if (GameState.Instance.drawnCardsPreviousTurn >= 2)
        {
            GameState.Instance.DrawCard(2, null);
        }
    } 
}
