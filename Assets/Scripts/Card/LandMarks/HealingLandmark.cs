using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/HealingLandmark")]
public class HealingLandmark : Landmarks
{
    private GameState gameState;

    public bool DoubleHealingEffect = false;
    public bool HealEachRound = false;

    public HealingLandmark(HealingLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield)
    {
        DoubleHealingEffect = card.DoubleHealingEffect;
        HealEachRound = card.HealEachRound;
    }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();
    }

    public override int HealingEffect(int healing)
    {   
        if(DoubleHealingEffect)
            return healing * 2;
        return healing;
    }

    public override int ShieldingEffect(int shielding)
    {
        if (DoubleHealingEffect)
            return shielding * 2;
        return shielding;
    }

    public override void UpKeep()
    {
        base.UpKeep();
        if (HealEachRound)
        {
            gameState = GameState.Instance;
            for (int i = 0; i < gameState.playerChampions.Count; i++)
            {
                Target = gameState.playerChampions[i].champion;
                gameState.CalculateAndHeal(AmountToHeal, this);
            }
            
        }
           
    }
}
