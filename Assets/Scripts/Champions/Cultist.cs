using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cultist", menuName = "Champion/Cultist", order = 1)]
public class Cultist : Champion
{
	public int perMissingHP = 20;
	public int damagePerMissingHP = 10;

	public int currentBonusDamage = 0;

	public Cultist(Cultist c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) 
	{
		perMissingHP = c.perMissingHP;
		damagePerMissingHP = c.damagePerMissingHP;
		currentBonusDamage = c.currentBonusDamage;
	}

	public override void Awake()
	{
		base.Awake();
		passiveEffect = currentBonusDamage + "+";
	}

	public override int DealDamageAttack(int damage)
	{
		return damage + currentBonusDamage;
	}

	public override void TakeDamage(int damage, GameObject gO)
	{
		base.TakeDamage(damage, gO);
		ChangeBonusDamage();
	}

	public override void HealChampion(int amountToHeal)
	{
		base.HealChampion(amountToHeal);
		ChangeBonusDamage();
	}

	private void ChangeBonusDamage()
	{
		int difference = (maxHealth - health) / perMissingHP;
		currentBonusDamage = damagePerMissingHP * difference;
		passiveEffect = currentBonusDamage + "+";
		
	}
}
