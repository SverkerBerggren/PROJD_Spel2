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
    public int currentPlayerID = 0;
    public bool hasPriority = true;

    public bool isItMyTurn;
    public bool didIStart;

    public int amountOfTurns;

   
    private int amountOfCardsToStartWith = 5;

    [SerializeField] private GameObject lostScreen;
    [SerializeField] private GameObject wonScreen;

    [SerializeField] private GameObject healEffect;

    public ActionOfPlayer actionOfPlayer;
    public int currentMana;
    public SpriteRenderer playedCardSpriteRenderer;
    public Sprite backfaceCard;

    public GameObject EndTurnButton;
    private UnityEngine.UI.Button endTurnBttn;

    public AvailableChampion playerChampion;
    public AvailableChampion opponentChampion;
    [NonSerialized] public bool drawExtraCardsEachTurn = false;
    [NonSerialized] public int occultGathering = 0;

    public List<AvailableChampion> playerChampions = new List<AvailableChampion>();
    public List<AvailableChampion> opponentChampions = new List<AvailableChampion>();

    public List<LandmarkDisplay> playerLandmarks = new List<LandmarkDisplay>();
    public List<LandmarkDisplay> opponentLandmarks = new List<LandmarkDisplay>();

    public List<Card> cardsPlayedThisTurn = new List<Card>();
    public int attacksPlayedThisTurn;

    [NonSerialized] public int tenExtraDamage;
    [NonSerialized] public int damageRamp = 0;
    [NonSerialized] public int slaughterhouse = 0;
    [NonSerialized] public int factory = 0;
    [NonSerialized] public int landmarkEffect = 1;

    private static GameState instance;
    public static GameState Instance { get; set; }


    public bool isOnline = false;


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

        AddChampions(playerChampions);
        AddChampionsOpponent(opponentChampions);
        playerChampion = playerChampions[0];
        opponentChampion = opponentChampions[0];
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
        endTurnBttn = EndTurnButton.GetComponent<UnityEngine.UI.Button>();
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

        Invoke(nameof(DrawStartingCards), 0.01f);
    }

    private void ChangeInteractabiltyEndTurn()
    {
        endTurnBttn.interactable = !endTurnBttn.interactable;
    }

    public  void EndTurnButtonClick()
    {
        if(isOnline)
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

        damage += damageRamp;

        if (slaughterhouse > 0)
        {
            damage += 10 * slaughterhouse;           
        }

        if (tenExtraDamage > 0)
        {
            damage += (10 * tenExtraDamage);
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
            if (landmarks[i] == cardUsed.Target)
            {
                return i;
            }
        }
        return -1;
    }

    public void DealDamage(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE =  targetAndAmount.targetInfo.whichList;

        if (targetAndAmount.targetInfo.index == -1)
        {
            print("ERROR INDEX -1, YOU STUPID");
        }

        if(lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount);
        }
        if(lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount);
        }
        if(lE.myLandmarks)
        {
            playerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }
        if(lE.opponentLandmarks)
        {
            opponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
        }

        if(isOnline)
        {
            List<TargetAndAmount> list = new List<TargetAndAmount>();
            list.Add(targetAndAmount);

            RequestDamage request = new RequestDamage(list);
            request.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(request, RequestDamage);
        }
    }

    public void CalculateHealing(int amount, Card cardUsed)
    {
        int healingToDo = 0;
        healingToDo += amount * landmarkEffect;

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
        tAA = new TargetAndAmount(tI, healingToDo);

        healEffect.SetActive(true);
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
        int shieldingToDo = 0;
        shieldingToDo += amount * landmarkEffect;

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
        tAA = new TargetAndAmount(tI, shieldingToDo);

        playerChampion.GetComponent<ArmorEffect>().SetArmor(shieldingToDo);

        ShieldTarget(tAA);
    }

    public void ShieldTarget(TargetAndAmount targetAndAmount) // TargetAndAmount
    {

        ListEnum lE = targetAndAmount.targetInfo.whichList;
        print("vilket index shielding" + targetAndAmount.targetInfo.index);

        if (lE.myChampions)
        {
            playerChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
        }
        if (lE.opponentChampions)
        {
            opponentChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
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


    private void AddChampions(List<AvailableChampion> champions)
    {
        for (int i = 0; i < champions.Count; i++)
        {
            Champion champ = null;
            switch (champions[i].champion.name)
            {
                case "Cultist":
                    champ = new Cultist((Cultist)champions[i].champion);
                    break;

                case "Builder":
                    champ = new Builder((Builder)champions[i].champion);
                    break;

                case "Shanker":
                    champ = new Shanker((Shanker)champions[i].champion);
                    break;

                case "Gravedigger":
                    champ = new Gravedigger((Gravedigger)champions[i].champion);
                    break;

                case "TheOneWhoDraws":
                    champ = new TheOneWhoDraws((TheOneWhoDraws)champions[i].champion);
                    break;

                case "Duelist":
                    champ = new Duelist((Duelist)champions[i].champion);
                    break;
            }
            playerChampions[i].champion = champ;
        }
    }

    private void AddChampionsOpponent(List<AvailableChampion> champions)
    {
        for (int i = 0; i < champions.Count; i++)
        {
            Champion champ = null;
            switch (champions[i].champion.name)
            {
                case "Cultist":
                    champ = new Cultist((Cultist)champions[i].champion);
                    break;

                case "Builder":
                    champ = new Builder((Builder)champions[i].champion);
                    break;

                case "Shanker":
                    champ = new Shanker((Shanker)champions[i].champion);
                    break;

                case "Gravedigger":
                    champ = new Gravedigger((Gravedigger)champions[i].champion);
                    break;

                case "TheOneWhoDraws":
                    champ = new TheOneWhoDraws((TheOneWhoDraws)champions[i].champion);
                    break;

                case "Duelist":
                    champ = new Duelist((Duelist)champions[i].champion);
                    break;
            }
            opponentChampions[i].champion = champ;
        }
    }

    private void DrawStartingCards()
    {
        StartCoroutine(DrawCardPlayer(amountOfCardsToStartWith, null));
        StartCoroutine(DrawCardOpponent(amountOfCardsToStartWith, null));
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


    public void ShowPlayedCard(Card card)
    {
        playedCardSpriteRenderer.sprite = card.artwork;
        Invoke(nameof(HideCardPlayed), 3f);
    }
    private void HideCardPlayed()
    {
        playedCardSpriteRenderer.sprite = null;
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
            StartCoroutine(DrawCardPlayer(amountToDraw, specificCard));
        }
        else
        {
            StartCoroutine(DrawCardPlayer(amountToDraw, specificCard));
        }
    }


    public IEnumerator DrawCardPlayer(int amountToDraw, Card specificCard)
    {
        if (actionOfPlayer.handPlayer.cardsInHand.Count > 0)
        {
          //  ChangeCardOrder();
            yield return new WaitForSeconds(0.01f); 
        }

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

                cardSlot.SetActive(true);
                drawnCards++;
                playerChampion.champion.DrawCard();
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
    public IEnumerator DrawCardOpponent(int amountToDraw, Card specificCard)
    {
        if (actionOfPlayer.handOpponent.cardsInHand.Count > 0)
        {
            ChangeCardOrder();
            yield return new WaitForSeconds(0.01f);
        }

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
                    cardDisplay.card = actionOfPlayer.handPlayer.deck.WhichCardToDrawPlayer();
                    cardDisplay.opponentCard = true;
                    cardDisplay.artworkSpriteRenderer.sprite = backfaceCard;
                }

                else
                    cardDisplay.card = specificCard;
                cardSlot.SetActive(true);
                drawnCards++;
                opponentChampion.champion.DrawCard();
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

    public void SwapActiveChampion(TargetInfo targetInfo)
    {   
        if(targetInfo != null)
        {
            SwapChampionWithTargetInfo(targetInfo);
            return;
        }

        for (int i = 0; i < 25; i++)
        {
            int randomChamp = UnityEngine.Random.Range(0, playerChampions.Count);
            if (playerChampion != playerChampions[randomChamp])
            {
                Champion champ = playerChampion.champion;
                playerChampion.champion = playerChampions[randomChamp].champion;
                playerChampions[randomChamp].champion = champ;

                if(isOnline)
                {
                    //Den måste berätta att championen har dött genom requesten, kanske genom att göra en variant alternativt göra en ny request
                    ListEnum lE = new ListEnum();
                    lE.myChampions = true;
                    TargetInfo tI = new TargetInfo(lE, randomChamp);
                    RequestSwitchActiveChamps request = new RequestSwitchActiveChamps(tI);
                    request.whichPlayer = ClientConnection.Instance.playerId;
                    request.targetToSwitch = tI;
                    ClientConnection.Instance.AddRequest(request, RequestEmpty);
                }

                break; 
            }
        }
        playerChampion.champion.WhenCurrentChampion();
        //playerChampion.champion = playerChampions[randomChamp].champion; 
    }

    private void SwapChampionWithTargetInfo(TargetInfo targetInfo)
    {
        if (targetInfo.whichList.opponentChampions == true)
        {
            Champion champ = playerChampion.champion;
            playerChampion.champion = playerChampions[targetInfo.index].champion;
            playerChampions[targetInfo.index].champion = champ;
        }
        else if (targetInfo.whichList.myChampions == true)
        {
            Champion champ = opponentChampion.champion;
            opponentChampion.champion = opponentChampions[targetInfo.index].champion;
            opponentChampions[targetInfo.index].champion = champ;
        }
    }

    public void SwapActiveChampionEnemy(TargetInfo targetInfo)
    {
        if (targetInfo != null)
        {
            SwapChampionWithTargetInfo(targetInfo);
            return;
        }

        for (int i = 0; i < 25; i++)
        {
            int randomChamp = UnityEngine.Random.Range(0, opponentChampions.Count);
            if (opponentChampion != opponentChampions[randomChamp])
            {
                Champion champ = opponentChampion.champion;
                opponentChampion.champion = opponentChampions[randomChamp].champion;
                opponentChampions[randomChamp].champion = champ;
                
                ListEnum lE = new ListEnum();
                lE.opponentChampions = true;
                TargetInfo tI = new TargetInfo(lE, randomChamp);
                break;

            }
        }
        opponentChampion.champion.WhenCurrentChampion();
        //playerChampion.champion = playerChampions[randomChamp].champion; 
    }

    private void ChangeCardOrder()
    {
        Hand hand = actionOfPlayer.handPlayer;
        for (int i = 0; i < hand.cardSlotsInHand.Count; i++)
        {
            if (hand.cardSlotsInHand[i].activeSelf == true)
            {
                for (int j = 0; j < i; j++)
                {
                    if (hand.cardSlotsInHand[j].activeSelf == false)
                    {
                        hand.cardSlotsInHand[j].GetComponent<CardDisplay>().card = hand.cardSlotsInHand[i].GetComponent<CardDisplay>().card;
                        hand.cardsInHand.Remove(hand.cardSlotsInHand[i]);
                        hand.cardsInHand.Add(hand.cardSlotsInHand[j]);
                        hand.cardSlotsInHand[i].SetActive(false);
                        hand.cardSlotsInHand[j].SetActive(true);
                        break;
                    }
                }

            }
        }
    }

    public void LandmarkPlaced(int index, Landmarks landmark, bool opponentPlayedLandmark)
    {
        if (opponentPlayedLandmark)
        {
            opponentLandmarks[index].card = landmark;
        }
        else
        {
            print("landmark index " + index);
            playerLandmarks[index].card = landmark;
        }
    }


    public void SwitchTurn(ServerResponse response)
    {
        print("switchar eden tur");
        TriggerEndStep(response);
        // spelaren med priority end of turn effects triggrar aka EndOfTurnEffects(Player player1)

        TriggerUpKeep(response);
        // spelaren med priority upkeep effects triggrar aka UpkeepEffects(Player player2)
        hasPriority = false;
    }

    public void TriggerEndStep(ServerResponse response)
    {
        print("Den triggrar endstep");
        playerChampion.champion.EndStep();
        //opponentChampion.champion.EndStep();
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            //Trigger landmark endstep
        }
        foreach (LandmarkDisplay landmark in opponentLandmarks)
        {
            //Trigger landmark endstep opponent
        }
        EndTurn();
    }


    public void EndTurn()
    {
        if (!isOnline)
        {
            amountOfTurns++;
            DrawCard(1, null);
            actionOfPlayer.IncreaseMana();
            playerChampion.champion.EndStep();
            //playerChampion.champion.UpKeep();
            return;
        }
        
        if(!didIStart)
        {
            amountOfTurns++;
        }

        if (isItMyTurn)
        {
            isItMyTurn = false;
            playerChampion.champion.EndStep();
            //opponentChampion.champion.UpKeep();
        }
        else
        {
            isItMyTurn = true;
            DrawCard(1, null);
            actionOfPlayer.IncreaseMana();
            opponentChampion.champion.EndStep();
            //playerChampion.champion.UpKeep();
        }

        if (drawExtraCardsEachTurn)
            DrawCard(1, null);

        ChangeInteractabiltyEndTurn();
        cardsPlayedThisTurn.Clear();
        damageRamp = 0;
    }

    public void AddCardToPlayedCardsThisTurn(Card cardPlayed)
    {
        cardsPlayedThisTurn.Add(cardPlayed);


        if (cardPlayed.GetType().Equals("AttackSpell"))
        {
            attacksPlayedThisTurn++;
        }

        if (occultGathering > 0)
        {
            if (cardPlayed.GetType().Equals("AttackSpell"))
            {
                damageRamp += 10 * occultGathering;
            }
        }

        playerChampion.champion.AmountOfCardsPlayed(cardPlayed);
    }

    public void TriggerUpKeep(ServerResponse response)
    {
        print("Den triggrar upkeep");
        playerChampion.champion.UpKeep();
        //opponentChampion.champion.UpKeep();
        foreach (LandmarkDisplay landmark in playerLandmarks)
        {
            //Trigger landmark endstep
        }
        foreach (LandmarkDisplay landmark in opponentLandmarks)
        {
            //Trigger landmark endstep opponent
        }
    }

    public void OnChampionDeath(ServerResponse response)
    {
        if (response.whichPlayer == ClientConnection.Instance.playerId)
        {

            //Choose Champion
            //Pass priority
            //hasPriority = true;
        }
        else
        {
            //hasPriority = false;
        }
    }

    public void ChampionDeath(Champion deadChampion)
    {
        SearchDeadChampion(deadChampion);

        if (playerChampions.Count == 0)
        {
            Defeat();
        }
        else if (opponentChampions.Count == 0)
        {
            Victory();
        }
    }

    private void SearchDeadChampion(Champion deadChampion)
    {

        if (playerChampion.champion == deadChampion)
        {
            SwapActiveChampion(null);
        }
        else if (opponentChampion.champion == deadChampion)
        {
            SwapActiveChampionEnemy(null);
        }

        foreach (AvailableChampion ac in playerChampions)
        {
            if (ac.champion == deadChampion)
            {
                playerChampions.Remove(ac);
                break;
            }
        }

        foreach (AvailableChampion ac in opponentChampions)
        {
            if (ac.champion == deadChampion)
            {
                opponentChampions.Remove(ac);
                break;
            }
        }

    }

    public void SwitchWhenChampionDead()
    {
        /*
		if (targetInfo != null)
		{
			SwapChampionWithTargetInfo(targetInfo);
			return;
		}

		for (int i = 0; i < 25; i++)
		{
			int randomChamp = UnityEngine.Random.Range(0, opponentChampions.Count);
			if (opponentChampion != opponentChampions[randomChamp])
			{
				Champion champ = opponentChampion.champion;
				opponentChampion.champion = opponentChampions[randomChamp].champion;
				opponentChampions[randomChamp].champion = champ;

				ListEnum lE = new ListEnum();
				lE.opponentChampions = true;
				TargetInfo tI = new TargetInfo(lE, randomChamp);
				return;

			}
		}
        */
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
}
