using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public abstract class Champion : ScriptableObject
{
    public new string name;
    public int health = 100;
    public int maxHealth;
    public int shield = 0;
    public Sprite artwork;
    public string passiveEffect;
    public bool healEachRound = false;

    
    private GameState gameState;

    public Champion(string name, int health, int maxHealth, int shield, Sprite artwork, string passiveEffect)
    {
        this.name = name;
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
            if (damage > shield)
            {
                int differenceAfterShieldDamage = damage - shield;
                shield = 0;
                health -= differenceAfterShieldDamage;
            }
            else
            {
                shield -= damage;
            }
        }

        if (health <= 0)
        {
            //Death();
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
        //shield += amountToBlock * landmarkEffect;
    }

    public virtual void DrawCard() { }

    public virtual void AmountOfCardsPlayed() { }

    public virtual int DealDamageAttack(int damage) { return damage; }

    public virtual void UpKeep() { HealEachRound(); } // Os�ker p� om jag gjort r�tt n�r jag la in den h�r

    public virtual void EndStep() { }

    public virtual void WhenCurrentChampion() {}

    public virtual void WhenLandmarksDie() {}

    public virtual void Death()
    {

        //CancelInvoke();
        GameState.Instance.ChampionDeath(this);
    }

}
