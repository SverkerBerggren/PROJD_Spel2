using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/SeersShack")]
public class SeersShack : Landmarks
{
	[Header("Seers Shack")]
	public int cardsShown;

	public SeersShack(int mH, string name, string desc, Sprite art, int maxMana, int damage, int amountToHeal, int amountToShield) : base(mH, name, desc, art, maxMana, damage, amountToHeal, amountToShield) {}

	public override void UpKeep()
	{
		base.UpKeep();
		ListEnum lE = new ListEnum();
		lE.myDeck = true;
		Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SeersShack, this);
	}
}

