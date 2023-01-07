using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraveyardClick : MonoBehaviour
{
    public void OnClick()
    {
        print("kan man se graven");
        if (GameState.Instance.isItMyTurn && GameState.Instance.hasPriority)
        {
            ListEnum lE = new ListEnum();
            lE.myGraveyard = true;
            Choice.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowGraveyard, null);
        }
    }

    public void OnClickOpponent()
    {
        if (GameState.Instance.isItMyTurn && GameState.Instance.hasPriority)
        { 
            ListEnum lE = new ListEnum();
            lE.opponentGraveyard = true;
            Choice.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowOpponentGraveyard, null);
        }
    }
}
