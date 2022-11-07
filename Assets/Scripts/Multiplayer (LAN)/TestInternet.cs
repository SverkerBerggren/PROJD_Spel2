using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestInternet : MonoBehaviour
{
    public GameObject gameObjectToDeActivatePlayer1;
    public GameObject gameObjectToActivatePlayer1;
    public GameObject gameObjectToActivatePlayer2;
    public GameObject gameObjectToDeActivatePlayer2;

   private Scene gameplayScene;

    ClientConnection clientConnection;

    [SerializeField] private string loadScene;
    bool hasEstablishedEnemurator = false; 



    public GameState gameState;
    public CardRegister register;

   public  bool hasJoinedLobby = false;
    //   public int LocalPlayerNumber; 

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {

        // System.Threading.Thread.(sendRequest(new ClientRequest()));

        clientConnection = ClientConnection.Instance;
        
    }


    public void PerformOpponentsActions(ServerResponse response)
    {   
        gameState = GameState.Instance;
        register = CardRegister.Instance;

        foreach (GameAction action in response.OpponentActions)
        {
            
            if(action.cardPlayed)
            {
                //print("Skiter det sig i perform oppnent action " + action.cardId);
                PlayCard(action.cardId);

                Destroy(GameObject.Find("Card (1)"));
            }

            print("vilket object typ ar grejen " + action.GetType());
            if (action is GameActionEndTurn )
            {
                // print("skickar den en gameAction end turn");
                gameState.EndTurn();

            }

            if (action is GameActionDrawCard)
            {
                print("skickar den en gameAction draw card");
                GameActionDrawCard theAction = (GameActionDrawCard)action;
                
                if(theAction.amountToDrawOpponent > 0)
                {
                    gameState.DrawCard(theAction.amountToDrawOpponent, null); 
                }
                if(theAction.amountToDraw > 0)
                {
                    StartCoroutine(gameState.DrawCardOpponent(theAction.amountToDraw,null));

                }
                //Draw card opponents

            }
            if (action is GameActionDiscardCard)
            {
                print("skickar den en gameAction discard");




                GameActionDiscardCard theAction = (GameActionDiscardCard)action;



                foreach(string card in theAction.listOfCardsDiscarded)
                {
                    Graveyard.Instance.AddCardToGraveyardOpponent(register.cardRegister[card]);
                    ActionOfPlayer.Instance.handOpponent.cardsInHand[0].GetComponent<CardDisplay>().card = null;

                }

            }
            if (action is GameActionHeal)
            {

                GameActionHeal castedAction = (GameActionHeal)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToHeal)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        GameState.Instance.playerChampions[targetAndAmount.targetInfo.index].champion.HealChampion(targetAndAmount.amount);
                    }

                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        GameState.Instance.opponentChampions[targetAndAmount.targetInfo.index].champion.HealChampion(targetAndAmount.amount);
                    }
                }
                print("hur mycket skulle healen heala " + castedAction.targetsToHeal[0].amount);
            }
            if (action is GameActionDamage)
            {
                GameActionDamage castedAction = (GameActionDamage)action;
				print("skickar den en gameAction damage");

				foreach (TargetAndAmount targetAndAmount in castedAction.targetsToDamage)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        GameState.Instance.playerChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount, GameState.Instance.playerChampion.gameObject);
                    }
                    if (targetAndAmount.targetInfo.whichList.opponentLandmarks)
                    {
                        GameState.Instance.playerLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        GameState.Instance.opponentChampions[targetAndAmount.targetInfo.index].champion.TakeDamage(targetAndAmount.amount, GameState.Instance.opponentChampion.gameObject);
                    }
                    if (targetAndAmount.targetInfo.whichList.myLandmarks)
                    {
                        GameState.Instance.opponentLandmarks[targetAndAmount.targetInfo.index].TakeDamage(targetAndAmount.amount);
                    }
                }
                

                //GameActionDamage theAction = (GameActionDamage)action;

                //Draw card opponents

            }            
            if (action  is GameActionShield)
            {
                print("skickar den en gameAction Shield");

                GameActionShield castedAction = (GameActionShield)action;

                foreach (TargetAndAmount targetAndAmount in castedAction.targetsToShield)
                {
                    if (targetAndAmount.targetInfo.whichList.opponentChampions)
                    {
                        GameState.Instance.playerChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
                    }

                    if (targetAndAmount.targetInfo.whichList.myChampions)
                    {
                        GameState.Instance.opponentChampions[targetAndAmount.targetInfo.index].champion.GainShield(targetAndAmount.amount);
                    }
                }

            }            
            if (action  is GameActionSwitchActiveChamp)
            {
                print("skickar den en gameAction switch active champion");
                //GameActionSwitchActiveChamp theAction = (GameActionSwitchActiveChamp)action;

                GameActionSwitchActiveChamp castedAction = (GameActionSwitchActiveChamp)action;

                GameState.Instance.SwapChampionWithTargetInfo(castedAction.targetToSwitch,castedAction.championDied);
                
                //Draw card opponents

            }            
            if (action is GameActionDestroyLandmark)
            {
                print("skickar den en gameAction destroylandmark");
                GameActionDestroyLandmark theAction = (GameActionDestroyLandmark)action;

                for (int i = 0; i < theAction.landmarksToDestroy.Count; i++)
                {   
                    TargetInfo targetInfo = theAction.landmarksToDestroy[i];
                    GameState.Instance.DestroyLandmark(targetInfo);
                }

                //Draw card opponents

            }            
            if (action is GameActionRemoveCardsGraveyard)
            {   
                print("skickar den en gameAction remove card graveyard");

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
                

                //GameActionRemoveCardsGraveyard theAction = (GameActionRemoveCardsGraveyard)action;

                //Draw card opponents

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
                    GameState.Instance.ShowPlayedCardLandmark((Landmarks)cardPlayed);
                else
                    GameState.Instance.ShowPlayedCard(cardPlayed);

                ActionOfPlayer.Instance.handOpponent.cardsInHand[0].GetComponent<CardDisplay>().card = null;

                //bool test =  gameState.actionOfPlayer.handOpponent.cardsInHand.Remove(gameState.actionOfPlayer.handOpponent.cardsInHand[0]);


                //print("tog den bort kort fran handen " + test);
                //GameActionPlayCard theAction = (GameActionPlayCard)action;

                //Draw card opponents

            }    
            if (action  is GameActionOpponentDiscardCard)
            {   

                print("Game action opponent discard card");

                GameActionOpponentDiscardCard castedAction = (GameActionOpponentDiscardCard)action;
                List<string> discardedCards = new List<string>();
                for(int i = 0; i < castedAction.amountOfCardsToDiscard; i++)
                {
                    if (ActionOfPlayer.Instance.handPlayer.cardsInHand.Count > 0)
                    {
                        discardedCards.Add(gameState.DiscardWhichCard(true));
                    }
                }

                RequestDiscardCard discardCardRequest = new RequestDiscardCard(discardedCards);
                discardCardRequest.whichPlayer = ClientConnection.Instance.playerId;
                print("vad ar which player " + discardCardRequest.whichPlayer);
                ClientConnection.Instance.AddRequest(discardCardRequest, gameState.RequestDiscardCard);

                //bool test =  gameState.actionOfPlayer.handOpponent.cardsInHand.Remove(gameState.actionOfPlayer.handOpponent.cardsInHand[0]);


                //print("tog den bort kort fran handen " + test);
                //GameActionPlayCard theAction = (GameActionPlayCard)action;

                //Draw card opponents

            }  
            if (action  is GameActionGameSetup)
            {   

                print("Hej hej nu startar spelet");

				GameActionGameSetup castedAction = (GameActionGameSetup)action;
                if (castedAction.reciprocate)
                {
					ClientRequestGameSetup request = new ClientRequestGameSetup();
					request.whichPlayer = ClientConnection.Instance.playerId;
					request.reciprocate = false;
					ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);
				}


			}
           
            if (action.GetType(action.Type).Equals(typeof(GameActionAddSpecificCardToHand)))
            {
                print("skickar den en gameAction add specific card");
                GameActionAddSpecificCardToHand castedAction = (GameActionAddSpecificCardToHand)action; 

                GameState.Instance.DrawCardOpponent(1, CardRegister.Instance.cardRegister[castedAction.cardToAdd]);

               // GameActionAddSpecificCardToHand theAction = (GameActionAddSpecificCardToHand)action;

                //Draw card opponents

            }
            if (action.GetType(action.Type).Equals(typeof(GameActionPlayLandmark)))
            {
                print("spelar den en landmark");
                GameActionPlayLandmark castedAction = (GameActionPlayLandmark)action;

                GameState.Instance.LandmarkPlaced(castedAction.landmarkToPlace.placement.index, (Landmarks)CardRegister.Instance.cardRegister[castedAction.landmarkToPlace.cardName], true);

               //GameActionAddSpecificCardToHand theAction = (GameActionAddSpecificCardToHand)action;

                //Draw card opponents

            }

            







            if (!action.errorMessage.Equals("") /*&& !action.errorMessage == null*/)
            {
                print(action.errorMessage); 
            }
        }
    }

    public void PlayCardCallback(ServerResponse response)
    {
        PlayCard(response.cardId);
    }

    public void PlayCard(int cardId)
    {

      //  Instantiate(cards[cardId], GameObject.Find("CardHolder").transform); //WIP
       
    }

    public void CreateScene()
    {
        SceneManager.LoadScene(loadScene);
    }

//    public void playCard(ServerResponse response)
//    {
//        if (!response.cardPlayed)
//        {
//            return;
//        }
//        if (response.whichPlayer == 0)
//        {
//            gameObjectToDeActivatePlayer1.SetActive(false);
//            gameObjectToActivatePlayer1.SetActive(true);
//        }
//        else
//        { 
//            gameObjectToActivatePlayer2.SetActive(true);
//            gameObjectToDeActivatePlayer2.SetActive(false);
//        }
//    }


    // Update is called once per frame
    void FixedUpdate()
    {   if(hasJoinedLobby && !hasEstablishedEnemurator)
        {                        //InvokeRepeating(nameof(SendRequest), 0, 1);
                                 // hasEstablishedEnemurator = true;
                                 // 
                                 StartCoroutine(SendRequest());
                                 //    hasEstablishedEnemurator = true;
                                 //    

        

            /*if (waitTime < 0)
            {
                waitTime = 60;
                RequestOpponentActions request = new RequestOpponentActions(ClientConnection.Instance.playerId, true);

                clientConnection.AddRequest(request, PerformOpponentsActions);

            }*/
        }
    }
    

    private IEnumerator SendRequest()
    {
        //
        //            ClientRequest request = new ClientRequest();
        //
        //            request.isPolling = true;
        //
        //            request.whichPlayer = clientConnection.playerId;
        //
        //            clientConnection.AddRequest(request, playCard);

        RequestOpponentActions request = new RequestOpponentActions(ClientConnection.Instance.playerId, true);

        clientConnection.AddRequest(request, PerformOpponentsActions);
       
        yield return new WaitForSeconds(0);
    }


}



// public class ServerResponse
// {
// 
// }
// public class ClientRequest
// {
//     public int type;
//     public int whichPlayer;
//     public bool disableCard;
// }