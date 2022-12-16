using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effects : ScriptableObject
{
	protected GameState gameState;
	public CardType cardTrigger;
	public bool untilEndStep = false;

	public abstract void AddEffect();
	public virtual void UpKeep() { }
	public virtual void EndStep()
	{
		if (untilEndStep)
		{
			GameState.Instance.RemoveEffect(this);
		}
	}
	public virtual void TakeDamage(int damage) {}
	public virtual void AmountOfCardsPlayed(Card card) { }
	public virtual int DealDamageAttack(int damage) { return damage; }
	public virtual int HealingEffect(int healing) { return healing; }
	public virtual int ShieldingEffect(int shielding) { return shielding; }
	public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.manaCost; }
}
