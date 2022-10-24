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

    public HealingLandmark(HealingLandmark card) : base(card.minionHealth,card.name,card.description,card.artwork,card.manaCost,card.tag)
    {
        doubleHealingEffect = card.doubleHealingEffect;
        healEachRound = card.healEachRound;
    }

    public override void PlaceLandmark()
    {
        if (doubleHealingEffect)
        {
            foreach (AvailableChampion champ in GameState.Instance.playerChampions)
            {
                champ.champion.landmarkEffect *= 2;
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
                champ.champion.landmarkEffect /= 2;
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
