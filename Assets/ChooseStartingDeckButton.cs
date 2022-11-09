using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseStartingDeckButton : MonoBehaviour
{

    public List<string> champions = new List<string>(); 
    public List<Card> cards = new List<Card>(); 
    public void OnClick()
    {
        Setup.Instance.myChampions = champions;
        Setup.Instance.playerDeckList = cards;
    }
}
