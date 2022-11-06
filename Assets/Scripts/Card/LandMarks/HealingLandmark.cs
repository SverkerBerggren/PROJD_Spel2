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

    public HealingLandmark(HealingLandmark card) : base(card.minionHealth,card.cardName,card.description,card.artwork,card.manaCost,card.tag)
    {
        doubleHealingEffect = card.doubleHealingEffect;
        healEachRound = card.healEachRound;
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
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                GameState.Instance.landmarkEffect *= 2;
            }
        }
        
        if (healEachRound)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                champ.champion.healEachRound = true;
            }
        }
    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();

        if (doubleHealingEffect)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                GameState.Instance.landmarkEffect /= 2;
            }
        }

        if (healEachRound)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                champ.champion.healEachRound = false;
            }
        }
    }
}
