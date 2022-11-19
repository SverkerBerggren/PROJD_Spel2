using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardClick : MonoBehaviour
{
    private void OnMouseUp()
    {
        ListEnum lE = new ListEnum();
        lE.myGraveyard = true;
        Choise.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowGraveyard);
    }
}
