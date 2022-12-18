using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckClicked : MonoBehaviour
{
    public void OnClick()
    {
        ListEnum lE = new ListEnum();
        lE.myDeck = true;
        Choice.Instance.ChoiceMenu(lE, 2, WhichMethod.ShowDeck, null);
    }
}
