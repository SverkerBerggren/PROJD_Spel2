using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effects : MonoBehaviour
{
	protected GameState gameState;
	protected CardType cardTrigger;
	protected bool untilEndStep = false;

	protected Effects(CardType cardType, bool untilEndStep)
	{
		cardTrigger = cardType;
		this.untilEndStep = untilEndStep;
		gameState = GameState.Instance;
	}

	public virtual void UpKeep() { }

	public virtual void EndStep()
	{
		if (untilEndStep)
		{
			gameState.RemoveEffect(this);
		}
	}

	public virtual void TakeDamage(int damage) {}

	public virtual void AmountOfCardsPlayed(Card card) { }

	public virtual int DealDamageAttack(int damage) { return damage; }

	public virtual int HealingEffect(int healing) { return healing; }

	public virtual int ShieldingEffect(int shielding) { return shielding; }

	public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.manaCost; }

}
