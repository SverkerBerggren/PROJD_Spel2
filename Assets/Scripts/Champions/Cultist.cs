using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cultist", menuName = "Champion/Cultist", order = 1)]
public class Cultist : Champion
{
	public int PerMissingHP = 20;
	public int DamagePerMissingHP = 10;

    public override void Awake()
	{
		base.Awake();
		UpdatePassive();
    }

	public override int DealDamageAttack(int damage) { return damage + CalculateBonusDamage(); }

	private int CalculateBonusDamage()
	{
        int difference = (MaxHealth - Health) / PerMissingHP;
        return DamagePerMissingHP * difference;
    }

	public override void UpdatePassive()
	{
        PassiveEffect = CalculateBonusDamage() + "+ Extra damage";
    }
}
