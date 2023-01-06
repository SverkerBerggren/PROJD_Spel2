using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/GraveyardDraw")]
public class GraveyardDraw : Spells
{
    public override void PlaySpell()
    {
        GameState.Instance.DrawRandomCardFromGraveyard(AmountOfCardsToDraw);
    }
}
