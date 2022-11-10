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

    [Header("Have Friends?")]
    public bool isOnline = false;

    [Header("CardsPlayed")]
    public List<Card> cardsPlayedThisTurn = new List<Card>();

    [Header("Win Screen")]
    [SerializeField] private GameObject lostScreen;
    [SerializeField] private GameObject wonScreen;

    [Header("Effect")]
    [SerializeField] private GameObject healEffect;

    
    [Header("UI Elements")]
    public GameObject playedCardGO;
    public Sprite backfaceCard;
    public UnityEngine.UI.Button endTurnBttn;


    [NonSerialized] public int currentPlayerID = 0;
    public bool hasPriority = true;

    [NonSerialized] public bool isItMyTurn;
    [NonSerialized] public bool didIStart;
    public int amountOfTurns;

    [NonSerialized] public GameObject targetingEffect;

    [NonSerialized] public int factory = 0;
    [NonSerialized] public int landmarkEffect = 1;
    [NonSerialized] public int attacksPlayedThisTurn;

    private bool firstTurn = true;

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
        actionOfPlayer = ActionOfPlayer.Instance;
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
        }
        if (isOnline)
        {
            AddChampions(Setup.Instance.myChampions);
            AddChampionsOpponent(Setup.Instance.opponentChampions);

            Deck.Instance.CreateDecks(Setup.Instance.playerDeckList);

        }
        else
        {
            List<string> ha = new List<string>();
            ha.Add("Builder");
            ha.Add("Duelist");
            ha.Add("Graverobber");
            AddChampions(ha);
            AddChampionsOpponent(ha);
        }
        playerChampion = playerChampions[0];
        opponentChampion = opponentChampions[0];

        DrawStartingCards();
    }

    private void ChangeInteractabiltyEndTurn()
    {
        endTurnBttn.interactable = !endTurnBttn.interactable;
    }

    public  void EndTurnButtonClick()
    {
        if (!hasPriority) return;
            
        if (isOnline)
        {
            RequestEndTurn request = new RequestEndTurn();
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestEndTurn);
        }

        EndTurn();
    }

    public void CalculateBonusDamage(int damage, Card cardUsed)
    {
        
        damage = playerChampion.champion.DealDamageAttack(damage);

        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            if(landmark.card != null)
            damage = landmark.card.DealDamageAttack(damage);
        }

        TargetAndAmount tAA = null;
        TargetInfo tI = null;
        ListEnum listEnum = new ListEnum();
         int index = 0;
        // WIP
        if (cardUsed.Target != null)
        {                 
            index = LookForChampionIndex(cardUsed, opponentChampions);
            if (index == -1)
            {
                index = LookForChampionIndex(cardUsed, playerChampions);
                listEnum.myChampions = true;                 
            }               
            else
            {
                listEnum.opponentChampions = true;                   
            }
        }
        else if (cardUsed.LandmarkTarget != null)
        {
            index = LookForLandmarkIndex(cardUsed, opponentLandmarks);
            if (index == -1)
            {
                index = LookForLandmarkIndex(cardUsed, playerLandmarks);
                listEnum.myLandmarks = true;
            }
            else
            {
                listEnum.opponentLandmarks = true;
            }
        }
        tI = new TargetInfo(listEnum, index);
        tAA = new TargetAndAmount(tI, damage);
        DealDamage(tAA);
        if (playerChampion.champion.animator != null)
        {
            playerChampion.champion.animator.SetTrigger("Attack");
        }
    }

    public int LookForChampionIndex(Card cardUsed, List<AvailableChampion> champ )
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
    public int LookForLandmarkIndex(Card cardUsed, List<LandmarkDisplay> landmarks )
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

    public void DealDamage(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE =  targetAndAmount.targetInfo.whichList;
        if (isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestDamage request = new RequestDamage(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestDamage);
        }

        if (targetAndAmount.targetInfo.index == -1)
        {
            print("ERROR INDEX -1, YOU STUPID");
        }

        if(lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount, playerChampion.gameObject);
        }
        if(lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount, opponentChampion.gameObject);
        }
        if(lE.myLandmarks)
        {
            playerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }
        if(lE.opponentLandmarks)
        {
            opponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }
    }

    public void CalculateHealing(int amount, Card cardUsed)
    {

        TargetAndAmount tAA = null;
        TargetInfo tI = null;
        ListEnum listEnum = new ListEnum();
        int index = 0;
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            if (landmark.card == null) continue;
            amount = landmark.card.HealingEffect(amount);
        }

        // WIP
        if (cardUsed.Target != null)
        {
            index = LookForChampionIndex(cardUsed, opponentChampions);
            if (index == -1)
            {
                index = LookForChampionIndex(cardUsed, playerChampions);
                listEnum.myChampions = true;
            }
            else
            {
                listEnum.opponentChampions = true;
            }
        }
        tI = new TargetInfo(listEnum, index);
        tAA = new TargetAndAmount(tI, amount);
        Invoke(nameof(TakeAwayHealEffect), 3f);
        HealTarget(tAA);
    }

    private void TakeAwayHealEffect()
    {
        healEffect.SetActive(false);
    }

    public void HealTarget(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE =  targetAndAmount.targetInfo.whichList;
        print("vilket index healing" + targetAndAmount.targetInfo.index);
        
        if(lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].champion.HealChampion(targetAndAmount.amount);
        }
        if(lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].champion.HealChampion(targetAndAmount.amount);
        }
                
        if(isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestHealing request = new RequestHealing(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestDamage);
        }
    }

    public void CalculateShield(int amount, Card cardUsed)
    {
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            if (landmark.card == null) continue;
            amount = landmark.card.ShieldingEffect(amount);
        }
        TargetAndAmount tAA = null;
        TargetInfo tI = null;
        ListEnum listEnum = new ListEnum();
        int index = 0;
        // WIP
        if (cardUsed.Target != null)
        {
            index = LookForChampionIndex(cardUsed, opponentChampions);
            if (index == -1)
            {
                index = LookForChampionIndex(cardUsed, playerChampions);
                listEnum.myChampions = true;
            }
            else
            {
                listEnum.opponentChampions = true;
            }
        }

        tI = new TargetInfo(listEnum, index);
        tAA = new TargetAndAmount(tI, amount);

        ShieldTarget(tAA);
    }

    public void ShieldTarget(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE = targetAndAmount.targetInfo.whichList;
        print("vilket index shielding" + targetAndAmount.targetInfo.index);

        if (lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
            EffectController.Instance.ActiveShield(playerChampions[targetAndAmount.targetInfo.index].gameObject, targetAndAmount.amount);
        }
        if (lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
            EffectController.Instance.ActiveShield(opponentChampions[targetAndAmount.targetInfo.index].gameObject, targetAndAmount.amount);
        }

        if (isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestShield request = new RequestShield(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestDamage);
        }
    }


    private void AddChampions(List<string> champions)
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
            playerChampions[i].champion = champ;
        }
    }

    private void AddChampionsOpponent(List<string> champions)
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
            opponentChampions[i].champion = champ;
        }
    }

    private void DrawStartingCards()
    {
        DrawCardPlayer(amountOfCardsToStartWith, null);
        DrawCardOpponent(amountOfCardsToStartWith, null);
    }

  

    public void DrawCardRequest(ServerResponse response)
    {
        ResponseDrawCard castedReponse = (ResponseDrawCard)response;

        //DrawCard(castedReponse.amountToDraw, null);
    }

    public void DrawRandomCardFromGraveyard(int amountOfCards)
    {
        Tuple<Card, int> info = Graveyard.Instance.RandomizeCardFromGraveyard();
        DrawCardPlayer(amountOfCards, info.Item1);
    }


    public void ShowPlayedCardLandmark(Landmarks landmark)
    {
        playedCardGO.SetActive(true);
        CardDisplay cardDisp = playedCardGO.GetComponent<CardDisplay>();
        cardDisp.card = landmark;
        cardDisp.manaCost = landmark.maxManaCost;
        
        Invoke(nameof(HideLandmarkPlayed), 3f);
    }

    public void ShowPlayedCard(Card card)
    {
        playedCardGO.SetActive(true);
        CardDisplay cardDisp = playedCardGO.GetComponent<CardDisplay>();
        cardDisp.card = card;
        cardDisp.manaCost = card.maxManaCost;
        Invoke(nameof(HideCardPlayed), 3f);
    }
    private void HideCardPlayed()
    {
        playedCardGO.GetComponent<CardDisplay>().card = null;
        playedCardGO.SetActive(false);
    }

    private void HideLandmarkPlayed()
    {
        playedCardGO.GetComponent<CardDisplay>().card = null;
        playedCardGO.SetActive(false);
    }


    public void DestroyLandmark()
    {
        bool existLandmark = false;
        for (int i = 0; i < opponentLandmarks.Count; i++)
        {
            if(opponentLandmarks[i] != null)
            {
                existLandmark = true;
                break;
            }
        }

        if(!existLandmark)return;

        for (int i = 0; i < 25; i++)
        {
            int random = UnityEngine.Random.Range(0, opponentLandmarks.Count);
            if (opponentLandmarks[random] != null)
            {
                opponentLandmarks[random] = null;
                break;
            }
        }
    }
    public void DestroyLandmark(TargetInfo targetInfo)
    {
        if (targetInfo.whichList.myLandmarks)
        {
            Graveyard.Instance.AddCardToGraveyardOpponent(opponentLandmarks[targetInfo.index].card);
            opponentLandmarks[targetInfo.index].card = null;
        }
        else
        {
            Graveyard.Instance.AddCardToGraveyard(playerLandmarks[targetInfo.index].card);
            playerLandmarks[targetInfo.index].card = null; 
        }
    }

    public string DiscardWhichCard(bool yourself)
    {
        string discardedCard = "";
        if (yourself)
            discardedCard = actionOfPlayer.handPlayer.DiscardRandomCardInHand().name;
        else
            discardedCard = actionOfPlayer.handOpponent.DiscardRandomCardInHand().name;
        return discardedCard;
    }

    public void DiscardCard(int amountToDiscard, bool discardCardsYourself)
    {

        if (isOnline)
        {
            if (discardCardsYourself)
            {
                RequestDiscardCard request = new RequestDiscardCard();
                request.whichPlayer = ClientConnection.Instance.playerId;
                List<string> cardsDiscarded = new List<string>();
                for (int i = 0; i < amountToDiscard; i++)
                {
                    cardsDiscarded.Add(DiscardWhichCard(discardCardsYourself));
                }
                request.listOfCardsDiscarded = cardsDiscarded;

                ClientConnection.Instance.AddRequest(request, RequestDiscardCard);
            }
            else
            {
                RequestOpponentDiscardCard requesten = new RequestOpponentDiscardCard();
                requesten.whichPlayer = ClientConnection.Instance.playerId;
                requesten.amountOfCardsToDiscard = amountToDiscard;
                ClientConnection.Instance.AddRequest(requesten, RequestDiscardCard);

            }
        }
        else
        {
            for (int i = 0; i < amountToDiscard; i++)
            {
                DiscardWhichCard(discardCardsYourself);
            }
        }
    }

    public void DrawCard(int amountToDraw, Card specificCard)
    {
        //  ActionOfPlayer.Instance.DrawCard(amountOfCardsToDraw);
        if (isOnline)
        {
            RequestDrawCard request = new RequestDrawCard(amountToDraw);
            request.whichPlayer = ClientConnection.Instance.playerId;
           
            ClientConnection.Instance.AddRequest(request, DrawCardRequest);
            DrawCardPlayer(amountToDraw, specificCard);
        }
        else
        {
            DrawCardPlayer(amountToDraw, specificCard);
        }
    }


    public void DrawCardPlayer(int amountToDraw, Card specificCard)
    {
        int drawnCards = 0;
        foreach (GameObject cardSlot in actionOfPlayer.handPlayer.cardSlotsInHand)
        {
            CardDisplay cardDisplay = cardSlot.GetComponent<CardDisplay>();
            if (cardDisplay.card != null) continue;

            if (!cardSlot.activeSelf)
            {
                if (drawnCards >= amountToDraw) break;

                if (specificCard == null)
                    cardDisplay.card = actionOfPlayer.handPlayer.deck.WhichCardToDrawPlayer();
                else
                    cardDisplay.card = specificCard;

                cardDisplay.manaCost = cardDisplay.card.maxManaCost;
                cardSlot.SetActive(true);
                drawnCards++;
                playerChampion.champion.DrawCard(cardDisplay);
            }
        }

        if (drawnCards < amountToDraw)
        {
            for (; drawnCards < amountToDraw; drawnCards++)
            {
                Card c = actionOfPlayer.handPlayer.deck.WhichCardToDrawPlayer();
                Graveyard.Instance.AddCardToGraveyard(c);
            }
        }

    }
    public void DrawCardOpponent(int amountToDraw, Card specificCard)
    {
        int drawnCards = 0;
        foreach (GameObject cardSlot in actionOfPlayer.handOpponent.cardSlotsInHand)
        {
            CardDisplay cardDisplay = cardSlot.GetComponent<CardDisplay>();
            if (cardDisplay.card != null) continue;

            if (!cardSlot.activeSelf)
            {
                if (drawnCards >= amountToDraw) break;

                if (specificCard == null)
                {
                    cardDisplay.card = actionOfPlayer.handOpponent.deck.WhichCardToDrawOpponent();
                    cardDisplay.opponentCard = true;
                    cardDisplay.artworkSpriteRenderer.sprite = backfaceCard;
                }

                else
                {
                    cardDisplay.card = specificCard;
                    cardDisplay.opponentCard = true;
                    cardDisplay.artworkSpriteRenderer.sprite = backfaceCard;

                }
                cardSlot.SetActive(true);
                drawnCards++;
                opponentChampion.champion.DrawCard(cardDisplay);
            }
        }

        if (drawnCards < amountToDraw)
        {
            for (; drawnCards < amountToDraw; drawnCards++)
            {
                Card c = actionOfPlayer.handOpponent.deck.WhichCardToDrawPlayer();
                Graveyard.Instance.AddCardToGraveyardOpponent(c);
            }
        }
    }

    public void SwitchMyChampions(TargetInfo targetInfo)
    {
        if (targetInfo.whichList.myChampions)
        {
            print("Hallo");
            playerChampion.champion.WhenInactiveChampion();
            Swap(playerChampions, 0, targetInfo.index);
            playerChampion.champion.WhenCurrentChampion();
        }
        else if(targetInfo.whichList.opponentChampions)
        {
			Swap(opponentChampions, 0, targetInfo.index);
            //hasPriority = false;
/*            print("HALLO");
            if (isOnline)
            {
                RequestPassPriority requestPassPriority = new RequestPassPriority(true);
                requestPassPriority.whichPlayer = ClientConnection.Instance.playerId;
                ClientConnection.Instance.AddRequest(requestPassPriority, RequestEmpty);
            }*/

        }

    }

    public void SwapActiveChampion(Card card)
    {   
        if (isOnline)
        {
            ListEnum lE = new ListEnum();
            lE.myChampions = true;
            Choise.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionDied);                    
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
                    opponentChampion.champion.WhenInactiveChampion();
                    Swap(opponentChampions, 0, randomChamp);
                    opponentChampion.champion.WhenCurrentChampion();
                    break;
                }
            }
            
        }
        
        
        //playerChampion.champion = playerChampions[randomChamp].champion; 
    }

    public void LandmarkPlaced(int index, Landmarks landmark, bool opponentPlayedLandmark)
    {
        switch (landmark.tag)
        {
            case "HealingLandmark":
                landmark = new HealingLandmark((HealingLandmark)landmark);
                break;
            case "TauntLandmark":
                landmark = new TauntLandmark((TauntLandmark)landmark);
                break;
            case "DamageLandmark":
                landmark = new DamageLandmark((DamageLandmark)landmark);
                break;
            case "DrawCardLandmark":
                landmark = new DrawCardLandmark((DrawCardLandmark)landmark);
                break;
            case "CultistLandmark":
                landmark = new CultistLandmark((CultistLandmark)landmark);
                break;
            case "BuilderLandmark":
                landmark = new BuilderLandmark((BuilderLandmark)landmark);
                break;
        }

        if (opponentPlayedLandmark)
        {
            opponentLandmarks[index].card = landmark;
            opponentLandmarks[index].manaCost = opponentLandmarks[index].card.maxManaCost;
            opponentLandmarks[index].health = landmark.minionHealth;
        }
        else
        {
            playerLandmarks[index].card = landmark;
            playerLandmarks[index].health = landmark.minionHealth;
            playerLandmarks[index].manaCost = playerLandmarks[index].card.maxManaCost;
        }
    }

    

    public void TriggerUpKeep()
    {
        print("Den triggrar upkeep");
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
            if (landmark.card != null)
                landmark.card.UpKeep();
            //Trigger landmark endstep
        }       
    }

    public void TriggerEndStep()
    {
        print("Den triggrar endstep");
        playerChampion.champion.EndStep();
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            if(landmark.card != null)
            landmark.card.EndStep();
            //Trigger landmark endstep
        }
    }


    public void EndTurn()
    {
        print("Ending turn");
        if (!isOnline)
        {
            TriggerEndStep();
            TriggerUpKeep();
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
            isItMyTurn = true;
            TriggerUpKeep();
        }

        ChangeInteractabiltyEndTurn();
        cardsPlayedThisTurn.Clear();
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
        if (playerChampions.Count == 1)
        {
            Defeat();
            return;
        }
        else if (opponentChampions.Count == 1)
        {
            Victory();
            return;
        }
        SearchDeadChampion(deadChampion);


    }

    private void SearchDeadChampion(Champion deadChampion)
    {
        if (playerChampion.champion == deadChampion)
        {
            hasPriority = true;
            SwapActiveChampion(null);
        }
        else if (!isOnline && opponentChampion.champion == deadChampion)
        {          
            SwapActiveChampionEnemy();
        }

		if (isOnline && opponentChampion.champion == deadChampion)
		{
            hasPriority = false;
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

    public void RequestDiscardCard(ServerResponse response)
    {
        //ResponseDiscardCard castedResponse = (ResponseDiscardCard)response;

       // DiscardCard(castedResponse.listOfCardsDiscarded);
    }
    public void DiscardCard(List<string> listOfCardsDiscarded)
    {

    }
    public void RequestHeal(ServerResponse response)
    {
        //ResponseDiscardCard castedResponse = (ResponseDiscardCard)response;

        //DiscardCard(castedResponse.listOfCardsDiscarded);
    }
 

    public void RequestDamage(ServerResponse response)
    {
     //   ResponseDiscardCard castedResponse = (ResponseDiscardCard)response;

    //    DiscardCard(castedResponse.listOfCardsDiscarded);
    }
    public void RequestPlayCard(ServerResponse response)
    {

    }
    public void RequestEndTurn(ServerResponse response)
    {

    }
    public void RequestEmpty(ServerResponse response)
    {

    }

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
}
