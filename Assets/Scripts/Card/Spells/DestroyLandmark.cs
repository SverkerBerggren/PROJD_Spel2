using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/DestroyLandmark")]
public class DestroyLandmark : Spells
{
	protected override void PlaySpell()
    {
        ListEnum lE = new ListEnum();
        lE.opponentLandmarks = true;
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.DestroyLandmarkEnemy, this);
    }
}
