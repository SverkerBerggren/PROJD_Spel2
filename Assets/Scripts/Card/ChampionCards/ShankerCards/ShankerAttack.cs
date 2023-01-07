using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/ShankerAttack")]
public class ShankerAttack : Spells
{
    public ShankerAttack()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Shanker;
    }
	protected override void PlaySpell()
    {
        //Damage Equals amount of cards discarded
        ListEnum lE = new ListEnum();
        lE.myHand = true;

        Choice.Instance.ChoiceMenu(lE, -1, WhichMethod.DiscardXCards, this);        
    }

    public void WaitForChoices(int amountOfChoices)
    {
        Damage *= (amountOfChoices + 1);
        GameState.Instance.CalculateAndDealDamage(Damage, this);
    }
}
