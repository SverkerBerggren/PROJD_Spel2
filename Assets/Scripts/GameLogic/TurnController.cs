using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
	private GameState gameState;
	public float playerTimer;
	public float opponentTimer;
	public MinuteHolder turnTimer;


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
		gameState = GameState.Instance;
		playerTimer = (turnTimer.minutes * MinuteHolder.secondsInMinute) + turnTimer.seconds;
		opponentTimer = (turnTimer.minutes * MinuteHolder.secondsInMinute) + turnTimer.seconds;
	}

	void Update()
    {
		//if (!gameState.isOnline) return;

		if (gameState.hasPriority)
		{
			playerTimer -= Time.deltaTime;
		}
		else
		{
			opponentTimer -= Time.deltaTime;
		}
    }

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
}

[Serializable]
public struct MinuteHolder
{
	public const int secondsInMinute = 60;
	public int minutes;
	public int seconds;
}
