using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraveyardClick : MonoBehaviour
{
    public void OnClick()
    {
        print("hej");
        ListEnum lE = new ListEnum();
        lE.myGraveyard = true;
        Choice.Instance.ChoiceMenu(lE, 0, WhichMethod.ShowGraveyard, null);
    }
}
