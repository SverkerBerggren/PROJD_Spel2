using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculations : MonoBehaviour
{
	private GameState gameState;
	private ActionOfPlayer actionOfPlayer;
	private static Calculations instance;
	public static Calculations Instance { get { return instance; } set { instance = value; } }
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		gameState = GameState.Instance;
		actionOfPlayer = ActionOfPlayer.Instance;

    }

	public int CalculateDamage(int baseDamage)
	{
		baseDamage = gameState.playerChampion.champion.DealDamageAttack(baseDamage);
		foreach (LandmarkDisplay landmark in gameState.playerLandmarks)
		{
			if (landmark.card != null)
				baseDamage = landmark.card.DealDamageAttack(baseDamage);
		}
		return baseDamage;
	}

	public int CalculateHealing(int amount)
	{
		foreach (LandmarkDisplay landmark in gameState.playerLandmarks)
		{
			if (landmark.card == null) continue;
			amount = landmark.card.HealingEffect(amount);
		}
		return amount;
	}

	public int CalculateShield(int amount)
	{
		foreach (LandmarkDisplay landmark in gameState.playerLandmarks)
		{
			if (landmark.card == null) continue;
			amount = landmark.card.ShieldingEffect(amount);
		}
		return amount;
	}

	public void CalculateHandManaCost(CardDisplay cardDisplay)
	{
		cardDisplay.manaCost = cardDisplay.card.maxManaCost;
        cardDisplay.manaCost = gameState.playerChampion.champion.CalculateManaCost(cardDisplay);
			
        foreach (LandmarkDisplay landmarkDisplay in gameState.playerLandmarks)
		{
			if (landmarkDisplay.card == null) continue;
            cardDisplay.manaCost = landmarkDisplay.card.CalculateManaCost(cardDisplay);
				
		}

		//ContinousEffects

		if (cardDisplay.manaCost <= 0)
			cardDisplay.manaCost = 0;
		else if (cardDisplay.manaCost > 10)
			cardDisplay.manaCost = 10;
        
	}

	public TargetAndAmount TargetAndAmountFromCard(Card cardUsed, int amount)
	{
		TargetAndAmount tAA = null;
		TargetInfo tI = null;
		ListEnum listEnum = new ListEnum();
		int index = 0;
		// WIP
		if (cardUsed.Target != null)
		{
			index = LookForChampionIndex(cardUsed, gameState.opponentChampions);
			if (index == -1)
			{
				index = LookForChampionIndex(cardUsed, gameState.playerChampions);
				listEnum.myChampions = true;
			}
			else
			{
				listEnum.opponentChampions = true;
			}
		}
		else if (cardUsed.LandmarkTarget != null)
		{
			index = LookForLandmarkIndex(cardUsed, gameState.opponentLandmarks);
			if (index == -1)
			{
				index = LookForLandmarkIndex(cardUsed, gameState.playerLandmarks);
				listEnum.myLandmarks = true;
			}
			else
			{
				listEnum.opponentLandmarks = true;
			}
		}
		tI = new TargetInfo(listEnum, index);
		tAA = new TargetAndAmount(tI, amount);
		return tAA;
	}


	private int LookForChampionIndex(Card cardUsed, List<AvailableChampion> champ)
	{
		for (int i = 0; i < champ.Count; i++)
		{
			if (champ[i].champion == cardUsed.Target)
			{
				return i;
			}
		}
		return -1;
	}
	private int LookForLandmarkIndex(Card cardUsed, List<LandmarkDisplay> landmarks)
	{
		for (int i = 0; i < landmarks.Count; i++)
		{
			if (landmarks[i] == cardUsed.LandmarkTarget)
			{
				return i;
			}
		}
		return -1;
	}
}
