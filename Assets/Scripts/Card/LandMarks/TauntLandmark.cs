using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/TauntLandmark")]
public class TauntLandmark : Landmarks
{    
    public TauntLandmark(TauntLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield) {}
}
