using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class InternetLoop : MonoBehaviour
{
    ClientConnection clientConnection;
    bool hasEstablishedEnemurator = false; 

    public GameState gameState;
    public CardRegister register;
    public string loadScene;
    public  bool hasJoinedLobby = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        clientConnection = ClientConnection.Instance;   
    }

    public void PerformOpponentsActions(ServerResponse response)
    {   
        gameState = GameState.Instance;
        register = CardRegister.Instance;

        if(!response.message.Equals(""))
        {
            print("Reponse message: " + response.message);
        }

        foreach (GameAction action in response.OpponentActions)
        {
            
            //print("vilket object typ ar grejen " + action.GetType() + action.Type);
            if (action is GameActionEndTurn )
            {
                gameState.EndTurn();

            }

            if (action is GameActionDrawCard)
            {
                GameActionDrawCard theAction = (GameActionDrawCard)action;
                
                if(theAction.amountToDraw > 0)
                {
					ActionOfPlayer.Instance.DrawCardPlayer(theAction.amountToDraw, null, false);
                }

            }
            if (action is GameActionDiscardCard)
            {
              
                GameActionDiscardCard theAction = (GameActionDiscardCard)action;

                foreach(string card in theAction.listOfCardsDiscarded)
                {
                    if (theAction.discardCardToOpponentGraveyard)
                    {
                        Graveyard.Instance.AddCardToGraveyard(register.cardRegister[card]);
                    }
                    else
                    {
                        Graveyard.Instance.AddCardToGraveyardOpponent(register.cardRegister[card]);
                    }
                    
                    ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
                    if(!theAction.listEnum.myDeck)
                        actionOfPlayer.ChangeCardOrder(false, actionOfPlayer.HandOpponent.cardsInHand[0]);
                    else
                    {
                        Deck.Instance.WhichCardToDrawPlayer(false);
                    }
                }

            }
            if (action is GameActionHeal)
            {

                GameActionHeal castedAction = (GameActionHeal)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToHeal)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        gameState.PlayerChampions[targetAndAmount.targetInfo.index].HealChampion(targetAndAmount.amount);
                        EffectController.Instance.GainHealingEffect(gameState.PlayerChampions[targetAndAmount.targetInfo.index].gameObject);
                    }

                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        gameState.OpponentChampions[targetAndAmount.targetInfo.index].HealChampion(targetAndAmount.amount);
                        EffectController.Instance.GainHealingEffect(gameState.OpponentChampions[targetAndAmount.targetInfo.index].gameObject);
                    }
                }
            }
            if (action is GameActionDamage)
            {
                GameActionDamage castedAction = (GameActionDamage)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToDamage)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        gameState.PlayerChampions[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                        foreach (Effects effect in gameState.PlayerEffects)
                        {
                            effect.TakeDamage(targetAndAmount.amount);
                        }
                        gameState.ClearEffects();
                    }
                    if (targetAndAmount.targetInfo.whichList.opponentLandmarks)
                    {
                        gameState.PlayerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        gameState.OpponentChampions[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                    if (targetAndAmount.targetInfo.whichList.myLandmarks)
                    {
                        gameState.OpponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                }
                if (gameState.OpponentChampion.Animator != null)
                    gameState.OpponentChampion.Animator.SetTrigger("Attack");

            }            
            if (action  is GameActionShield)
            {
                GameActionShield castedAction = (GameActionShield)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToShield)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        gameState.PlayerChampions[targetAndAmount.targetInfo.index].GainShield(targetAndAmount.amount);
                        Tuple<string, bool> tuple = new Tuple<string, bool>(gameState.PlayerChampions[targetAndAmount.targetInfo.index].Champion.ChampionName, false);
                        EffectController.Instance.ActiveShield(tuple, targetAndAmount.amount, gameState.PlayerChampions[targetAndAmount.targetInfo.index].gameObject);
                    }

                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        gameState.OpponentChampions[targetAndAmount.targetInfo.index].GainShield(targetAndAmount.amount);
                        Tuple<string, bool> tuple = new Tuple<string, bool>(gameState.OpponentChampions[targetAndAmount.targetInfo.index].Champion.ChampionName, true);
                        EffectController.Instance.ActiveShield(tuple, targetAndAmount.amount, gameState.OpponentChampions[targetAndAmount.targetInfo.index].gameObject);
                    }
                }

            }            
            if (action  is GameActionSwitchActiveChamp)
            {
                GameActionSwitchActiveChamp castedAction = (GameActionSwitchActiveChamp)action;

                gameState.SwapChampionOnline(castedAction.targetToSwitch,castedAction.championDied);

                if (castedAction.targetToSwitch.whichList.opponentChampions)               
                    gameState.PlayerChampion.Champion.WhenCurrentChampion();

            }            
            if (action is GameActionDestroyLandmark)
            {
    
                GameActionDestroyLandmark theAction = (GameActionDestroyLandmark)action;

                for (int i = 0; i < theAction.landmarksToDestroy.Count; i++)
                {   
                    TargetInfo targetInfo = theAction.landmarksToDestroy[i];
                    gameState.DestroyLandmark(targetInfo);
                }

            }            
            if (action is GameActionRemoveCardsGraveyard)
            {   
                GameActionRemoveCardsGraveyard castedAction = (GameActionRemoveCardsGraveyard)action;
                
                foreach(TargetInfo targetInfo in castedAction.cardsToRemoveGraveyard)
                {
                    ListEnum listEnum = targetInfo.whichList; 

                    if(listEnum.myGraveyard)
                    {
                        Graveyard.Instance.GraveyardOpponent.RemoveAt(targetInfo.index);
                    }
                    if(listEnum.opponentGraveyard)
                    {
                        Graveyard.Instance.GraveyardPlayer.RemoveAt(targetInfo.index);
                    }
                }
            }            
            if (action  is GameActionPlayCard)
            {
                GameActionPlayCard castedAction = (GameActionPlayCard)action;

                Card cardPlayed = register.cardRegister[castedAction.cardAndPlacement.CardName];

                if (castedAction.cardAndPlacement.Placement.whichList.myGraveyard)
                    Graveyard.Instance.GraveyardPlayer.Add(cardPlayed);
                else if (castedAction.cardAndPlacement.Placement.whichList.opponentGraveyard)
                    Graveyard.Instance.GraveyardOpponent.Add(cardPlayed);

                gameState.ShowPlayedCard(cardPlayed, true, castedAction.manaCost);

                ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
                actionOfPlayer.EnemyMana -= castedAction.manaCost;

                if (cardPlayed is AttackSpell)
                {
                    EffectController.Instance.PlayAttackEffect(gameState.OpponentChampion);
                }
                actionOfPlayer.ChangeCardOrder(false, actionOfPlayer.HandOpponent.cardsInHand[actionOfPlayer.HandOpponent.cardsInHand.Count - 1].GetComponent<CardDisplay>());
            }    
            if (action  is GameActionOpponentDiscardCard)
            {   
                GameActionOpponentDiscardCard castedAction = (GameActionOpponentDiscardCard)action;
                List<string> discardedCards = new List<string>();
                if(castedAction.isRandom)
                {
                    for (int i = 0; i < castedAction.amountOfCardsToDiscard; i++)
                    {
                        if (ActionOfPlayer.Instance.HandPlayer.cardsInHand.Count > 0)
                        {
                            if (castedAction.discardCardToOpponentGraveyard)
                            {
                                discardedCards.Add(ActionOfPlayer.Instance.DiscardWhichCard(false));
                            }
                            else
                            {
                                discardedCards.Add(ActionOfPlayer.Instance.DiscardWhichCard(true));
                            }
                            
                        }
                    }

                    RequestDiscardCard discardCardRequest = new RequestDiscardCard(discardedCards, castedAction.discardCardToOpponentGraveyard);
                    discardCardRequest.whichPlayer = ClientConnection.Instance.playerId;
                    ClientConnection.Instance.AddRequest(discardCardRequest, gameState.RequestEmpty);
                }
                else
                {   
                    ListEnum listEnum = new ListEnum();
                    listEnum.myHand = true;
                    Choice.Instance.ChoiceMenu(listEnum, castedAction.amountOfCardsToDiscard, WhichMethod.DiscardCard, null, 2f);
                }
            }  
            if (action  is GameActionGameSetup)
            {   

                print("Hej hej nu startar spelet");

				GameActionGameSetup castedAction = (GameActionGameSetup)action;

                Setup.Instance.opponentChampions.Clear();
                foreach (string stringen in castedAction.opponentChampions)
                {
                    Setup.Instance.opponentChampions.Add(stringen);
                }

                if (castedAction.reciprocate)
                {
					RequestGameSetup request = new RequestGameSetup();
					request.whichPlayer = ClientConnection.Instance.playerId;
					request.reciprocate = false;
                    request.opponentChampions = Setup.Instance.myChampions;

                    Setup.Instance.shouldStartGame = !castedAction.firstTurn;

					ClientConnection.Instance.AddRequest(request, EmptyRequest);
				}

                CreateScene();

			}
           
            if (action is GameActionAddSpecificCardToHand)
            {

                GameActionAddSpecificCardToHand castedAction = (GameActionAddSpecificCardToHand)action; 

                Deck.Instance.AddCardToDeckOpponent(CardRegister.Instance.cardRegister[castedAction.cardToAdd]);
				ActionOfPlayer.Instance.DrawCardPlayer(1, CardRegister.Instance.cardRegister[castedAction.cardToAdd], false);
            }
            if (action is GameActionPassPriority)
            {
                GameActionPassPriority castedAction = (GameActionPassPriority)action;
                gameState.HasPriority = true;
            }
            if (action is GameActionPlayLandmark)
            {
                GameActionPlayLandmark castedAction = (GameActionPlayLandmark)action;
                gameState.LandmarkPlaced(castedAction.landmarkToPlace.Placement.index, (Landmarks)CardRegister.Instance.cardRegister[castedAction.landmarkToPlace.CardName], true);
                gameState.ShowPlayedCard(CardRegister.Instance.cardRegister[castedAction.landmarkToPlace.CardName], true, -1);
                ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
			}

            if (action is GameActionStopSwapping)
            {

                print("den swappar ej");
                GameActionStopSwapping castedAction = (GameActionStopSwapping)action;

                gameState.CanSwap = castedAction.canSwap;
            }

            if (gameState != null)
                gameState.Refresh();

            if (!action.errorMessage.Equals(""))
            {
                print(action.errorMessage); 
            }
        }
    }

    public void CreateScene()
    {
        SceneManager.LoadScene(loadScene);
    }


    // Update is called once per frame
    void FixedUpdate()
    {   if(hasJoinedLobby && !hasEstablishedEnemurator)
        {
           StartCoroutine(SendRequest());
            hasEstablishedEnemurator = true;
        }
    }
    
    private IEnumerator SendRequest()
    {

        while(true)
        {
            RequestOpponentActions request = new RequestOpponentActions(ClientConnection.Instance.playerId, true);

            clientConnection.AddRequest(request, PerformOpponentsActions);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void EmptyRequest(ServerResponse response) {}
}