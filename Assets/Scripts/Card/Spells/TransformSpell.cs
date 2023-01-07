using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/TransformSpell")]
public class TransformSpell : Spells
{
	protected override void PlaySpell()
    {
        ListEnum lE = new ListEnum();
        lE.myHand = true;
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.TransformChampionCard, this);
    }
}
