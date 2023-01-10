using UnityEngine;

public class Landmarks : Card
{
    [Header("Landmark")]
    public int MinionHealth;

    public Landmarks(int mH, string name, string desc, int maxMana, int damage, int amountToHeal, int amountToShield) : base()
    {
        MinionHealth = mH;
        CardName = name;
        Description = desc;
        TypeOfCard = CardType.Landmark;
        MaxManaCost = maxMana;

        AmountToHeal = amountToHeal;
        AmountToShield = amountToShield;
        Damage = damage;
    }

    public override void PlayCard()
    {
        PlaceLandmark();
        GameState.Instance.Refresh();
        AudioManager.Instance.PlayLandmarkSound();
    }

    public void TakeDamage(int damageToTake)
    {
        MinionHealth -= damageToTake;
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

    public virtual int CalculateManaCost(CardDisplay cardDisplay) { return cardDisplay.ManaCost; }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nMinionHealth: " + MinionHealth;
        return lineToWriteOut;
    }
}
