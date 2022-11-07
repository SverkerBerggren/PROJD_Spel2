using System.Collections;
using System.Collections.Generic;
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
            LandmarkTarget.DestroyLandmark();
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
