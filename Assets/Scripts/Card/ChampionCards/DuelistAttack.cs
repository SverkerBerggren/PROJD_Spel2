using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistAttack")]
public class DuelistAttack : Spells
{
    public int damage = 30;
    private GameState gameState;

    public DuelistAttack()
    {
        championCard = true;
        championCardType = ChampionCardType.Duelist;
    }

    public override void PlaySpell()
    {
        gameState = GameState.Instance;

        gameState.CalculateAndDealDamage(damage, this);

        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;      

        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionDied);

        gameState.CalculateAndDealDamage(damage, this);
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nDamage: " + damage;
        return lineToWriteOut;
    }
}
