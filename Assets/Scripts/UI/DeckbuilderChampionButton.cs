using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckbuilderChampionButton : MonoBehaviour
{
	[NonSerialized] public Champion champion;
	private int tapCount = 0;

	public void OnClick()
	{
		tapCount++;
		//CancelInvoke();
		Invoke(nameof(OnDoAction), 0.3f);
	}

	private void OnDoAction()
	{
		if (tapCount == 1)
		{
			Add();
		}
		else if (tapCount == 2)
		{
			Remove();
		}
		tapCount = 0;
	}

	public void Add()
	{
		Setup.Instance.AddChampion(champion);
	}

	private void Remove()
	{
		Setup.Instance.RemoveChampion(champion);		
	}
}
