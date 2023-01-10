using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Spells/GraveyardDraw")]
public class GraveyardDraw : Spells
{
	protected override void PlaySpell()
    {
        GameState.Instance.DrawRandomCardFromGraveyard(AmountOfCardsToDraw);
    }
}
