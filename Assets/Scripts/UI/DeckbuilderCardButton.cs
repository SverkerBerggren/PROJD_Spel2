using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckbuilderCardButton : MonoBehaviour
{
	public Card card;

    public void OnClick()
    {
        Setup.Instance.AddCard(card);
    }
}
