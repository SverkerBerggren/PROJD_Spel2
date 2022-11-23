using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public abstract class Champion : ScriptableObject
{
    private AvailableChampion aC;

    protected GameState gameState;

    public string championName;
    public int health = 100;
    public int maxHealth;
    public int shield = 0;
    public Sprite artwork;
    public string passiveEffect;
    public bool destroyShield = false;
    

    public Champion(string championName, int health, int maxHealth, int shield, Sprite artwork, string passiveEffect)
    {
        gameState = GameState.Instance;
        this.championName = championName;
        this.health = health;
        this.maxHealth = maxHealth;
        this.shield = shield;
        this.artwork = artwork;
        this.passiveEffect = passiveEffect;
    }

    public virtual void Awake() { maxHealth = health; gameState = GameState.Instance; }

    public virtual void TakeDamage(int damage)
    {
        
        if (shield == 0)
        {
            health -= damage;
        }
        else
        {
            if (damage >= shield)
            {
                int differenceAfterShieldDamage = damage - shield;
                shield = 0;
                destroyShield = true;
                EffectController.Instance.DestroyShield(this);
                health -= differenceAfterShieldDamage;
            }
            else
            {
                shield -= damage;
            }
        }

        if (health <= 0)
        {
            Death();
        }
    }

    public virtual void HealChampion(int amountToHeal)
    {
        health += amountToHeal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

    }
    public virtual void GainShield(int amountToBlock)
    {
        destroyShield = false;
        shield += amountToBlock;
    }

    public virtual void DrawCard(CardDisplay cardDisplay) { }

    public virtual void AmountOfCardsPlayed(Card card) {}

    public virtual int DealDamageAttack(int damage) { return damage; }

    public virtual void UpKeep() {}

    public virtual void EndStep() { }

    public virtual void WhenCurrentChampion() {}

    public virtual void UpdatePassive() {}

    public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.manaCost; }

    public virtual void Death()
    {
        gameState.ChampionDeath(this);
    }

}
