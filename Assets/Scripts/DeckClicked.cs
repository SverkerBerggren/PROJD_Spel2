using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckClicked : MonoBehaviour
{
    private void OnMouseUp()
    {
        ListEnum lE = new ListEnum();
        lE.myDeck = true;
        Choise.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowDeck);
    }
}
