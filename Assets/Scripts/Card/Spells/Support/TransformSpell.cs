using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/TransformSpell")]
public class TransformSpell : Spells
{
    public override void PlaySpell()
    {
        ListEnum lE = new ListEnum();
        lE.myHand = true;
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.TransformChampionCard,this);
    }
}
