using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/ShankerAttack")]
public class ShankerAttack : Spells
{
    public ShankerAttack()
    {
        championCard = true;
        championCardType = ChampionCardType.Shanker;
    }
    public override void PlaySpell()
    {
        //Damage Equals amount of cards discarded
        ListEnum lE = new ListEnum();
        lE.myHand = true;

        Choice.Instance.ChoiceMenu(lE, -1, WhichMethod.discardXCardsInMyHand, this);        
    }

    public void WaitForChoices(int amountOfChoices)
    {
        damage *= amountOfChoices;
        GameState.Instance.CalculateAndDealDamage(damage, this);
    }
}
