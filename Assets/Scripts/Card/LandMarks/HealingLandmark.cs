using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/HealingLandmark")]
public class HealingLandmark : Landmarks
{
    public bool doubleHealingEffect = false;
    public bool healEachRound = false;

    private GameState gameState;

    public HealingLandmark(HealingLandmark card) : base(card.minionHealth,card.cardName,card.description,card.artwork,card.manaCost,card.tag)
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
        if (doubleHealingEffect)
        {
            foreach (AvailableChampion champ in gameState.playerChampions)
            {
                gameState.landmarkEffect *= 2;
            }
        }
    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();

        if (doubleHealingEffect)
        {
            foreach (AvailableChampion champ in gameState.playerChampions)
            {
                gameState.landmarkEffect /= 2;
            }
        }
    }

    public override void UpKeep()
    {
        base.UpKeep();
        if (healEachRound)
        {
            for (int i = 0; i < gameState.playerChampions.Count; i++)
            {
                Target = gameState.playerChampions[i].champion;
                gameState.CalculateHealing(10, this);
            }
            
        }
           
    }
}
