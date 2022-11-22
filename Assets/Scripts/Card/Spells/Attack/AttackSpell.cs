using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/AttackSpell")]
public class AttackSpell : Spells
{
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

        if (damageToBothActiveChampions)
        {
            DamageToBothActiveChampions();
            return;
        }

        if (Target != null || LandmarkTarget)
            gameState.CalculateAndDealDamage(damage, this);

        
        if (destroyLandmark)
        {
            ListEnum lE = new ListEnum();
            lE.myLandmarks = true;
            Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.ShowLandmarks);
            //DestroyLandmark();
        }

    }

    private void DamageToBothActiveChampions()
    {
        Target = gameState.opponentChampion.champion;
        gameState.CalculateAndDealDamage(damage, this);

        Target = gameState.playerChampion.champion;
        gameState.CalculateAndDealDamage(damage, this);
    }

    private void DestroyLandmark()
    {
        int amountOfLandmarksAlreadyInUse = 0;

        foreach (LandmarkDisplay lDisplay in gameState.opponentLandmarks)
        {
            if (lDisplay.card != null)
                amountOfLandmarksAlreadyInUse++;
        }

        if (amountOfLandmarksAlreadyInUse == 0) return;

        LandmarkDisplay landmarkDisplay = null;
        for (int i = 0; i < 25; i++)
        {
            int random = Random.Range(0, 4);
            if (gameState.opponentLandmarks[random].card != null)
            {
                landmarkDisplay = gameState.opponentLandmarks[random];
                break;
            }

        }
        landmarkDisplay.DestroyLandmark();
    }

    private void DamageAsYourChampionHP()
    {
        damage = GameState.Instance.playerChampion.health;
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nDamage: " + damage;
        return lineToWriteOut;
    }
}
