using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/ShankerAttack")]
public class ShankerAttack : Spells
{
    public int damage = 0;

    public ShankerAttack()
    {
        championCard = true;
        championCardType = ChampionCardType.Shanker;
    }
    public override void PlaySpell()
    {
        //Damage Equals amount of cards discarded
    }
}
