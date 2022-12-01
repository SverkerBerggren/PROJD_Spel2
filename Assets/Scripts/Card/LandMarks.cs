using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmarks : Card
{
    [Header("Landmark")]
    public int minionHealth;

    public Landmarks(int mH, string name, string desc, Sprite art, int maxMana, int damage, int amountToHeal, int amountToShield) : base()
    {
        minionHealth = mH;
        cardName = name;
        description = desc;
        artwork = art;
        typeOfCard = CardType.Landmark;
        maxManaCost = maxMana;

        this.amountToHeal = amountToHeal;
        this.amountToShield = amountToShield;
        this.damage = damage;
    }

    public override void PlayCard()
    {
        //base.PlayCard();
        PlaceLandmark();
        GameState.Instance.Refresh();
    }

    public void TakeDamage(int damageToTake)
    {
        minionHealth -= damageToTake;
    }

    public virtual void PlaceLandmark() {}

    public virtual void AmountOfCardsPlayed(Card card) { }

    public virtual int DealDamageAttack(int damage) { return damage; }

    public virtual int HealingEffect(int healing) { return healing; }

    public virtual int ShieldingEffect(int shielding) { return shielding; }

    public virtual void UpKeep()  {}

    public virtual void EndStep() { }

    public virtual void WhenCurrentChampion() { }

    public virtual void WhenLandmarksDie() { }

    public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.manaCost; }


    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nMinionHealth: " + minionHealth;
        return lineToWriteOut;
    }
}
