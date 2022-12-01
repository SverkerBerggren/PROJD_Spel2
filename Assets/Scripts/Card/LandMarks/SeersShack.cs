using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/SeersShack")]
public class SeersShack : Landmarks
{
	[Header("Seers Shack")]
	public int cardsShown;

    public SeersShack(SeersShack card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.damage, card.amountToHeal, card.amountToShield)
    {

    }

    public override void UpKeep()
	{
		base.UpKeep();
		ListEnum lE = new ListEnum();
		lE.myDeck = true;
		Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SeersShack, this);
	}
}

