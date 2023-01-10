using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsSupport")]
public class TheOneWhoDrawsSupport : Spells
{
    public int CardsDrawnNeeded;

    public TheOneWhoDrawsSupport()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.TheOneWhoDraws;
    }
    protected override void PlaySpell()
    {
        if (GameState.Instance.DrawnCardsPreviousTurn >= CardsDrawnNeeded)
        {
            GameState.Instance.DrawCard(2, null);
        }
    } 
}
