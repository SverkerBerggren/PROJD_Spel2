using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculations : MonoBehaviour
{
	private GameState gameState;
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
		gameState = GameState.Instance;
	}

	private int LookForChampionIndex(Card cardUsed, List<AvailableChampion> champ)
	{
		for (int i = 0; i < champ.Count; i++)
		{
			if (champ[i].Champion == cardUsed.Target)
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

	public int CalculateDamage(int baseDamage, bool justBaseDmg)
	{
		if (justBaseDmg)
			return baseDamage;

		baseDamage = gameState.PlayerChampion.Champion.DealDamageAttack(baseDamage);
		foreach (LandmarkDisplay landmarkDisplay in gameState.PlayerLandmarks)
		{
			if (landmarkDisplay.Card != null && landmarkDisplay.LandmarkEnabled)
			{
				Landmarks landmark = (Landmarks)landmarkDisplay.Card;
                baseDamage = landmark.DealDamageAttack(baseDamage);
			}
		}
		foreach (Effects effect in gameState.PlayerEffects)
		{
			baseDamage = effect.DealDamageAttack(baseDamage);
		}
		return baseDamage;
	}
	public int CalculateHealing(int amount, bool justBaseHealing)
	{
		if (justBaseHealing)
			return amount;
		foreach (LandmarkDisplay landmarkDisplay in gameState.PlayerLandmarks)
		{
			if (landmarkDisplay.Card != null && landmarkDisplay.LandmarkEnabled)
			{
                Landmarks landmark = (Landmarks)landmarkDisplay.Card;
				amount = landmark.HealingEffect(amount);
            }
		}
		foreach (Effects effect in gameState.PlayerEffects)
		{
			amount = effect.HealingEffect(amount);
		}
		return amount;
	}
	public int CalculateShield(int amount, bool justBaseShield)
	{
		if (justBaseShield)
			return amount;
		foreach (LandmarkDisplay landmarkDisplay in gameState.PlayerLandmarks)
		{
			if (landmarkDisplay.Card != null && landmarkDisplay.LandmarkEnabled)
			{
                Landmarks landmark = (Landmarks)landmarkDisplay.Card;
                amount = landmark.ShieldingEffect(amount);
			}
		}
		foreach (Effects effect in gameState.PlayerEffects)
		{
			amount = effect.ShieldingEffect(amount);
		}
		return amount;
	}
	public void CalculateHandManaCost(CardDisplay cardDisplay)
	{
        cardDisplay.ManaCost = cardDisplay.Card.MaxManaCost;
        cardDisplay.ManaCost = gameState.PlayerChampion.Champion.CalculateManaCost(cardDisplay);
			
        foreach (LandmarkDisplay landmarkDisplay in gameState.PlayerLandmarks)
		{
			if (landmarkDisplay.Card != null && landmarkDisplay.LandmarkEnabled)
			{
                Landmarks landmark = (Landmarks)landmarkDisplay.Card;
                cardDisplay.ManaCost = landmark.CalculateManaCost(cardDisplay);
			}
		}

		foreach (Effects effect in gameState.PlayerEffects)
		{
			cardDisplay.ManaCost = effect.CalculateManaCost(cardDisplay);
		}

		if (cardDisplay.ManaCost <= 0)
			cardDisplay.ManaCost = 0;
		else if (cardDisplay.ManaCost > 10)
			cardDisplay.ManaCost = 10;
	}

	public TargetAndAmount TargetAndAmountFromCard(Card cardUsed, int amount)
	{
		TargetAndAmount tAA = null;
		TargetInfo tI = null;
		ListEnum listEnum = new ListEnum();
		int index = 0;
		if (cardUsed.Target != null) // Calculates the target based on champions
		{
			index = LookForChampionIndex(cardUsed, gameState.OpponentChampions);
			if (index == -1) // if target is not an enemychampion
			{
				index = LookForChampionIndex(cardUsed, gameState.PlayerChampions);
				listEnum.myChampions = true;
			}
			else
				listEnum.opponentChampions = true;
		}
		else if (cardUsed.LandmarkTarget != null) // Calculates the target based on landmarks
		{
			index = LookForLandmarkIndex(cardUsed, gameState.OpponentLandmarks); 
			if (index == -1) // if target is not an enemylandmark
			{
				index = LookForLandmarkIndex(cardUsed, gameState.PlayerLandmarks);
				listEnum.myLandmarks = true;
			}
			else
				listEnum.opponentLandmarks = true;
		}
		tI = new TargetInfo(listEnum, index);
		tAA = new TargetAndAmount(tI, amount);
		return tAA;
	}
}
