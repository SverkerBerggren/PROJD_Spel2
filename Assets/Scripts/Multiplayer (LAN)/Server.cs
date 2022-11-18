using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using System.Text; 

public class Server
{
    System.Net.Sockets.TcpListener m_Listener;
    bool m_Stopping = false;

    public Dictionary<int, bool> hasPLayedCard  = new Dictionary<int, bool>();

    public List<GameAction> player1Actions = new List<GameAction>(); 
    public List<GameAction> player2Actions = new List<GameAction>(); 

    public static Int32 ParseBigEndianInteger(byte[] BytesToParse, int ByteOffset)
    {
        Int32 ReturnValue = 0;
        if (BytesToParse.Length < ByteOffset + 4)
        {
            throw new Exception("Unsufficient bytes to parse big endian integer");
        }
        for (int i = 0; i < 4; i++)
        {
            ReturnValue <<= 8;
            ReturnValue += BytesToParse[ByteOffset + i];
        }

        return (ReturnValue);
    }
    public static void WriteBigEndianInteger(byte[] OutArray, uint IntegerToWrite, int BufferOffset)
    {
        for (int i = 0; i < 4; i++)
        {
            OutArray[i + BufferOffset] = (byte)(IntegerToWrite >> (4 * 8 - ((i + 1) * 8)));
        }
    }
    public static MBJson.JSONObject ParseJsonObject(System.IO.Stream Stream)
    {
        MBJson.JSONObject ReturnValue = new MBJson.JSONObject();
        byte[] LengthBuffer = new byte[4];
        int ReadBytes = Stream.Read(LengthBuffer, 0, 4);
        if (ReadBytes < 4)
        {
            throw new Exception("Insufficient bytes to parse JSON object length");
        }
        int DataLength = ParseBigEndianInteger(LengthBuffer, 0);
        byte[] JSONData = new byte[DataLength];
        ReadBytes = Stream.Read(JSONData, 0, DataLength);
        if (ReadBytes < DataLength)
        {
            throw new Exception("Insufficient bytes sent for json object");
        }
        int temp;
        ReturnValue = MBJson.JSONObject.ParseJSONObject(JSONData, 0, out temp);
        return (ReturnValue);
    }

    void p_HandleConnection(object ConnectionToHandle)
    {
        System.Net.Sockets.TcpClient Connection = (System.Net.Sockets.TcpClient)ConnectionToHandle;
        while (Connection.Connected)
        {
            ClientRequest NewRequest = MBJson.JSONObject.DeserializeObject<ClientRequest>(ParseJsonObject(Connection.GetStream()));
            ServerResponse Response = HandleClientRequest(NewRequest);
            byte[] BytesToSend = SerializeJsonObject(MBJson.JSONObject.SerializeObject(Response));
            Connection.GetStream().Write(BytesToSend);
        }
        Connection.Close();
    }
    void p_Listen()
    {
        m_Listener.Start();
        while (!m_Stopping)
        {
            System.Net.Sockets.TcpClient NewConnection = m_Listener.AcceptTcpClient();
            Thread ConnectionThread = new Thread(this.p_HandleConnection);
            ConnectionThread.Start(NewConnection);
        }
    }
    public void StartServer(int Port)
    {
        m_Listener = new System.Net.Sockets.TcpListener(Port);
        Thread ListenerThread = new Thread(this.p_Listen);
        ListenerThread.Start();


    }


    public static byte[] SerializeJsonObject(MBJson.JSONObject ObjectToSerialize)
    {
        string ObjectString = ObjectToSerialize.ToString();
        byte[] ObjectBytes = System.Text.UTF8Encoding.UTF8.GetBytes(ObjectString);
        byte[] ReturnValue = new byte[ObjectBytes.Length + 4];
        WriteBigEndianInteger(ReturnValue, (uint)ObjectBytes.Length, 0);
        ObjectBytes.CopyTo(ReturnValue, 4);
        return (ReturnValue);
    }
    
    public ServerResponse HandleClientRequest(ClientRequest requestToHandle)
    {
        if(requestToHandle is RequestOpponentActions)
        {
            return HandleRequestActions(requestToHandle);
        }
        if(requestToHandle is RequestEndTurn)
        {
            RequestEndTurn castedRequest = (RequestEndTurn)requestToHandle;

            return HandleEndTurn(castedRequest);
        }
        if (requestToHandle is RequestDrawCard)
        {
            RequestDrawCard castedRequest = (RequestDrawCard)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleDrawCard(castedRequest);
        }
        if (requestToHandle is RequestDiscardCard)
        {
            RequestDiscardCard castedRequest = (RequestDiscardCard)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleDiscardCard(castedRequest);
        }
        if (requestToHandle is RequestHealing)
        {
            RequestHealing castedRequest = (RequestHealing)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleHeal(castedRequest);
        }
        if (requestToHandle is RequestDamage)
        {
            RequestDamage castedRequest = (RequestDamage)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleDamage(castedRequest);
        }       
        if (requestToHandle is RequestShield)
        {
            RequestShield castedRequest = (RequestShield)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleShield(castedRequest);
        }       
        if (requestToHandle is RequestSwitchActiveChamps )
        {
            RequestSwitchActiveChamps castedRequest = (RequestSwitchActiveChamps)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleSwitchActiveChamp(castedRequest);
        }       
        if (requestToHandle is RequestDestroyLandmark)
        {
            RequestDestroyLandmark castedRequest = (RequestDestroyLandmark)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleDestroyLandmark(castedRequest);
        }       
        if (requestToHandle is RequestRemoveCardsGraveyard)
        {
            RequestRemoveCardsGraveyard castedRequest = (RequestRemoveCardsGraveyard)requestToHandle;
            castedRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleRemoveCardsGraveyard(castedRequest);
        }       
        if (requestToHandle is RequestPlayCard)
        {
            RequestPlayCard castedRequestTest = (RequestPlayCard)requestToHandle;

            castedRequestTest.whichPlayer = requestToHandle.whichPlayer;
            return HandlePlayCard(castedRequestTest);
        }       
        if (requestToHandle is RequestAddSpecificCardToHand)
        {
            //RequestAddSpecificCardToHand castedRequest = (RequestAddSpecificCardToHand)requestToHandle;
            RequestAddSpecificCardToHand testRequest = (RequestAddSpecificCardToHand)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleAddSpecificCardToHand(testRequest);
        }       
        if (requestToHandle is RequestOpponentDiscardCard)
        {
            //RequestAddSpecificCardToHand castedRequest = (RequestAddSpecificCardToHand)requestToHandle;
            RequestOpponentDiscardCard testRequest = (RequestOpponentDiscardCard)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleDiscardCardOpponent(testRequest);
        }    
        if (requestToHandle is RequestPlayLandmark)
        {
            //RequestAddSpecificCardToHand castedRequest = (RequestAddSpecificCardToHand)requestToHandle;
            RequestPlayLandmark testRequest = (RequestPlayLandmark)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandlePlayLandmark(testRequest);
        }
           
        if (requestToHandle is ClientRequestGameSetup)
        {
			//RequestAddSpecificCardToHand castedRequest = (RequestAddSpecificCardToHand)requestToHandle;
			ClientRequestGameSetup testRequest = (ClientRequestGameSetup)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleGameSetup(testRequest);
        }
        if (requestToHandle is RequestPassPriority)
        {
            //RequestAddSpecificCardToHand castedRequest = (RequestAddSpecificCardToHand)requestToHandle;
            RequestPassPriority testRequest = (RequestPassPriority)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandlePassPriority(testRequest);
        }

        GameAction errorMessage = new GameAction();
        errorMessage.errorMessage = "den kommer inte till ratt handle " + requestToHandle.Type +" " + requestToHandle.GetType() + " "+ (requestToHandle is RequestAddSpecificCardToHand);
        ServerResponse errorResponse = new ServerResponse();
        AddGameAction(errorResponse, errorMessage); 

        ServerResponse blank = new ServerResponse();
        return blank;
    }

    private ServerResponse HandlePlayCard(ClientRequest requestToHandle)
    {
        ServerResponse response = new ServerResponse();

        response.whichPlayer = requestToHandle.whichPlayer;

        response.cardId = requestToHandle.cardId;
        if (requestToHandle.hasPlayedCard)
        {
            GameAction gameAction = new GameAction();
            gameAction.cardId = requestToHandle.cardId;
            gameAction.cardPlayed = true;

            AddGameAction(response, gameAction);
        }

        return response; 
    }
    private ServerResponse HandleCreateScene(ClientRequest requestToHandle)
    {
        ServerResponse response = new ServerResponse();

        response.whichPlayer = requestToHandle.whichPlayer;

        response.cardId = requestToHandle.cardId;
        if (requestToHandle.hasPlayedCard)
        {
            GameAction gameAction = new GameAction();
            gameAction.cardId = requestToHandle.cardId;
            gameAction.cardPlayed = true;

            AddGameAction(response, gameAction);
        }

        return response;
    }
    private ServerResponse HandleEndTurn(RequestEndTurn requestToHandle)
    {
        ResponseEndTurn response = new ResponseEndTurn(requestToHandle.whichPlayer);

       

        GameActionEndTurn gameAction = new GameActionEndTurn(0);

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleDrawCard(RequestDrawCard requestToHandle)
    {
        ResponseDrawCard response = new ResponseDrawCard(requestToHandle.amountToDraw);
        
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDrawCard gameAction = new GameActionDrawCard(requestToHandle.amountToDraw);
        
        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandlePassPriority(RequestPassPriority requestToHandle)
    {
        ResponsePassPriority response = new ResponsePassPriority(requestToHandle.priority);
        
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionPassPriority gameAction = new GameActionPassPriority(requestToHandle.priority);
        
        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandlePlayLandmark(RequestPlayLandmark requestToHandle)
    {
        ResponsePlayLandmark response = new ResponsePlayLandmark(requestToHandle.landmarkToPlace);
        
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionPlayLandmark gameAction = new GameActionPlayLandmark(requestToHandle.landmarkToPlace);
        
        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleDiscardCardOpponent(RequestOpponentDiscardCard requestToHandle)
    {
        ResponseOpponentDiscardCard response = new ResponseOpponentDiscardCard(requestToHandle.amountOfCardsToDiscard);
        
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionOpponentDiscardCard gameAction = new GameActionOpponentDiscardCard(requestToHandle.amountOfCardsToDiscard);
        gameAction.isRandom = requestToHandle.isRandom;
        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleGameSetup(ClientRequestGameSetup requestToHandle)
    {
        ServerResponse response = new ServerResponse();
        
        response.whichPlayer = requestToHandle.whichPlayer;

		GameActionGameSetup gameAction = new GameActionGameSetup();
        gameAction.reciprocate = requestToHandle.reciprocate;
        gameAction.opponentChampions = requestToHandle.opponentChampions;
        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleDiscardCard(RequestDiscardCard requestToHandle)
    {
        ResponseDiscardCard response = new ResponseDiscardCard(new List<string>( requestToHandle.listOfCardsDiscarded));

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDiscardCard gameAction = new GameActionDiscardCard(new List<string>(requestToHandle.listOfCardsDiscarded));

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleHeal(RequestHealing requestToHandle)
    {
        ResponseHeal response = new ResponseHeal(requestToHandle.targetsToHeal);

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionHeal gameAction = new GameActionHeal(requestToHandle.targetsToHeal);

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleDamage(RequestDamage requestToHandle)
    {
        ResponseDamage response = new ResponseDamage(requestToHandle.targetsToDamage);

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDamage gameAction = new GameActionDamage(new  List<TargetAndAmount>(requestToHandle.targetsToDamage));

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleShield(RequestShield requestToHandle)
    {
        ResponseShield response = new ResponseShield(requestToHandle.targetsToShield);

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionShield gameAction = new GameActionShield(new List<TargetAndAmount>(requestToHandle.targetsToShield));

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleSwitchActiveChamp(RequestSwitchActiveChamps requestToHandle)
    {
        ResponseSwitchActiveChamp response = new ResponseSwitchActiveChamp(requestToHandle.targetToSwitch);

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionSwitchActiveChamp gameAction = new GameActionSwitchActiveChamp(requestToHandle.targetToSwitch);
        gameAction.championDied = requestToHandle.championDied;
        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleDestroyLandmark(RequestDestroyLandmark requestToHandle)
    {
        ResponseDestroyLandmark response = new ResponseDestroyLandmark(new List<TargetInfo>(requestToHandle.landmarksToDestroy));

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDestroyLandmark gameAction = new GameActionDestroyLandmark( requestToHandle.landmarksToDestroy);

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleRemoveCardsGraveyard(RequestRemoveCardsGraveyard requestToHandle)
    {
        ResponseRemoveCardsGraveyard response = new ResponseRemoveCardsGraveyard(new List<TargetInfo>(requestToHandle.cardsToRemoveGraveyard));

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionRemoveCardsGraveyard gameAction = new GameActionRemoveCardsGraveyard( requestToHandle.cardsToRemoveGraveyard);

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandlePlayCard(RequestPlayCard requestToHandle)
    {
        ResponsePlayCard response = new ResponsePlayCard(requestToHandle.cardAndPlacement);

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionPlayCard gameAction = new GameActionPlayCard( requestToHandle.cardAndPlacement    );

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleAddSpecificCardToHand(RequestAddSpecificCardToHand requestToHandle)
    {
        ResponseAddSpecificCardToHand response = new ResponseAddSpecificCardToHand(requestToHandle.cardToAdd);

        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionAddSpecificCardToHand gameAction = new GameActionAddSpecificCardToHand( requestToHandle.cardToAdd);

        AddGameAction(response, gameAction);
        return response;
    }
    private ServerResponse HandleRequestActions(ClientRequest requestToHandle)
    {
        ServerResponse response = new ServerResponse();

        
        int player = requestToHandle.whichPlayer == 0 ? 1 : 0;
        if (player == 1)
        {
            response.OpponentActions = new List<GameAction>(player2Actions);
            player2Actions.Clear();
        }
        else
        {
            response.OpponentActions = new List<GameAction>(player1Actions);
            player1Actions.Clear();
        }

        return response;
    }

    private void AddGameAction(ServerResponse response, GameAction gameAction)
    {   
        
        if (response.whichPlayer == 1)
        {
            player2Actions.Add(gameAction);
        }
        else
        {
            player1Actions.Add(gameAction);
        }
    }

    ~Server()
    {
        m_Stopping = true;
    }



}