using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFilterButtons : MonoBehaviour
{
	private Deckbuilder deckbuilder;
	[SerializeField] private List<Toggle> toggles = new List<Toggle>();
	public bool TypeFilter = false;

	public void Start()
	{
        deckbuilder = Deckbuilder.Instance;
	}

	public void OnClick()
    {
		TypeFilter = false;
		foreach (Toggle toggle in toggles) // Is one toggle activated?
        {
            if (toggle.isOn)
            {
                TypeFilter = true;
				break;
            }
        }
		deckbuilder.FilterCards(deckbuilder.CardFilter);
    }
}
