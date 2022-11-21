using Newtonsoft.Json.Serialization;
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

        foreach (GameAction action in response.OpponentActions)
        {
            print("vilket object typ ar grejen " + action.GetType() + action.Type);
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
                    Graveyard.Instance.AddCardToGraveyardOpponent(register.cardRegister[card]);
                    ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
                    //actionOfPlayer.ChangeCardOrder(false, actionOfPlayer.handOpponent.cardsInHand[0].GetComponent<CardDisplay>());
                }

            }
            if (action is GameActionHeal)
            {

                GameActionHeal castedAction = (GameActionHeal)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToHeal)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        gameState.playerChampions[targetAndAmount.targetInfo.index].champion.HealChampion(targetAndAmount.amount);
                    }

                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        gameState.opponentChampions[targetAndAmount.targetInfo.index].champion.HealChampion(targetAndAmount.amount);
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
                        gameState.playerChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount, gameState.playerChampion.gameObject);
                    }
                    if (targetAndAmount.targetInfo.whichList.opponentLandmarks)
                    {
                        gameState.playerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        gameState.opponentChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount, gameState.opponentChampion.gameObject);
                    }
                    if (targetAndAmount.targetInfo.whichList.myLandmarks)
                    {
                        gameState.opponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                }
                if (gameState.opponentChampion.animator != null)
                    gameState.opponentChampion.animator.SetTrigger("Attack");

            }            
            if (action  is GameActionShield)
            {
                GameActionShield castedAction = (GameActionShield)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToShield)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        gameState.playerChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
                    }

                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        gameState.opponentChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
                    }
                }

            }            
            if (action  is GameActionSwitchActiveChamp)
            {
                GameActionSwitchActiveChamp castedAction = (GameActionSwitchActiveChamp)action;

                gameState.SwapChampionWithTargetInfo(castedAction.targetToSwitch,castedAction.championDied);

                if (castedAction.targetToSwitch.whichList.opponentChampions)               
                    gameState.playerChampion.champion.WhenCurrentChampion();

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
                        Graveyard.Instance.graveyardOpponent.RemoveAt(targetInfo.index);
                    }
                    if(listEnum.opponentGraveyard)
                    {
                        Graveyard.Instance.graveyardPlayer.RemoveAt(targetInfo.index);
                    }
                }
            }            
            if (action  is GameActionPlayCard)
            {
                GameActionPlayCard castedAction = (GameActionPlayCard)action;

                Card cardPlayed = register.cardRegister[castedAction.cardAndPlacement.cardName];

                if (castedAction.cardAndPlacement.placement.whichList.myGraveyard)
                {
                    Graveyard.Instance.graveyardOpponent.Add(cardPlayed);
                }

                Graveyard.Instance.graveyardOpponent.Add(cardPlayed);

                if (cardPlayed.typeOfCard == CardType.Landmark)
                    gameState.ShowPlayedCardLandmark((Landmarks)cardPlayed);
                else
                    gameState.ShowPlayedCard(cardPlayed);
                ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;

                actionOfPlayer.handOpponent.FixCardOrderInHand();
                actionOfPlayer.ChangeCardOrder(false, actionOfPlayer.handOpponent.cardsInHand[actionOfPlayer.handOpponent.cardsInHand.Count - 1].GetComponent<CardDisplay>());

            }    
            if (action  is GameActionOpponentDiscardCard)
            {   
                GameActionOpponentDiscardCard castedAction = (GameActionOpponentDiscardCard)action;
                List<string> discardedCards = new List<string>();
                if(castedAction.isRandom)
                {
                    for (int i = 0; i < castedAction.amountOfCardsToDiscard; i++)
                    {
                        if (ActionOfPlayer.Instance.handPlayer.cardsInHand.Count > 0)
                        {

                            discardedCards.Add(ActionOfPlayer.Instance.DiscardWhichCard(true));
                        }
                    }

                    RequestDiscardCard discardCardRequest = new RequestDiscardCard(discardedCards);
                    discardCardRequest.whichPlayer = ClientConnection.Instance.playerId;
                    ClientConnection.Instance.AddRequest(discardCardRequest, gameState.RequestEmpty);
                }
                else
                {   
                    ListEnum listEnum = new ListEnum();
                    listEnum.myHand = true;
                    Choice.Instance.ChoiceMenu(listEnum, castedAction.amountOfCardsToDiscard, WhichMethod.discardCard);
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

					ClientConnection.Instance.AddRequest(request, EmptyRequest);
				}

                CreateScene();

			}
           
            if (action is GameActionAddSpecificCardToHand)
            {

                GameActionAddSpecificCardToHand castedAction = (GameActionAddSpecificCardToHand)action; 

                ActionOfPlayer.Instance.handOpponent.deck.AddCardToDeckOpponent(CardRegister.Instance.cardRegister[castedAction.cardToAdd]);
				ActionOfPlayer.Instance.DrawCardPlayer(1, CardRegister.Instance.cardRegister[castedAction.cardToAdd], false);
            }
            if (action is GameActionPassPriority)
            {
                GameActionPassPriority castedAction = (GameActionPassPriority)action;
                gameState.hasPriority = true;
            }
            if (action is GameActionPlayLandmark)
            {
                GameActionPlayLandmark castedAction = (GameActionPlayLandmark)action;

                gameState.LandmarkPlaced(castedAction.landmarkToPlace.placement.index, (Landmarks)CardRegister.Instance.cardRegister[castedAction.landmarkToPlace.cardName], true);
                gameState.ShowPlayedCardLandmark((Landmarks)CardRegister.Instance.cardRegister[castedAction.landmarkToPlace.cardName]);
                ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
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
        }
    }
    
    private IEnumerator SendRequest()
    {
        RequestOpponentActions request = new RequestOpponentActions(ClientConnection.Instance.playerId, true);

        clientConnection.AddRequest(request, PerformOpponentsActions);
       
        yield return new WaitForSeconds(0);
    }

    private void EmptyRequest(ServerResponse response) {}
}