using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spells : Card
{
    protected Spells()
    {
        TypeOfCard = CardType.Spell;
    }

    public override void PlayCard()
    {   
        base.PlayCard();
        PlaySpell();
        GameState.Instance.Refresh();
    }

   


    public abstract void PlaySpell();
}
