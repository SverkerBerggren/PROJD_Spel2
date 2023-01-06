using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/AttackSpell")]
public class AttackSpell : Spells
{
    private GameState gameState;
    private AudioManager audioManager;

    public bool DestroyLandmark = false;
    public bool DamageEqualsToYourChampionHP = false;
    public bool DamageToBothActiveChampions = false;

    private AttackSpell()
    {
        TypeOfCard = CardType.Attack;
        Targetable = true;
    }

    public override void PlaySpell()
    {
        gameState = GameState.Instance;
        audioManager = AudioManager.Instance;

        if (DamageEqualsToYourChampionHP)
            DamageAsYourChampionHP();

        if (DamageToBothActiveChampions)
        {
            DamageToBothActiveChampionsActive();
            return;
        }

        if (Target != null || LandmarkTarget)
            gameState.CalculateAndDealDamage(Damage, this);

        
        if (DestroyLandmark)
        {
            ListEnum lE = new ListEnum();
            lE.opponentLandmarks = true;
            Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.DestroyLandmarkEnemy, null);
        }

        switch (gameState.playerChampion.champion.ChampionName)
        {
            case "Shanker":
            audioManager.PlayShankerAttack();
            break;

            case "Builder":
            audioManager.PlayBuilderAttackSound();
            break;

            case "Graverobber":
            audioManager.PlayGraveDiggerAttackSound();
            break;
        }
    }

    private void DamageToBothActiveChampionsActive()
    {
        Target = gameState.opponentChampion.champion;
        gameState.CalculateAndDealDamage(Damage, this);

        Target = gameState.playerChampion.champion;
        gameState.CalculateAndDealDamage(Damage, this);
    }

    private void DamageAsYourChampionHP()
    {
        Damage = GameState.Instance.playerChampion.health;
    }
}
