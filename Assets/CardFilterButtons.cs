using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFilterButtons : MonoBehaviour
{
	private Deckbuilder deckbuilder;
	public bool typeFilter = false;
	[SerializeField] private List<Toggle> toggles = new List<Toggle>();

	public void Start()
	{
        deckbuilder = Deckbuilder.Instance;
	}

	public void OnClick()
    {
		typeFilter = false;
		foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                typeFilter = true;
				break;
            }
        }
		deckbuilder.FilterCards(deckbuilder.cardFilter);
    }
}
