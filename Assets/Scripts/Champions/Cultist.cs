using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cultist", menuName = "Champion/Cultist", order = 1)]
public class Cultist : Champion
{
	[System.NonSerialized] public int currentBonusDamage = 0;
	public int perMissingHP = 20;
	public int damagePerMissingHP = 10;


	public Cultist(Cultist c) : base(c.championName, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect, ChampionCardType.Cultist) 
	{
		perMissingHP = c.perMissingHP;
		damagePerMissingHP = c.damagePerMissingHP;
		currentBonusDamage = c.currentBonusDamage;
	}

	public override void Awake()
	{
		base.Awake();
		UpdatePassive();

    }

	public override int DealDamageAttack(int damage)
	{
		return damage + currentBonusDamage;
	}

/*	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
		ChangeBonusDamage();
	}

	public override void HealChampion(int amountToHeal)
	{
		base.HealChampion(amountToHeal);
		ChangeBonusDamage();
	}*/

	private void ChangeBonusDamage()
	{
		int difference = (maxHealth - health) / perMissingHP;
		currentBonusDamage = damagePerMissingHP * difference;
		
	}

	public override void UpdatePassive()
	{
        passiveEffect = currentBonusDamage + "+ Extra damage";
    }
}
