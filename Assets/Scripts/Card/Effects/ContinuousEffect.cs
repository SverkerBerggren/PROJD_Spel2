using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContinuousEffect : MonoBehaviour
{

	public virtual void EndStep() { }
	public virtual void OnPlayCard() { }
	public virtual void OnDrawCard(Card card) { }
	public virtual int OnShielding(int amountToModify) { return amountToModify; }
	public virtual int OnHealing(int amountToModify) { return amountToModify; }
	public virtual int OnDealDamage(int amountToModify) { return amountToModify; }
}
