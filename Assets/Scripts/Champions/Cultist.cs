using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cultist", menuName = "Champion/Cultist", order = 1)]
public class Cultist : Champion
{
	public int perMissingHP = 20;
	public int damagePerMissingHP = 10;

    public override void Awake()
	{
		base.Awake();
		UpdatePassive();
    }

	public override int DealDamageAttack(int damage) { return damage + CalculateBonusDamage(); }

	private int CalculateBonusDamage()
	{
        int difference = (maxHealth - health) / perMissingHP;
        return damagePerMissingHP * difference;
    }

	public override void UpdatePassive()
	{
        passiveEffect = CalculateBonusDamage() + "+ Extra damage";
    }
}
