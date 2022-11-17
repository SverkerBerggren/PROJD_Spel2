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

        gameState.CalculateBonusDamage(damage, this);

        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;      

        Choise.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionDied);

        gameState.CalculateBonusDamage(damage, this);
    }

    public override string ToString()
    {
        base.ToString();
        string lineToWriteOut = null;
        lineToWriteOut = "\nDamage: " + damage;
        return lineToWriteOut;
    }
}
