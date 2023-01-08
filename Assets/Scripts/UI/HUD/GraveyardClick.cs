using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardClick : MonoBehaviour
{
    public void OnClick()
    {
        if (GameState.Instance.IsItMyTurn && GameState.Instance.HasPriority)
        {
            ListEnum lE = new ListEnum();
            lE.myGraveyard = true;
            Choice.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowGraveyard, null);
        }
    }

    public void OnClickOpponent()
    {
        if (GameState.Instance.IsItMyTurn && GameState.Instance.HasPriority)
        { 
            ListEnum lE = new ListEnum();
            lE.opponentGraveyard = true;
            Choice.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowOpponentGraveyard, null);
        }
    }
}
