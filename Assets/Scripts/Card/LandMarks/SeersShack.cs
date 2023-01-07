using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/SeersShack")]
public class SeersShack : Landmarks
{
	[Header("Seers Shack")]
	public int CardsShown;

    public SeersShack(SeersShack card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield)
    {

    }

    public override void UpKeep()
	{
		base.UpKeep();
		ListEnum lE = new ListEnum();
		lE.myDeck = true;
		Choice.Instance.ChoiceMenu(lE, -1, WhichMethod.SeersShack, this);
	}
}

