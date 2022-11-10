using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/AttackSpell")]
public class AttackSpell : Spells
{
    public int damage = 10;
    public bool destroyLandmark = false;
    public bool damageEqualsToYourChampionHP = false;
    public bool damageToBothActiveChampions = false;

    private AttackSpell()
    {
        typeOfCard = CardType.Attack;
        targetable = true;
    }

    private GameState gameState;
    public override void PlaySpell()
    {
        gameState = GameState.Instance;
        if (damageEqualsToYourChampionHP)
            DamageAsYourChampionHP();
        if (Target != null || LandmarkTarget)
            gameState.CalculateBonusDamage(damage, this);

        
        if (destroyLandmark)
        {
            int amountOfLandmarksAlreadyInUse = 0;
            foreach (LandmarkDisplay lDisplay in gameState.opponentLandmarks)
            {
                if (lDisplay.card != null)
                    amountOfLandmarksAlreadyInUse++;
            }
            if (amountOfLandmarksAlreadyInUse == 0) return;

            LandmarkDisplay landmarkDisplay = null;
            int random = Random.Range(0, 4);
            for (int i = 0; i < 25; i++)
            {
                if (gameState.opponentLandmarks[random].card != null)
                {
                    landmarkDisplay = gameState.opponentLandmarks[random]; 
                    break;
                }
                    
            }
            landmarkDisplay.DestroyLandmark();
        }

        if (damageToBothActiveChampions)
        { 
            if (Target == gameState.playerChampion.champion)
                Target = gameState.opponentChampion.champion;
            else if (Target == gameState.opponentChampion.champion)
                Target = gameState.playerChampion.champion;

            gameState.CalculateBonusDamage(damage, this);
        }
            

    }

    private void DamageAsYourChampionHP()
    {
        damage = GameState.Instance.playerChampion.health;
    }
}
