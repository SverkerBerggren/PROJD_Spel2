using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/HealingLandmark")]
public class HealingLandmark : Landmarks
{
    public bool doubleHealingEffect = false;
    public bool healEachRound = false;

    public int amountToHeal = 10;

    private GameState gameState;

    public HealingLandmark(HealingLandmark card) : base(card.minionHealth,card.cardName,card.description,card.artwork,card.maxManaCost, card.tag)
    {
        doubleHealingEffect = card.doubleHealingEffect;
        healEachRound = card.healEachRound;
        gameState = GameState.Instance;
    }

    public static void CreateInstance(HealingLandmark card)
    {
        new HealingLandmark(card);
    }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();
    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();
    }

    public override int HealingEffect(int healing)
    {   
        if(doubleHealingEffect)
            return healing * 2;
        return healing;
    }

    public override int ShieldingEffect(int shielding)
    {
        if (doubleHealingEffect)
            return shielding * 2;
        return shielding;
    }

    public override void UpKeep()
    {
        base.UpKeep();
        if (healEachRound)
        {
            for (int i = 0; i < gameState.playerChampions.Count; i++)
            {
                Target = gameState.playerChampions[i].champion;
                gameState.CalculateAndHeal(amountToHeal, this);
            }
            
        }
           
    }
}
