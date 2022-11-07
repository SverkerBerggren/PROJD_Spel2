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

    public new string name;
    public int health = 100;
    public int maxHealth;
    public int shield = 0;
    public Sprite artwork;
    public string passiveEffect;
    public bool healEachRound = false;
    public GameObject mesh;
    public Animator animator;
    public bool destroyShield = false;
    

    public Champion(string name, int health, int maxHealth, int shield, Sprite artwork, string passiveEffect, GameObject mesh)
    {
        gameState = GameState.Instance;
        this.name = name;
        this.health = health;
        this.maxHealth = maxHealth;
        this.shield = shield;
        this.artwork = artwork;
        this.passiveEffect = passiveEffect;
        this.mesh = mesh;
    }

    public virtual void Awake() { maxHealth = health; gameState = GameState.Instance; }

    public virtual void TakeDamage(int damage, GameObject gO)
    {
        
        if (shield == 0)
        {
            health -= damage;
        }
        else
        {
            if (damage > shield)
            {
                int differenceAfterShieldDamage = damage - shield;
                shield = 0;
                destroyShield = true;
                EffectController.Instance.DestoryShield(gO);
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

    public void HealEachRound()
    {
        if (healEachRound)
        {
            HealChampion(10);
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

    public virtual int DealDamageAttack(int damage) { if(animator != null) animator.SetTrigger("Attack"); return damage; }

    public virtual void UpKeep() { HealEachRound(); } // Osäker på om jag gjort rätt när jag la in den här

    public virtual void EndStep() { }

    public virtual void WhenCurrentChampion() {}

    public virtual void WhenLandmarksDie() {}

    public virtual void Death()
    {

        //CancelInvoke();
        gameState.ChampionDeath(this);
    }

}
