using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{

	private static TurnController instance;
	public static TurnController Instance { get; set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
		}
	}

	void Update()
    {
        
    }
}
