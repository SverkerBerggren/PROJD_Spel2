using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
	private GameState gameState;
	private float turnTimer;

	[SerializeField] private MinuteHolder time;
	[SerializeField] private float playerTimer;
	[SerializeField] private float opponentTimer;


	private static TurnController instance;
	public static TurnController Instance { get; set; }

	private void Awake() // WIP code on a timer system
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
		}
		gameState = GameState.Instance;
		turnTimer = (time.minutes * MinuteHolder.secondsInMinute) + time.seconds;
		playerTimer = turnTimer;
		opponentTimer = turnTimer;
	}

	void Update()
    {
		if (!gameState.IsOnline) return;

		if (gameState.HasPriority)
		{
			playerTimer -= Time.deltaTime;
		}
		else
		{
			opponentTimer -= Time.deltaTime;
		}
    }

	/*
	public void AddTime(bool isPlayer, float time) 
	{
		if (isPlayer)
		{
			playerTimer += time;
		}
		else
		{
			opponentTimer += time;
		}
	}

	public void ResetTimer(bool isPlayer)
	{
		if (isPlayer)
		{
			playerTimer = turnTimer;
		}
		else
		{
			opponentTimer = turnTimer;
		}
	}
	*/

}

[Serializable]
public struct MinuteHolder
{
	public const int secondsInMinute = 60;
	public int minutes;
	public int seconds;
}
