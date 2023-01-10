using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/SwapChampion")]
public class SwapChampion : Spells
{
	protected override void PlaySpell()
    {
        ListEnum listEnum = new ListEnum();
        listEnum.myChampions = true;
        Choice.Instance.ChoiceMenu(listEnum, 1, WhichMethod.SwitchChampionPlayer, null);
    }
}
