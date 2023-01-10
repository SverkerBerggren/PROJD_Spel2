using System.Collections.Generic;
using UnityEngine;

public class ChooseStartingDeckButton : MonoBehaviour
{
    public List<string> Champions = new List<string>(); 
    public List<Card> Cards = new List<Card>(); 
    public void OnClick()
    {
        Setup.Instance.myChampions = Champions;
        Setup.Instance.playerDeckList = Cards;
    }
}
