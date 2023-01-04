using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFilterButtons : MonoBehaviour
{
	private Deckbuilder deckbuilder;
	public bool typeFilter = false;
	private List<Toggle> toggles = new List<Toggle>();
	private Dictionary<Toggle, CardType> toggleType = new Dictionary<Toggle, CardType>();

	public void Start()
	{
        deckbuilder = Deckbuilder.Instance;
		foreach (Toggle toggle in toggles)
		{
			toggleType.Add(toggle, CardType.Spell);
		}
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
