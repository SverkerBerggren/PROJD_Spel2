using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckClicked : MonoBehaviour
{
    public void OnClick()
    {
        if (GameState.Instance.IsItMyTurn && GameState.Instance.HasPriority)
        {
            ListEnum lE = new ListEnum();
            lE.myDeck = true;
            Choice.Instance.ChoiceMenu(lE, 2, WhichMethod.ShowDeck, null);
        }
    }
}
