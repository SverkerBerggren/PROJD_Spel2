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

    [NonSerialized] public int MaxHealth;
    [NonSerialized] public int Shield = 0;
    [NonSerialized] public string PassiveEffect;
    [NonSerialized] public Animator ChampAnimator;

    public string ChampionName;
    public string Description;
    public int Health = 100;
    public Sprite Artwork;
    public ChampionCardType ChampionCardType;
    public Sprite ChampBackground;

    public GameObject ChampionMesh;


    public virtual void Awake() { MaxHealth = Health; gameState = GameState.Instance; }


    
    public virtual void DrawCard(CardDisplay cardDisplay) { }

    public virtual void AmountOfCardsPlayed(Card card) {}

    public virtual int DealDamageAttack(int damage) { return damage; }

    public virtual void UpKeep() {}

    public virtual void EndStep() { }

    public virtual void WhenCurrentChampion() {}

    public virtual void UpdatePassive() {}

    public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.manaCost; }



}
