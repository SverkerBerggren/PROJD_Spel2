using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmarks : Card
{
    public int minionHealth;

    public Landmarks(int mH, string name, string desc, Sprite art, int maxMana, string tag) : base()
    {
        this.minionHealth = mH;
        this.cardName = name;
        this.description = desc;
        this.artwork = art;
        this.tag = tag;
        this.typeOfCard = CardType.Landmark;
        maxManaCost = maxMana;
    }

    public override void PlayCard()
    {
        PlaceLandmark();
    }

    public void TakeDamage(int damageToTake)
    {
        minionHealth -= damageToTake;
    }

    public virtual void PlaceLandmark() {}

    public virtual void LandmarkEffectTakeBack() { }

    public virtual void DrawCard() { }

    public virtual void AmountOfCardsPlayed() { }

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
