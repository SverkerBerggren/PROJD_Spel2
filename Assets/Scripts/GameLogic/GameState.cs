using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameState : MonoBehaviour
{
    private bool firstTurn = true;
    private Calculations calculations;
    private ActionOfPlayer actionOfPlayer;
    private CardRegister cardRegister;
    private Setup setup;
    private Deck deck;
    private EffectController effectController;
    private AudioManager audioManager;
    private Choice choice;
    private Graveyard graveyard;

    private bool didIStart;
    private List<Card> cardsPlayedThisTurn = new List<Card>();
    private List<Effects> removeEffects = new List<Effects>();

    [SerializeField] private int amountOfCardsToStartWith = 5;
	[SerializeField] private bool mulligan = true;
	[SerializeField] private bool chooseStartChampion = true;

	[Header("Win Screen")]
    [SerializeField] private GameObject lostScreen;
    [SerializeField] private GameObject wonScreen;
    [SerializeField] private ParticleSystem confettiOne;
    [SerializeField] private ParticleSystem confettiTwo;
    public Animator cameraAnimator;

    [Header("ShowPlayedCard")]
	[SerializeField] private GameObject playedCardGO;

    [Header("Effect")]
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject cardRegisterPrefab;
    [SerializeField] private YourTurnEffect yourTurnEffect;

    [Header("EndTurnButton")]
    [SerializeField] private Button endTurnBttn;

    [NonSerialized] public GameObject TargetingEffect;

    [NonSerialized] public AvailableChampion PlayerChampion;
    [NonSerialized] public AvailableChampion OpponentChampion;

    [NonSerialized] public bool CanSwap = true;
    [NonSerialized] public bool IsOnline = false;
    [NonSerialized] public bool IsItMyTurn;

    [NonSerialized] public int AttacksPlayedThisTurn;
    [NonSerialized] public int DrawnCardsThisTurn = 0;
    [NonSerialized] public int DrawnCardsPreviousTurn = 0;


    [Header("ChampionLists")]
    public List<AvailableChampion> PlayerChampions = new List<AvailableChampion>();
    public List<AvailableChampion> OpponentChampions = new List<AvailableChampion>();

    [Header("LandmarkLists")]
    public List<LandmarkDisplay> PlayerLandmarks = new List<LandmarkDisplay>();
    public List<LandmarkDisplay> OpponentLandmarks = new List<LandmarkDisplay>();

    [Header("EffectLists")]
    public List<Effects> PlayerEffects = new List<Effects>();
    public List<Effects> OpponentEffects = new List<Effects>();


    [Header("ImportantStuff")]
    public bool HasPriority = true;
    public int AmountOfTurns = 1;

	private static GameState instance;
    public static GameState Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (ClientConnection.Instance != null)
            IsOnline = true;

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
		audioManager = AudioManager.Instance;
		choice = Choice.Instance;
		graveyard = Graveyard.Instance;

		if (IsOnline)
        {
            if (setup.shouldStartGame)
            {
                IsItMyTurn = true;
                didIStart = true;
            }
            else
            {
                HasPriority = false;
                IsItMyTurn = false;
                didIStart = false;
                ChangeInteractabiltyEndTurn();
            }
            AddChampions(setup.myChampions, true);
            AddChampions(setup.opponentChampions, false);
            InstantiateCardsFromDeck(setup.playerDeckList);

            deck.CreateDecks(setup.playerDeckList);
        }
        else
        {
            IsItMyTurn = true;
            List<string> offlineChamps = new List<string>
            {
                "Shanker",
                //"TheOneWhoDraws",
                //"Duelist",
                "Graverobber",
                "Builder",
                //"Cultist",
            };
            AddChampions(offlineChamps, true);
            AddChampions(offlineChamps, false);
            InstantiateCardsFromDeck(deck.DeckPlayer);
        }
        PlayerChampion = PlayerChampions[0];
        OpponentChampion = OpponentChampions[0];


        if (!didIStart)
            actionOfPlayer.EnemyMana++;

        StartMulligan();
    }

    private void StartMulligan()
    {
        DrawStartingCards();

        if (mulligan)
        {
            ListEnum listEnum = new ListEnum();
            listEnum.myHand = true;
            choice.ChoiceMenu(listEnum, -1, WhichMethod.Mulligan, null, 0.1f);
        }

        if (chooseStartChampion)
        {
			ListEnum listEnum = new ListEnum();
			listEnum.myChampions = true;
			choice.ChoiceMenu(listEnum, 1, WhichMethod.SwitchChampionMulligan, null);
		}
    }

    /*
    public void StartGame() // Test with duelist and mulligan
    {
        if (!isItMyTurn)
        {
            //hasPriority = false;
        }
    }
    */

    public void PlayCardRequest(CardAndPlacement cardPlacement)
    {
        CardDisplay cardDisplay =  playedCardGO.GetComponent<CardDisplay>();
        RequestPlayCard playCardRequest = new RequestPlayCard(cardPlacement, cardDisplay.ManaCost);
        playCardRequest.whichPlayer = ClientConnection.Instance.playerId;
        ClientConnection.Instance.AddRequest(playCardRequest, RequestEmpty);

        Refresh();
    }

    private void InstantiateCardsFromDeck(List<Card> listOfCards) // Makes copies of the scriptable objects
    {
        List<Card> listToAdd = new List<Card>();
        for (int i = 0; i < listOfCards.Count; i++)
        {
            Card card = Instantiate(cardRegister.cardRegister[listOfCards[i].CardName]);
            if (IsOnline)
            {
                deck.AddCardToDeckPlayer(card);
                deck.AddCardToDeckOpponent(card);
            }
            listToAdd.Add(card);

        }
        if (!IsOnline)
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
    {  
        if (choice.IsChoiceActive || !HasPriority) return;

        if (IsOnline)
        {
            RequestEndTurn request = new RequestEndTurn();
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }

        audioManager.PlayClickSound();
        PassPriority();
        EndTurn();
    }

    public void CalculateAndDealDamage(int damage, Card cardUsed)
    {
        damage = calculations.CalculateDamage(damage, false);
        DealDamage(calculations.TargetAndAmountFromCard(cardUsed, damage));

        if (PlayerChampion.Animator != null)
            PlayerChampion.Animator.SetTrigger("Attack");

        effectController.PlayAttackEffect(PlayerChampion);
    }

    public void DealDamage(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE = targetAndAmount.targetInfo.whichList;
        if (IsOnline)
        {
			List<TargetAndAmount> list = new List<TargetAndAmount>{ targetAndAmount };
			RequestDamage request = new RequestDamage(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEmpty);
        }

        if (targetAndAmount.targetInfo.index == -1)
            throw new ArgumentOutOfRangeException();

        if (lE.myChampions)
        {
            PlayerChampions[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
            foreach (Effects effect in PlayerEffects)
            {
                effect.TakeDamage(targetAndAmount.amount);
            }
        }

        if (lE.opponentChampions)
            OpponentChampions[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);

        if (lE.myLandmarks)
            PlayerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);

        if (lE.opponentLandmarks)
            OpponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
    }

    public void ChangeLandmarkStatus(TargetInfo targetInfo, bool enable) // TargetAndAmount
    {
        ListEnum lE = targetInfo.whichList;

        if (lE.myLandmarks)
        {
            if (!enable)
                PlayerLandmarks[targetInfo.index].DisableLandmark();
            else
                PlayerLandmarks[targetInfo.index].EnableLandmark();
        }
        else if (lE.opponentLandmarks)
        {
            if (!enable)
                OpponentLandmarks[targetInfo.index].DisableLandmark();
            else
                OpponentLandmarks[targetInfo.index].EnableLandmark();
        }
    }

    public void CalculateAndHeal(int amount, Card cardUsed)
    {
        amount = calculations.CalculateHealing(amount, false);
        Invoke(nameof(TakeAwayHealEffect), 3f);
        HealTarget(calculations.TargetAndAmountFromCard(cardUsed, amount));

        audioManager.PlayHealSound();
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
            PlayerChampions[targetAndAmount.targetInfo.index].HealChampion(targetAndAmount.amount);
            effectController.GainHealingEffect(PlayerChampions[targetAndAmount.targetInfo.index].gameObject);

        }
        if (lE.opponentChampions)
        {
            OpponentChampions[targetAndAmount.targetInfo.index].HealChampion(targetAndAmount.amount);
			effectController.GainHealingEffect(OpponentChampions[targetAndAmount.targetInfo.index].gameObject);
        }

        if (IsOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>() { targetAndAmount, };
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


    public void ShieldTarget(TargetAndAmount targetAndAmount) // TargetAndAmount, If shieldchampion
    {
        ListEnum lE = targetAndAmount.targetInfo.whichList;
        if (lE.myChampions)
        {
            PlayerChampions[targetAndAmount.targetInfo.index].GainShield(targetAndAmount.amount);
            Tuple<string, bool> tuple = new Tuple<string, bool>(PlayerChampions[targetAndAmount.targetInfo.index].Champion.ChampionName, false);
            effectController.ActiveShield(tuple, targetAndAmount.amount, PlayerChampions[targetAndAmount.targetInfo.index].gameObject);
        }
        if (lE.opponentChampions)
        {
            OpponentChampions[targetAndAmount.targetInfo.index].GainShield(targetAndAmount.amount);
            Tuple<string, bool> tuple = new Tuple<string, bool>(OpponentChampions[targetAndAmount.targetInfo.index].Champion.ChampionName, true);
			effectController.ActiveShield(tuple, targetAndAmount.amount, OpponentChampions[targetAndAmount.targetInfo.index].gameObject);
        }

        if (IsOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>() { targetAndAmount, };
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
			    PlayerChampions[i].Champion = champ;
            else
                OpponentChampions[i].Champion = champ;
        }
    }

    public void ShowPlayedCard(Card card, bool opponent, int manaCost)
    {
        playedCardGO.SetActive(true);
        CardDisplay cardDisp = playedCardGO.GetComponent<CardDisplay>();
        CardDisplayAttributes cardDisplayAttributes = playedCardGO.transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        if (manaCost != -1)
            cardDisp.ManaCost = manaCost;

        cardDisp.Card = card;
        cardDisplayAttributes.previewCard = opponent;
        cardDisp.UpdateTextOnCard(true);
        StopCoroutine(HideCardPlayed());
        StartCoroutine(HideCardPlayed());
    }
    private IEnumerator HideCardPlayed()
    {
        yield return new WaitForSeconds(3f);
        playedCardGO.GetComponent<CardDisplay>().Card = null;
        playedCardGO.SetActive(false);
    }

    public void DestroyLandmark(TargetInfo targetInfo)
    {
        if (targetInfo.whichList.opponentLandmarks)
            graveyard.AddCardToGraveyardOpponent(OpponentLandmarks[targetInfo.index].Card);
        else
            graveyard.AddCardToGraveyard(PlayerLandmarks[targetInfo.index].Card);

        PlayerLandmarks[targetInfo.index].Card = null;
    }

    public void DiscardCard(int amountToDiscard, bool discardCardsYourself)
    {
        if (IsOnline)
        {
            if (discardCardsYourself)
            {
                ListEnum listEnum = new ListEnum();
                listEnum.myHand = true;
                choice.ChoiceMenu(listEnum, amountToDiscard, WhichMethod.DiscardCard, null);
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
                choice.ChoiceMenu(listEnum, amountToDiscard, WhichMethod.DiscardCard, null);
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
        if (IsOnline && isPlayer)
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
        if (IsOnline)
            DrawCard(amountOfCardsToStartWith, null);
        else
        {
            actionOfPlayer.DrawCardPlayer(amountOfCardsToStartWith, null, true);
            actionOfPlayer.DrawCardPlayer(amountOfCardsToStartWith, null, false);
        }
    }

	public void DrawRandomCardFromGraveyard(int amountOfCards)
	{
		Tuple<Card, int> info = graveyard.RandomizeCardFromGraveyard();
		actionOfPlayer.DrawCardPlayer(amountOfCards, info.Item1, true);
	}

    public void PassPriority()
    {
        if(HasPriority && IsOnline)
        {
            HasPriority = false;
            RequestPassPriority requestPassPriority = new RequestPassPriority(true);
            requestPassPriority.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(requestPassPriority, RequestEmpty);
        }
    }

    public void SwapOnDeath(AvailableChampion champ)
    {
        if (!IsOnline && champ.IsOpponent)
        {
            for (int i = 0; i < 100; i++)
            {
                int randomChamp = UnityEngine.Random.Range(0, OpponentChampions.Count);
                if (OpponentChampion != OpponentChampions[randomChamp])
                {
                    Swap(OpponentChampions, 0, randomChamp);
                    OpponentChampion.Champion.WhenCurrentChampion();
                    return;
                }
            }
        }

        ListEnum lE = new ListEnum();
        lE.myChampions = true;
        choice.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionDied, null);
    }

    public void SwapChampionWithTargetInfo(TargetInfo targetInfo, bool championDied)
    {
        if (targetInfo.whichList.myChampions == true)
        {
            Swap(PlayerChampions, 0, targetInfo.index);

            if (championDied)
                RemoveChampion(PlayerChampions[targetInfo.index].Champion);
        }
        if (targetInfo.whichList.opponentChampions == true)
            Swap(OpponentChampions, 0, targetInfo.index);
    }

    public void SwapChampionOnline(TargetInfo targetInfo, bool championDied)
    {
        if (targetInfo.whichList.myChampions == true)
        {
            Swap(OpponentChampions, 0, targetInfo.index);

            if (championDied)
                RemoveChampion(OpponentChampions[targetInfo.index].Champion);
        }
        if (targetInfo.whichList.opponentChampions == true)
            Swap(PlayerChampions, 0, targetInfo.index);
    }


    public void LandmarkPlaced(int index, Landmarks landmark, bool opponentPlayedLandmark)
    {
        landmark = Instantiate(cardRegister.landmarkRegister[landmark.CardName]);
        ShowPlayedCard(landmark, opponentPlayedLandmark, -1);
        LandmarkDisplay landmarkDisplay;

        if (opponentPlayedLandmark)
            landmarkDisplay = OpponentLandmarks[index];
        else
            landmarkDisplay = PlayerLandmarks[index];

		landmarkDisplay.Card = landmark;
		landmarkDisplay.transform.GetChild(0).gameObject.SetActive(true);
		landmarkDisplay.Health = landmark.MinionHealth;
		landmarkDisplay.ManaCost = landmarkDisplay.Card.MaxManaCost;
    }

	public IEnumerator ActivateYourTurnEffectAfterMulligan()
	{
		yield return new WaitUntil(() => HasPriority);
		yourTurnEffect.ActivateEffect();
	}

	public void TriggerUpKeep()
    {
        DrawCard(1, null);

        if (IsItMyTurn && !firstTurn || !IsOnline)
            actionOfPlayer.IncreaseMana();

        if (firstTurn)
            AmountOfTurns++;

        PlayerChampion.Champion.UpKeep();

        foreach (LandmarkDisplay landmarkDisplay in PlayerLandmarks)
        {
            if (landmarkDisplay.Card != null && landmarkDisplay.LandmarkEnabled)
            {
                Landmarks landmark = (Landmarks)landmarkDisplay.Card;
                landmark.UpKeep();
            }
        } 
        foreach (Effects effect in PlayerEffects)
        {
            effect.UpKeep();
        }
    }

    public void TriggerEndStep()
    {   
        if(!IsOnline)
            DrawCard(1, null, false);

		if (!firstTurn)
		    AmountOfTurns++;

		PlayerChampion.Champion.EndStep();

        foreach (LandmarkDisplay landmarkDisplay in PlayerLandmarks)
        {
            if(landmarkDisplay.Card != null && landmarkDisplay.LandmarkEnabled)
            {
                Landmarks landmark = (Landmarks)landmarkDisplay.Card;
                landmark.EndStep();
            }
        }
        foreach (Effects effect in PlayerEffects)
        {
            effect.EndStep();
        }
        foreach (CardDisplay cardDisplay in actionOfPlayer.HandPlayer.cardsInHand)
        {
            cardDisplay.EndStep();
        }

        actionOfPlayer.EnemyMana = actionOfPlayer.PlayerMana;
        actionOfPlayer.UpdateUnspentMana();
    }


    public void EndTurn()
    {
        if (!IsOnline)
        {
            TriggerEndStep();
            TriggerUpKeep();
            yourTurnEffect.ActivateEffect();
			actionOfPlayer.RoundCounter.text = "Round: " + AmountOfTurns;
			Refresh();
			return;
        }

        if (IsItMyTurn)
        {
            IsItMyTurn = false;
            TriggerEndStep();
            firstTurn = false;
        }
        else
        {       
            IsItMyTurn = true;
            TriggerUpKeep();
            yourTurnEffect.ActivateEffect();
        }

        actionOfPlayer.RoundCounter.text = "Round: " + AmountOfTurns;
        AttacksPlayedThisTurn = 0;
        ChangeInteractabiltyEndTurn();
        cardsPlayedThisTurn.Clear();

        DrawnCardsPreviousTurn = DrawnCardsThisTurn;
        DrawnCardsThisTurn = 0;
        Refresh();
    }

 

    public void AddCardToPlayedCardsThisTurn(Card cardPlayed)
    {
        cardsPlayedThisTurn.Add(cardPlayed);

        if (cardPlayed.TypeOfCard == CardType.Attack)
            AttacksPlayedThisTurn++;
        PlayerChampion.Champion.AmountOfCardsPlayed(cardPlayed);
    }

    public void ChampionDeath(Champion deadChampion)
    {
        SearchDeadChampion(deadChampion);
        if (PlayerChampions.Count == 0)
        {
            Defeat();
            return;
        }
        else if (OpponentChampions.Count == 0)
        {
            Victory();
            if(IsOnline)
            {

                print("skickas win game actionen??!?!??");
                RequestEndGame request = new RequestEndGame();
                ClientConnection.Instance.AddRequest(request,RequestEmpty);
            }

            return;
        }
    }

    private void SearchDeadChampion(Champion deadChampion)
    {
        if (PlayerChampion.Champion == deadChampion)
            SwapOnDeath(PlayerChampion);

        if (OpponentChampion.Champion == deadChampion)
        {
            if (!IsOnline)
            {
                SwapOnDeath(OpponentChampion);
                RemoveChampion(deadChampion);
            }
			else
			{
				if (OpponentChampions.Count == 1)
					Victory();
				else
					PassPriority();
			}
		}
	}

    private void RemoveChampion(Champion deadChamp)
    {
        foreach (AvailableChampion ac in PlayerChampions)
        {
            if (ac.Champion == deadChamp)
            {
                ac.UpdateTextOnCard();
                PlayerChampions.Remove(ac);
                break;
            }
        }
        foreach (AvailableChampion ac in OpponentChampions)
        {
            if (ac.Champion == deadChamp)
            {
                ac.UpdateTextOnCard();
                OpponentChampions.Remove(ac);
                break;
            }
        }
    }

    public void AddEffect(Effects effect)
    {
        effect.AddEffect();
        PlayerEffects.Add(effect);
    }

    public void RemoveEffect(Effects effect)
    {
        removeEffects.Add(effect);
    }

    public void ClearEffects()
    {
       foreach (Effects effect in removeEffects)
       {
           PlayerEffects.Remove(effect);
       }
       removeEffects.Clear();
    }

    public void RequestEmpty(ServerResponse response) {}

    //Victory and defeat should be in another script
    public void Victory()
    {
        wonScreen.SetActive(true);
        confettiOne.Play();
        confettiTwo.Play();
    }

    public void Defeat()
    {
        lostScreen.SetActive(true);
    }

    public static void Swap(List<AvailableChampion> list, int i, int j)
    {
        Champion temp = list[i].Champion;
        list[i].Champion = list[j].Champion;
        list[j].Champion = temp;
    }

    public void Refresh()
    {
        ClearEffects();
        foreach (LandmarkDisplay landmarkDisplay in PlayerLandmarks)
        {
            if (landmarkDisplay.Card != null)
                landmarkDisplay.UpdateTextOnCard();
        }
        foreach (LandmarkDisplay landmarkDisplay in OpponentLandmarks)
        {
            if (landmarkDisplay.Card != null)
                landmarkDisplay.UpdateTextOnCard();
        }
        
        ActionOfPlayer.Instance.HandPlayer.FixCardOrderInHand();
        ActionOfPlayer.Instance.HandOpponent.FixCardOrderInHand();

        foreach (AvailableChampion aC in PlayerChampions)
        {
            aC.UpdateTextOnCard();
        }
        foreach (AvailableChampion aC in OpponentChampions)
        {
            aC.UpdateTextOnCard();
        }


        WhichManaCrystalsToShow.Instance.UpdateManaCrystals();
        yourTurnEffect.ChangePicture(PlayerChampion);


        if (PlayerChampions.Count <= 0)
            Defeat();
           
        else if (OpponentChampions.Count == 1 && OpponentChampion.Champion.Health <= 0)
        {
            
            /*Victory();*/
        }
    }
}
