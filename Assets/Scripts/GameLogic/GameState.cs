using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameState : MonoBehaviour
{
    [SerializeField] private int amountOfCardsToStartWith = 5;
    private bool firstTurn = true;
    private Calculations calculations;
    private ActionOfPlayer actionOfPlayer;
    private CardRegister cardRegister;
    private Setup setup;
    private Deck deck;
    private EffectController effectController;

	[SerializeField] private bool mulligan = true;
	[SerializeField] private bool chooseStartChampion = true;

	[Header("Win Screen")]
    [SerializeField] private GameObject lostScreen;
    [SerializeField] private GameObject wonScreen;
    [SerializeField] private ParticleSystem particle1;
    [SerializeField] private ParticleSystem particle2;
    public Animator animator;

    [Header("Effect")]
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject cardRegisterPrefab;
    [SerializeField] private YourTurnEffect yourTurnEffect;

    [Header("EndTurnButton")]
    [SerializeField] private Button endTurnBttn;


    [NonSerialized] public GameObject targetingEffect;

    [NonSerialized] public AvailableChampion playerChampion;
    [NonSerialized] public AvailableChampion opponentChampion;
    [NonSerialized] public List<Effects> removeEffects = new List<Effects>();
    [NonSerialized] public List<Card> cardsPlayedThisTurn = new List<Card>();

    [NonSerialized] public bool canSwap = true;
    [NonSerialized] public bool isOnline = false;
    [NonSerialized] public bool isItMyTurn;
    [NonSerialized] public bool didIStart;

    [NonSerialized] public int currentPlayerID = 0;
    [NonSerialized] public int landmarkEffect = 1;
    [NonSerialized] public int attacksPlayedThisTurn;
    [NonSerialized] public int drawnCardsThisTurn = 0;
    [NonSerialized] public int drawnCardsPreviousTurn = 0;

    [Header("ChampionLists")]
    public List<AvailableChampion> playerChampions = new List<AvailableChampion>();
    public List<AvailableChampion> opponentChampions = new List<AvailableChampion>();

    [Header("LandmarkLists")]
    public List<LandmarkDisplay> playerLandmarks = new List<LandmarkDisplay>();
    public List<LandmarkDisplay> opponentLandmarks = new List<LandmarkDisplay>();

    [Header("EffectLists")]
    public List<Effects> playerEffects = new List<Effects>();
    public List<Effects> opponentEffects = new List<Effects>();

    [Header("ShowPlayedCard")]
    public GameObject playedCardGO;

    [Header("ImportantStuff")]
    public bool hasPriority = true;
    public int amountOfTurns = 1;


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

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        if (CardRegister.Instance == null)
            Instantiate(cardRegisterPrefab, transform.parent);

        actionOfPlayer = ActionOfPlayer.Instance;
        calculations = Calculations.Instance;
        cardRegister = CardRegister.Instance;
        setup = Setup.Instance;
        deck = Deck.Instance;
        effectController = EffectController.Instance;

        if (isOnline)
        {
            if (Setup.Instance.shouldStartGame)
            {
                isItMyTurn = true;
                didIStart = true;
            }
            else
            {
                hasPriority = false;
                isItMyTurn = false;
                didIStart = false;
                ChangeInteractabiltyEndTurn();
            }
            AddChampions(setup.myChampions, true);
            AddChampions(setup.opponentChampions, false);
            InstantiateCardsFromDeck(setup.playerDeckList);

            Deck.Instance.CreateDecks(setup.playerDeckList);
        }
        else
        {
            isItMyTurn = true;
            List<string> ha = new List<string>
            {
                "Shanker",
                "TheOneWhoDraws",
                "Duelist",
                //"Graverobber",
                //"Builder",
                //"Cultist",
            };
            AddChampions(ha, true);
            AddChampions(ha, false);
            InstantiateCardsFromDeck(deck.deckPlayer);
        }
        playerChampion = playerChampions[0];
        opponentChampion = opponentChampions[0];


        if (!didIStart)
            actionOfPlayer.enemyMana++;

        StartMulligan();
    }

    private void StartMulligan()
    {
        DrawStartingCards();

        if (mulligan)
        {
            ListEnum listEnum = new ListEnum();
            listEnum.myHand = true;
            Choice.Instance.ChoiceMenu(listEnum, -1, WhichMethod.Mulligan, null, 0.1f);
        }

        if (chooseStartChampion)
        {
			ListEnum listEnum = new ListEnum();
			listEnum.myChampions = true;
			Choice.Instance.ChoiceMenu(listEnum, 1, WhichMethod.SwitchChampionMulligan, null);
		}
    }

    public void StartGame()
    {
        if (!isItMyTurn)
        {
            //hasPriority = false;
        }
    }

    public void PlayCardRequest(CardAndPlacement cardPlacement)
    {
        CardDisplay cardDisplay =  playedCardGO.GetComponent<CardDisplay>();
        RequestPlayCard playCardRequest = new RequestPlayCard(cardPlacement, cardDisplay.manaCost);
        playCardRequest.whichPlayer = ClientConnection.Instance.playerId;
        ClientConnection.Instance.AddRequest(playCardRequest, RequestEmpty);

        Refresh();
    }


    private void InstantiateCardsFromDeck(List<Card> listOfCards)
    {
        Dictionary<string, Card> cardReg = cardRegister.cardRegister;
        List<Card> listToAdd = new List<Card>();
        for (int i = 0; i < listOfCards.Count; i++)
        {
            Card card = Instantiate(cardRegister.cardRegister[listOfCards[i].cardName]);
            if (isOnline)
            {
                deck.AddCardToDeckPlayer(card);
                deck.AddCardToDeckOpponent(card);
            }
            listToAdd.Add(card);

        }
        if (!isOnline)
        {
            listOfCards.Clear();
            listOfCards.AddRange(listToAdd);
        }
    }
    private void ChangeInteractabiltyEndTurn()
    {
        endTurnBttn.interactable = !endTurnBttn.interactable;
    }

    public void EndTurnButtonClick()
    {   if (Choice.Instance.isChoiceActive) return;
        if (!hasPriority) return;

        if (isOnline)
        {
            RequestEndTurn request = new RequestEndTurn();
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }
        AudioManager.Instance.PlayClickSound();

        PassPriority();

        EndTurn();
    }

    public void CalculateAndDealDamage(int damage, Card cardUsed)
    {
        damage = calculations.CalculateDamage(damage, false);
        DealDamage(calculations.TargetAndAmountFromCard(cardUsed, damage));

        if (playerChampion.animator != null)
        {
            playerChampion.animator.SetTrigger("Attack");
        }
        EffectController.Instance.PlayAttackEffect(playerChampion);
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
        amount = calculations.CalculateHealing(amount, false);
        Invoke(nameof(TakeAwayHealEffect), 3f);
        HealTarget(calculations.TargetAndAmountFromCard(cardUsed, amount));

        AudioManager.Instance.PlayHealSound();
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
        amount = calculations.CalculateShield(amount, false);
        ShieldTarget(calculations.TargetAndAmountFromCard(cardUsed, amount));
    }


    public void ShieldTarget(TargetAndAmount targetAndAmount) // TargetAndAmount
    {
        ListEnum lE = targetAndAmount.targetInfo.whichList;
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
        Dictionary<string, Champion> champReg = cardRegister.champRegister;
        for (int i = 0; i < champions.Count; i++)
        {
            Champion champ = Instantiate(champReg[champions[i]]);
       
			if (isPlayer)
			    playerChampions[i].champion = champ;
            else
                opponentChampions[i].champion = champ;
        }
    }

    public void ShowPlayedCard(Card card, bool opponent, int manaCost)
    {
        playedCardGO.SetActive(true);
        CardDisplay cardDisp = playedCardGO.GetComponent<CardDisplay>();
        CardDisplayAttributes cardDisplayAttributes = playedCardGO.transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        if (manaCost != -1)
            cardDisp.manaCost = manaCost;

        cardDisp.card = card;
        cardDisplayAttributes.previewCard = opponent;
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
                Choice.Instance.ChoiceMenu(listEnum, amountToDiscard, WhichMethod.DiscardCard, null);
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
            if (discardCardsYourself)
            {
                ListEnum listEnum = new ListEnum();
                listEnum.myHand = true;
                Choice.Instance.ChoiceMenu(listEnum, amountToDiscard, WhichMethod.DiscardCard, null);
            }
            else
            {
                for (int i = 0; i < amountToDiscard; i++)
                {
                    actionOfPlayer.DiscardWhichCard(discardCardsYourself);
                }
            }
        }
    }

    public void DrawCard(int amountToDraw, Card specificCard, bool isPlayer)
    {
        if (isOnline && isPlayer)
        {
            RequestDrawCard request = new RequestDrawCard(amountToDraw);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }
        actionOfPlayer.DrawCardPlayer(amountToDraw, specificCard, isPlayer);
    }
    public void DrawCard(int amountToDraw, Card specificCard)
    {
        DrawCard(amountToDraw, specificCard, true);    
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

    public void SwapOnDeath(AvailableChampion champ)
    {
        if (!isOnline && champ.isOpponent)
        {
            for (int i = 0; i < 100; i++)
            {
                
                int randomChamp = UnityEngine.Random.Range(0, opponentChampions.Count);
                if (opponentChampion != opponentChampions[randomChamp])
                {
                    Swap(opponentChampions, 0, randomChamp);
                    opponentChampion.champion.WhenCurrentChampion();
                    return;
                }
            }
        }

        ListEnum lE = new ListEnum();
        lE.myChampions = true;
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionDied, null);
    }

    public void SwapChampionWithTargetInfo(TargetInfo targetInfo, bool championDied)
    {
        if (targetInfo.whichList.myChampions == true)
        {
            Swap(playerChampions, 0, targetInfo.index);

            if (championDied)
                RemoveChampion(playerChampions[targetInfo.index].champion);
        }
        if (targetInfo.whichList.opponentChampions == true)
        {
            Swap(opponentChampions, 0, targetInfo.index);
        }
    }

    public void SwapChampionOnline(TargetInfo targetInfo, bool championDied)
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


    public void LandmarkPlaced(int index, Landmarks landmark, bool opponentPlayedLandmark)
    {
        landmark = Instantiate(cardRegister.landmarkRegister[landmark.cardName]);
        ShowPlayedCard(landmark, opponentPlayedLandmark, -1);
        
        if (opponentPlayedLandmark)
        {
            opponentLandmarks[index].card = landmark;
            opponentLandmarks[index].transform.GetChild(0).gameObject.SetActive(true);
            opponentLandmarks[index].health = landmark.minionHealth;
            opponentLandmarks[index].manaCost = opponentLandmarks[index].card.maxManaCost;
        }
        else
        {
            playerLandmarks[index].card = landmark;
            playerLandmarks[index].transform.GetChild(0).gameObject.SetActive(true);
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

        if (firstTurn)
        {
            amountOfTurns++;
        }

        playerChampion.champion.UpKeep();

        foreach (LandmarkDisplay landmarkDisplay in playerLandmarks)
        {
            if (landmarkDisplay.card != null && landmarkDisplay.landmarkEnabled)
            {
                Landmarks landmark = (Landmarks)landmarkDisplay.card;
                landmark.UpKeep();
            }
        } 
        foreach (Effects effect in playerEffects)
        {
            effect.UpKeep();
        }
    }

    public void TriggerEndStep()
    {   
        if(!isOnline)
        {
            DrawCard(1, null,false);
        }

		if (!firstTurn)
		{
			amountOfTurns++;
		}

		playerChampion.champion.EndStep();

        foreach (LandmarkDisplay landmarkDisplay in playerLandmarks)
        {
            if(landmarkDisplay.card != null && landmarkDisplay.landmarkEnabled)
            {
                Landmarks landmark = (Landmarks)landmarkDisplay.card;
                landmark.EndStep();
            }
        }
        foreach (Effects effect in playerEffects)
        {
            effect.EndStep();
        }
        foreach (CardDisplay cardDisplay in actionOfPlayer.handPlayer.cardsInHand)
        {
            cardDisplay.EndStep();
        }

        actionOfPlayer.enemyMana = actionOfPlayer.playerMana;
        actionOfPlayer.UpdateUnspentMana();
    }


    public void EndTurn()
    {
        if (!isOnline)
        {
            TriggerEndStep();
            TriggerUpKeep();
            yourTurnEffect.ActivateEffect();
			actionOfPlayer.roundCounter.text = "Round: " + amountOfTurns;
			Refresh();
			return;
        }

        if (isItMyTurn)
        {
            isItMyTurn = false;
            TriggerEndStep();
            firstTurn = false;
        }
        else
        {       
            isItMyTurn = true;
            TriggerUpKeep();
            yourTurnEffect.ActivateEffect();
        }

        actionOfPlayer.roundCounter.text = "Round: " + amountOfTurns;
        attacksPlayedThisTurn = 0;
        ChangeInteractabiltyEndTurn();
        cardsPlayedThisTurn.Clear();

        drawnCardsPreviousTurn = drawnCardsThisTurn;
        drawnCardsThisTurn = 0;
        Refresh();
    }

 

    public void AddCardToPlayedCardsThisTurn(Card cardPlayed)
    {
        cardsPlayedThisTurn.Add(cardPlayed);

        if (cardPlayed.typeOfCard == CardType.Attack)
        {
            attacksPlayedThisTurn++;
        }
        playerChampion.champion.AmountOfCardsPlayed(cardPlayed);
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
            SwapOnDeath(playerChampion);
        }

        if (opponentChampion.champion == deadChampion)
        {
            if (!isOnline)
            {
                SwapOnDeath(opponentChampion);
                RemoveChampion(deadChampion);
            }
            else
                PassPriority();
        }
	}

    private void RemoveChampion(Champion deadChamp)
    {
        foreach (AvailableChampion ac in playerChampions)
        {
            if (ac.champion == deadChamp)
            {
                ac.UpdateTextOnCard();
                playerChampions.Remove(ac);
                break;
            }
        }
        foreach (AvailableChampion ac in opponentChampions)
        {
            if (ac.champion == deadChamp)
            {
                ac.UpdateTextOnCard();
                opponentChampions.Remove(ac);
                break;
            }
        }
    }

    public void AddEffect(Effects effect)
    {
        effect.AddEffect();
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
        particle1.Play();
        particle2.Play();
        

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
        foreach (LandmarkDisplay landmarkDisplay in playerLandmarks)
        {
            if (landmarkDisplay.card != null)
                landmarkDisplay.UpdateTextOnCard();
        }
        foreach (LandmarkDisplay landmarkDisplay in opponentLandmarks)
        {
            if (landmarkDisplay.card != null)
                landmarkDisplay.UpdateTextOnCard();
        }
        
        ActionOfPlayer.Instance.handPlayer.FixCardOrderInHand();
        ActionOfPlayer.Instance.handOpponent.FixCardOrderInHand();

        foreach (AvailableChampion aC in playerChampions)
        {
            aC.UpdateTextOnCard();
        }
        foreach (AvailableChampion aC in opponentChampions)
        {
            aC.UpdateTextOnCard();
        }


        WhichManaCrystalsToShow.Instance.UpdateManaCrystals();
        yourTurnEffect.ChangePicture(playerChampion);


        if (playerChampions.Count <= 0)
        {
            
            Defeat();
        }
           
        else if (opponentChampions.Count == 1 && opponentChampion.champion.health <= 0)
        {
            
            /*Victory();*/
        }
    }
}
