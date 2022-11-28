using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public abstract class Champion : ScriptableObject
{
    protected GameState gameState;

    public string championName;
    public int health = 100;
    [NonSerialized] public int maxHealth;
    [NonSerialized] public int shield = 0;
    public Sprite artwork;
    public ChampionCardType championCardType;
    [NonSerialized] public string passiveEffect;

    public Champion(string championName, int health, int maxHealth, int shield, Sprite artwork, string passiveEffect, ChampionCardType championType)
    {
        this.championName = championName;
        this.health = health;
        this.maxHealth = health;
        this.shield = shield;
        this.artwork = artwork;
        this.passiveEffect = passiveEffect;
        championCardType = championType;
    }

    public virtual void Awake() { maxHealth = health; gameState = GameState.Instance; }


    
    public virtual void DrawCard(CardDisplay cardDisplay) { }

    public virtual void AmountOfCardsPlayed(Card card) {}

    public virtual int DealDamageAttack(int damage) { return damage; }

    public virtual void UpKeep() {}

    public virtual void EndStep() { }

    public virtual void WhenCurrentChampion() {}

    public virtual void UpdatePassive() {}

    public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.manaCost; }



}
