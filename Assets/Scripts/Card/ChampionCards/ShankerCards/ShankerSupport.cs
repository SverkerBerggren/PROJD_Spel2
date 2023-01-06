using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/ShankerSupport")]
public class ShankerSupport : Spells
{

    public ShankerSupport()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Shanker;
    }
    public override void PlaySpell()
    {
        //Reduce cost of attack cards
    }
}
