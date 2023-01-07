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

	protected override void PlaySpell()
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

        switch (gameState.playerChampion.Champion.ChampionName)
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
        Target = gameState.opponentChampion.Champion;
        gameState.CalculateAndDealDamage(Damage, this);

        Target = gameState.playerChampion.Champion;
        gameState.CalculateAndDealDamage(Damage, this);
    }

    private void DamageAsYourChampionHP()
    {
        Damage = GameState.Instance.playerChampion.Health;
    }
}
