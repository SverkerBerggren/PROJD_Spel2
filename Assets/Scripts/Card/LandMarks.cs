using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmarks : Card
{
    public int minionHealth;

    public Landmarks(int mH, string name, string desc, Sprite art, int mana, string tag) : base()
    {
        this.minionHealth = mH;
        this.cardName = name;
        this.description = desc;
        this.artwork = art;
        this.manaCost = mana;
        this.tag = tag;
        this.typeOfCard = CardType.Landmark;
    }

    public override void PlayCard()
    {
        PlaceLandmark();
    }

    public void TakeDamage(int damageToTake)
    {
        minionHealth -= damageToTake;
    }

    public virtual void PlaceLandmark() { }
    public virtual void LandmarkEffectTakeBack() { }
}
