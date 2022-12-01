using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86.Avx;

public class GameState : MonoBehaviour
{

    private int amountOfCardsToStartWith = 5;

    [Header("Active Champions")]
    public AvailableChampion playerChampion;
    public AvailableChampion opponentChampion;

    [Header("ChampionLists")]
    public List<AvailableChampion> playerChampions = new List<AvailableChampion>();
    public List<AvailableChampion> opponentChampions = new List<AvailableChampion>();

    [Header("LandmarkLists")]
    public List<LandmarkDisplay> playerLandmarks = new List<LandmarkDisplay>();
    public List<LandmarkDisplay> opponentLandmarks = new List<LandmarkDisplay>();

    public List<Effects> playerEffects = new List<Effects>();
    public List<Effects> opponentEffects = new List<Effects>();
    public List<Effects> removeEffects = new List<Effects>();

    [Header("Have Friends?")]
    public bool isOnline = false;

    [Header("CardsPlayed")]
    public List<Card> cardsPlayedThisTurn = new List<Card>();

    [Header("Win Screen")]
    [SerializeField] private GameObject lostScreen;
    [SerializeField] private GameObject wonScreen;

    [Header("Effect")]
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject yourTurnEffect;


    [Header("UI Elements")]
    public GameObject playedCardGO;
    public UnityEngine.UI.Button endTurnBttn;

    [SerializeField] private GameObject cardRegisterPrefab;

    [NonSerialized] public int currentPlayerID = 0;
    public bool hasPriority = true;

    [NonSerialized] public bool isItMyTurn;
    [NonSerialized] public bool didIStart;
    [NonSerialized] public bool canSwap;
    public int amountOfTurns;

    [NonSerialized] public GameObject targetingEffect;

    [NonSerialized] public int landmarkEffect = 1;
    [NonSerialized] public int attacksPlayedThisTurn;

    private bool firstTurn = true;
    private Calculations calculations;

    private ActionOfPlayer actionOfPlayer;

    private static GameState instance;
    public static GameState Instance { get; set; }



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

        if (ClientConnection.Instance != null)
            isOnline = true;

        DontDestroyOnLoad(this);
    }


    void Start()
    {
        if (CardRegister.Instance == null)
            Instantiate(cardRegisterPrefab, transform.parent);
        actionOfPlayer = ActionOfPlayer.Instance;
        calculations = Calculations.Instance;
        if (isOnline)
        {
            if (ClientConnection.Instance.playerId == 0)
            {
                isItMyTurn = true;
                didIStart = true;
            }
            else
            {
                isItMyTurn = false;
                didIStart = false;
                ChangeInteractabiltyEndTurn();
            }
            AddChampions(Setup.Instance.myChampions, true);
            AddChampions(Setup.Instance.opponentChampions, false);
            Deck.Instance.CreateDecks(Setup.Instance.playerDeckList);
        }
        else
        {
            isItMyTurn = true;
            List<string> ha = new List<string>
            {
                "TheOneWhoDraws",
                "Duelist",
                "Graverobber"
            };
            AddChampions(ha, true);
            AddChampions(ha, false);
        }
        playerChampion = playerChampions[0];
        opponentChampion = opponentChampions[0];

        DrawStartingCards();
        //Refresh();
    }

    private void ChangeInteractabiltyEndTurn()
    {
        endTurnBttn.interactable = !endTurnBttn.interactable;
    }

    public void EndTurnButtonClick()
    {
        if (!hasPriority) return;

        if (isOnline)
        {
            RequestEndTurn request = new RequestEndTurn();
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }

        PassPriority();

        EndTurn();
    }

    public void CalculateAndDealDamage(int damage, Card cardUsed)
    {
        damage = calculations.CalculateDamage(damage);
        DealDamage(calculations.TargetAndAmountFromCard(cardUsed, damage));

        if (playerChampion.animator != null)
        {
            playerChampion.animator.SetTrigger("Attack");
        }
    }

    public void DealDamage(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE = targetAndAmount.targetInfo.whichList;
        if (isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestDamage request = new RequestDamage(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }

        if (targetAndAmount.targetInfo.index == -1)
        {
            print("ERROR INDEX -1, YOU STUPID");
        }

        if (lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
            foreach (Effects effect in playerEffects)
            {
                effect.TakeDamage(targetAndAmount.amount);
            }
        }
        if (lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }
        if (lE.myLandmarks)
        {
            playerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }
        if (lE.opponentLandmarks)
        {
            opponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }
    }

    public void ChangeLandmarkStatus(TargetInfo targetInfo, bool enable) // TargetAndAmount
    {
        ListEnum lE = targetInfo.whichList;
        if (lE.myLandmarks && !enable)
        {
            playerLandmarks[targetInfo.index].DisableLandmark();
        }
        else if (lE.myLandmarks && enable)
        {
            playerLandmarks[targetInfo.index].EnableLandmark();
        }

        if (lE.opponentLandmarks && !enable)
        {
            opponentLandmarks[targetInfo.index].DisableLandmark();
        }
        else if (lE.myLandmarks && enable)
        {
            playerLandmarks[targetInfo.index].EnableLandmark();
        }
    }

    public void CalculateAndHeal(int amount, Card cardUsed)
    {
        amount = calculations.CalculateHealing(amount);
        Invoke(nameof(TakeAwayHealEffect), 3f);
        HealTarget(calculations.TargetAndAmountFromCard(cardUsed, amount));
    }

    private void TakeAwayHealEffect()
    {
        healEffect.SetActive(false);
    }

    public void HealTarget(TargetAndAmount targetAndAmount) // TargetAndAmount
    {
        ListEnum lE =  targetAndAmount.targetInfo.whichList;
        
        if(lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].HealChampion(targetAndAmount.amount);
            EffectController.Instance.GainHealingEffect(playerChampions[targetAndAmount.targetInfo.index].gameObject);

        }
        if (lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].HealChampion(targetAndAmount.amount);
            EffectController.Instance.GainHealingEffect(opponentChampions[targetAndAmount.targetInfo.index].gameObject);
        }

        if (isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestHealing request = new RequestHealing(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }
    }

    public void CalculateAndShield(int amount, Card cardUsed)
    {
        amount = calculations.CalculateShield(amount);
        ShieldTarget(calculations.TargetAndAmountFromCard(cardUsed, amount));
    }


    public void ShieldTarget(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE = targetAndAmount.targetInfo.whichList;
        print("vilket index shielding" + targetAndAmount.targetInfo.index);

        if (lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].GainShield(targetAndAmount.amount);
            Tuple<string, bool> tuple = new Tuple<string, bool>(playerChampions[targetAndAmount.targetInfo.index].champion.championName, false);
            EffectController.Instance.ActiveShield(tuple, targetAndAmount.amount, playerChampions[targetAndAmount.targetInfo.index].gameObject);
        }
        if (lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].GainShield(targetAndAmount.amount);
            Tuple<string, bool> tuple = new Tuple<string, bool>(opponentChampions[targetAndAmount.targetInfo.index].champion.championName, true);
            EffectController.Instance.ActiveShield(tuple, targetAndAmount.amount, opponentChampions[targetAndAmount.targetInfo.index].gameObject);
        }

        if (isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestShield request = new RequestShield(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }
    }


    private void AddChampions(List<string> champions, bool isPlayer)
    {
        Dictionary<string, Champion> champReg = CardRegister.Instance.champRegister;
        for (int i = 0; i < champions.Count; i++)
        {
            Champion champ = null;
            switch (champions[i])
            {
                case "Cultist":
                    champ = new Cultist((Cultist)champReg[champions[i]]);
                    break;

                case "Builder":
                    champ = new Builder((Builder)champReg[champions[i]]);
                    break;

                case "Shanker":
                    champ = new Shanker((Shanker)champReg[champions[i]]);
                    break;

                case "Graverobber":
                    champ = new Graverobber((Graverobber)champReg[champions[i]]);
                    break;

                case "TheOneWhoDraws":
                    champ = new TheOneWhoDraws((TheOneWhoDraws)champReg[champions[i]]);
                    break;

                case "Duelist":
                    champ = new Duelist((Duelist)champReg[champions[i]]);
                    break;
            }
			if (isPlayer)
			    playerChampions[i].champion = champ;
            else
                opponentChampions[i].champion = champ;
        }
    }


    //Move all showedplayedcard to new script with UI elements
    public void ShowPlayedCardLandmark(Landmarks landmark)
    {
        playedCardGO.SetActive(true);
        CardDisplay cardDisp = playedCardGO.GetComponent<CardDisplay>();
        cardDisp.card = landmark;
        cardDisp.manaCost = landmark.maxManaCost;
        cardDisp.UpdateTextOnCard();

        StopCoroutine(HideCardPlayed());
        StartCoroutine(HideCardPlayed());
    }

    public void ShowPlayedCard(Card card)
    {
        playedCardGO.SetActive(true);
        CardDisplay cardDisp = playedCardGO.GetComponent<CardDisplay>();
        cardDisp.card = card;
        cardDisp.manaCost = card.maxManaCost;
        cardDisp.UpdateTextOnCard();

        StopCoroutine(HideCardPlayed());
        StartCoroutine(HideCardPlayed());
    }
    private IEnumerator HideCardPlayed()
    {
        yield return new WaitForSeconds(3f);
        playedCardGO.GetComponent<CardDisplay>().card = null;
        playedCardGO.SetActive(false);
    }

    public void DestroyLandmark(TargetInfo targetInfo)
    {
        if (targetInfo.whichList.opponentLandmarks)
            Graveyard.Instance.AddCardToGraveyardOpponent(opponentLandmarks[targetInfo.index].card);
        else
            Graveyard.Instance.AddCardToGraveyard(playerLandmarks[targetInfo.index].card);

        playerLandmarks[targetInfo.index].card = null;
    }

    public void DiscardCard(int amountToDiscard, bool discardCardsYourself)
    {

        if (isOnline)
        {
            if (discardCardsYourself)
            {
                ListEnum listEnum = new ListEnum();
                listEnum.myHand = true;
                Choice.Instance.ChoiceMenu(listEnum, amountToDiscard, WhichMethod.discardCard, null);
            }
            else
            {
                RequestOpponentDiscardCard requesten = new RequestOpponentDiscardCard();
                requesten.whichPlayer = ClientConnection.Instance.playerId;
                requesten.amountOfCardsToDiscard = amountToDiscard;
                requesten.isRandom = false;
                ClientConnection.Instance.AddRequest(requesten, RequestEmpty);

                PassPriority();
            }
        }
        else
        {
            for (int i = 0; i < amountToDiscard; i++)
            {
				actionOfPlayer.DiscardWhichCard(discardCardsYourself);
            }
        }
    }

    public void DrawCard(int amountToDraw, Card specificCard)
    {
        if (isOnline)
        {
            RequestDrawCard request = new RequestDrawCard(amountToDraw);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }
        actionOfPlayer.DrawCardPlayer(amountToDraw, specificCard, true);
    }

    private void DrawStartingCards()
    {
        if (isOnline)
            DrawCard(amountOfCardsToStartWith, null);
        else
        {
            actionOfPlayer.DrawCardPlayer(amountOfCardsToStartWith, null, true);
            actionOfPlayer.DrawCardPlayer(amountOfCardsToStartWith, null, false);
        }
		//actionOfPlayer.DrawCardPlayer(amountOfCardsToStartWith, null, false);
    }

	public void DrawRandomCardFromGraveyard(int amountOfCards)
	{
		Tuple<Card, int> info = Graveyard.Instance.RandomizeCardFromGraveyard();
		actionOfPlayer.DrawCardPlayer(amountOfCards, info.Item1, true);
	}

	public void SwitchMyChampions(TargetInfo targetInfo)
    {
        if (targetInfo.whichList.myChampions)
        {
            Swap(playerChampions, 0, targetInfo.index);
        }
        else if(targetInfo.whichList.opponentChampions)
        {
			Swap(opponentChampions, 0, targetInfo.index);
        }
    }

    public void PassPriority()
    {
        if(hasPriority && isOnline)
        {
            hasPriority = false;
            RequestPassPriority requestPassPriority = new RequestPassPriority(true);
            requestPassPriority.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(requestPassPriority, RequestEmpty);
        }
    }

    public void SwapActiveChampion(Card card)
    {
        ListEnum lE = new ListEnum();
        lE.myChampions = true;
        if (isOnline)
            Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionDied, null);
        else
        {
            if(card)
                Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionPlayer, null);
            else
                Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionDied, null);

        }
    }

    public void SwapChampionWithTargetInfo(TargetInfo targetInfo, bool championDied)
    {
        if (targetInfo.whichList.myChampions == true)
        {
            Swap(opponentChampions, 0, targetInfo.index);
            if (championDied)
                RemoveChampion(opponentChampions[targetInfo.index].champion);
        }
        if (targetInfo.whichList.opponentChampions == true)
        {
            Swap(playerChampions, 0, targetInfo.index);
        }
    }

    public void SwapActiveChampionEnemy()
    {
        if (!isOnline)
        {
            for (int i = 0; i < 25; i++)
            {
                int randomChamp = UnityEngine.Random.Range(0, opponentChampions.Count);
                if (opponentChampion != opponentChampions[randomChamp])
                {
                    Swap(opponentChampions, 0, randomChamp);
                    opponentChampion.champion.WhenCurrentChampion();
                    break;
                }
            }
            
        }
    }

    public void LandmarkPlaced(int index, Landmarks landmark, bool opponentPlayedLandmark)
    {
        if (landmark is HealingLandmark)
        {
            landmark = new HealingLandmark((HealingLandmark)landmark);
        }
        else if (landmark is TauntLandmark)
        {
            landmark = new TauntLandmark((TauntLandmark)landmark);
        }
        else if (landmark is DamageLandmark)
        {
            landmark = new DamageLandmark((DamageLandmark)landmark);
        }
        else if (landmark is DrawCardLandmark)
        {
            landmark = new DrawCardLandmark((DrawCardLandmark)landmark);
        }
        else if (landmark is CultistLandmark)
        {
            landmark = new CultistLandmark((CultistLandmark)landmark);
        }
        else if (landmark is BuilderLandmark)
        {
            landmark = new BuilderLandmark((BuilderLandmark)landmark);
        }
        else if (landmark is SeersShack)
        {
            landmark = new SeersShack((SeersShack)landmark);
        }
        else if (landmark is DisableCardLandmark)
        {
            landmark = new DisableCardLandmark((DisableCardLandmark)landmark);
        }
        else if (landmark is TheOneWhoDrawsLandmark)
        {
            landmark = new TheOneWhoDrawsLandmark((TheOneWhoDrawsLandmark)landmark);
        }
        else if (landmark is DuelistLandmark)
        {
            landmark = new DuelistLandmark((DuelistLandmark)landmark);
        }
 

        if (opponentPlayedLandmark)
        {
            opponentLandmarks[index].card = landmark;
            opponentLandmarks[index].landmark = landmark;
            opponentLandmarks[index].health = landmark.minionHealth;
            opponentLandmarks[index].manaCost = opponentLandmarks[index].card.maxManaCost;
        }
        else
        {
            playerLandmarks[index].card = landmark;
            playerLandmarks[index].landmark = landmark;
            playerLandmarks[index].health = landmark.minionHealth;
            playerLandmarks[index].manaCost = playerLandmarks[index].card.maxManaCost;
        }
    }

    

    public void TriggerUpKeep()
    {
        DrawCard(1, null);

        if (isItMyTurn && !firstTurn || !isOnline)
        {
            actionOfPlayer.IncreaseMana();
        }
        if (didIStart || !isOnline)
        {
            amountOfTurns++;
        }
        playerChampion.champion.UpKeep();
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            if (landmark.card != null && landmark.landmarkEnabled)
                landmark.landmark.UpKeep();
        } 
        foreach (Effects effect in playerEffects)
        {
            effect.UpKeep();
        }
    }

    public void TriggerEndStep()
    {
        playerChampion.champion.EndStep();
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            if(landmark.card != null && landmark.landmarkEnabled)
            landmark.landmark.EndStep();
        }
        foreach (Effects effect in playerEffects)
        {
            effect.EndStep();
        }
        foreach (CardDisplay cardDisplay in actionOfPlayer.handPlayer.cardsInHand)
        {
            cardDisplay.EndStep();
        }
    }


    public void EndTurn()
    {
        if (!isOnline)
        {
            if (yourTurnEffect != null)
            {
                yourTurnEffect.SetActive(true);
                Invoke(nameof(YourTurnEffectHide), 1.5f);
            }
            TriggerEndStep();
            TriggerUpKeep();
			Refresh();
			return;
        }

        if (isItMyTurn)
        {
            isItMyTurn = false;
            firstTurn = false;
            TriggerEndStep();
        }
        else
        {
            if (yourTurnEffect != null)
            {
                yourTurnEffect.SetActive(true);
                Invoke(nameof(YourTurnEffectHide), 1.5f);
            }

            isItMyTurn = true;
            TriggerUpKeep();
        }
        attacksPlayedThisTurn = 0;
        ChangeInteractabiltyEndTurn();
        cardsPlayedThisTurn.Clear();

        Refresh();
    }

    private void YourTurnEffectHide()
    {
        yourTurnEffect.SetActive(false);
    }

    public void AddCardToPlayedCardsThisTurn(CardDisplay cardPlayed)
    {
        Card card = cardPlayed.card;
        cardsPlayedThisTurn.Add(cardPlayed.card);

        if (card.typeOfCard == CardType.Attack)
        {
            attacksPlayedThisTurn++;
        }
        playerChampion.champion.AmountOfCardsPlayed(card);
        actionOfPlayer.ChangeCardOrder(true, cardPlayed);
    }

    public void ChampionDeath(Champion deadChampion)
    {
        SearchDeadChampion(deadChampion);
        if (playerChampions.Count == 0)
        {
            Defeat();
            return;
        }
        else if (opponentChampions.Count == 0)
        {
            Victory();
            return;
        }
    }

    private void SearchDeadChampion(Champion deadChampion)
    {
        if (playerChampion.champion == deadChampion)
        {
            SwapActiveChampion(null);
        }
        else if (!isOnline && opponentChampion.champion == deadChampion)
        {          
            SwapActiveChampionEnemy();
            RemoveChampion(deadChampion);
        }

		if (isOnline && opponentChampion.champion == deadChampion)
		{
            PassPriority();
		}
	}

    public void RemoveChampion(Champion deadChamp)
    {
        foreach (AvailableChampion ac in playerChampions)
        {
            if (ac.champion == deadChamp)
            {
                playerChampions.Remove(ac);
                break;
            }
        }
        foreach (AvailableChampion ac in opponentChampions)
        {
            if (ac.champion == deadChamp)
            {
                opponentChampions.Remove(ac);
                break;
            }
        }
    }

    public void AddEffect(Effects effect)
    {
        playerEffects.Add(effect);
    }

    public void RemoveEffect(Effects effect)
    {
        removeEffects.Add(effect);
    }

    public void ClearEffects()
    {
       foreach (Effects effect in removeEffects)
       {
           playerEffects.Remove(effect);
       }
       removeEffects.Clear();
    }

    public void RequestEmpty(ServerResponse response) {}

    //Victory and defeat should be in another script
    public void Victory()
    {
        wonScreen.SetActive(true);
        //Request defeat maybe????
    }

    public void Defeat()
    {
        lostScreen.SetActive(true);
        //Request victory maybe????
    }

    public static void Swap(List<AvailableChampion> list, int i, int j)
    {
        Champion temp = list[i].champion;
        list[i].champion = list[j].champion;
        list[j].champion = temp;
    }

    public void Refresh()
    {
        ClearEffects();
        print("Clear");
        foreach (LandmarkDisplay landmarkDisplay in playerLandmarks)
        {
            print("landmarks");
            landmarkDisplay.UpdateTextOnCard();
        }
        
        ActionOfPlayer.Instance.handPlayer.FixCardOrderInHand();
        print("bef updPlayer");
        playerChampion.UpdateTextOnCard();
        print("aft updPlayer");
        print("bef updOponent");
        opponentChampion.UpdateTextOnCard();
        print("aft updOponent");
    }
}
