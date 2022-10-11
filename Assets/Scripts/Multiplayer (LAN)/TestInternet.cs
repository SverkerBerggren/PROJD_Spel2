using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestInternet : MonoBehaviour
{
    public GameObject gameObjectToDeActivatePlayer1;
    public GameObject gameObjectToActivatePlayer1;
    public GameObject gameObjectToActivatePlayer2;
    public GameObject gameObjectToDeActivatePlayer2;

    public GameObject sceneToActivate;
    public GameObject sceneToDeActivate;

    ClientConnection clientConnection;

    bool hasEstablishedEnemurator = false; 


    public Dictionary<int, GameObject> cards  = new Dictionary<int, GameObject>();

    public GameObject cardToPlay;
    int waitTime = 60; 
   public  bool hasJoinedLobby = false; 
 //   public int LocalPlayerNumber; 

    // Start is called before the first frame update
    void Start()
    {
        cards.Add(1, cardToPlay);
        // System.Threading.Thread.(sendRequest(new ClientRequest()));

        clientConnection = ClientConnection.Instance;
        
        
    }


    public void PerformOpponentsActions(ServerResponse response)
    {
        print("vilket player id har man " + ClientConnection.Instance.playerId) ;

        print("listan av game actions  " + response.OpponentActions.Count);
        foreach (GameAction action in response.OpponentActions)
        {
            
            if(action.cardPlayed)
            {
                //print("Skiter det sig i perform oppnent action " + action.cardId);
                PlayCard(action.cardId);
                print("har motstandaren gjort en action");
                Destroy(GameObject.Find("Card (1)"));
            }

            if(typeof(GameAction) == typeof(GameActionEndTurn))
            {
                GameState.Instance.SwitchTurn(response);

                GameState.Instance.hasPriority = true; 
            }
        }
    }

    public void PlayCardCallback(ServerResponse response)
    {
        PlayCard(response.cardId);
    }

    public void PlayCard(int cardId)
    {
        print("Vilket id ar det " + cardId);
        Instantiate(cards[cardId], GameObject.Find("CardHolder").transform); //WIP
        print("kom den forbi instantiate ");
    }

    public void CreateScene()
    {
    //    print("kommer den till test internet");
    //
    //    sceneToActivate.SetActive(true);
    //    sceneToDeActivate.SetActive(false);
    //    if (clientConnection.playerId == 0)
    //    {
    //        gameObjectToDeActivatePlayer1.SetActive(true);
    //    }
    //    else
    //    {
    //        gameObjectToDeActivatePlayer2.SetActive(true);
    //    }
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
                                 //    StartCoroutine(SendRequest());
                                 //    hasEstablishedEnemurator = true;
                                 //    

            waitTime -= 1;

            if (waitTime < 0)
            {
                waitTime = 60;
                RequestOpponentActions request = new RequestOpponentActions(ClientConnection.Instance.playerId, true);
                print("kommer den till add request");
                clientConnection.AddRequest(request, PerformOpponentsActions);
                print("den klarade det");
            }
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
        print("kommer den till add request");
        clientConnection.AddRequest(request, PerformOpponentsActions);
        print("den klarade det");
        yield return new WaitForSeconds(1);
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