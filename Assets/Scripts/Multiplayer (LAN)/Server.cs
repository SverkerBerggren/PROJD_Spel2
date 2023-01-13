using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;


public class Server
{
    System.Net.Sockets.TcpListener m_Listener;
    bool m_Stopping = false;

    // public Dictionary<int, bool> hasPLayedCard = new Dictionary<int, bool>();

    public bool playingFromServer = false;

    public Dictionary<int, OngoingGame> onGoingGames = new Dictionary<int, OngoingGame>();

    public Integer currentGameId = new Integer(0);
    //   public Integer lobbyId = new Integer(0);


    public Dictionary<int, HostedLobby> hostedLobbys = new Dictionary<int, HostedLobby>();

    //   public Dictionary<int, int> uniqueIntegersHosted = new Dictionary<int, int>();

    public Integer uniqueInteger = new Integer(1);

    bool connectedToSQLServer = false;
    TcpClient SQLServerSocket = new TcpClient();

    private SemaphoreSlim addGameActionSemaphore = new SemaphoreSlim(0, 10);
    private List<GameActionToLog> gameActionsToAdd = new List<GameActionToLog>();

    private void ConnectToSQLServer()
    {   
        try   
        {
            SQLServerSocket.Connect("mrboboget.se", 54000);

            if (SQLServerSocket.Connected)
            {

                connectedToSQLServer = true;

                RequestMatchID();

                Thread gameActionLoop = new Thread(SQLAddGameActionLoop);

                gameActionLoop.Start();
            }
        }
        catch(Exception e)
        {
            string hej = e.Message;
        }


        
    } 

    public void SQLaddGameAction(GameActionToLog gameActionToLog)
    {   try
        {
            lock (gameActionsToAdd)
            {
                gameActionsToAdd.Add(gameActionToLog);
            }
            addGameActionSemaphore.Release();
        }
        catch(Exception e)
        {
            string errorMessage = e.Message;
        }


    }

    public void SQLAddGameActionLoop()
    {   try
        {
            while (connectedToSQLServer)
            {
                addGameActionSemaphore.Wait();
                GameActionToLog gameActionToLog;
                lock (gameActionsToAdd)
                {
                    gameActionToLog = gameActionsToAdd[0];
                    gameActionsToAdd.RemoveAt(0);
                }
                bool messageRead = false;
                while (!messageRead)
                {
                    byte[] buf = new byte[4096];
                    string message = "Request gameActionIndex:" + gameActionToLog.playerIndex + "" + gameActionToLog.gameId;
                    int messagLength = message.Length;
                    char firstChar = (char)(messagLength >> 24);
                    char secondChar = (char)(messagLength >> 16);
                    char thirdChar = (char)(messagLength >> 8);
                    char fourthChar = (char)(messagLength);
                    buf = new byte[messagLength + 4];
                    buf[0] = (byte)firstChar;
                    buf[1] = (byte)secondChar;
                    buf[2] = (byte)thirdChar;
                    buf[3] = (byte)fourthChar;

                    for (int i = 0; i < messagLength; i++)
                    {
                        buf[4 + i] = (byte)message[i];
                    }
                    string responseToUse = "";
                    //      buf.CopyTo(hej.ToCharArray(), hej.Length);
                    lock (SQLServerSocket)
                    {
                        SQLServerSocket.GetStream().Write(buf, 0, messagLength + 4);
                        //
                        responseToUse = "";
                        bool continueReading = true;
                        int totalBytesRecieved = 0;
                        byte[] newBuffer = new byte[4096];
                        int gameActionIndex = 0;
                        while (continueReading)
                        {

                            totalBytesRecieved += SQLServerSocket.GetStream().Read(newBuffer, totalBytesRecieved, 4096 - totalBytesRecieved);

                            if ((totalBytesRecieved - 4) == (newBuffer[0] * (1 << 24) + (newBuffer[1] * (1 << 16) + (newBuffer[2] * (1 << 8) + newBuffer[3]))))
                            {
                                char[] castedBuffer = System.Text.Encoding.UTF8.GetString(newBuffer).ToCharArray();
                                responseToUse = new String(castedBuffer, 4, (newBuffer[0] * (1 << 24) + (newBuffer[1] * (1 << 16) + (newBuffer[2] * (1 << 8) + newBuffer[3]))));
                                continueReading = false;
                                messageRead = true;
                                gameActionIndex = Convert.ToInt32(responseToUse) + 1;

                                responseToUse = Convert.ToString(gameActionIndex);
                            }
                        }
                    }

                    SendMessageToSQLServerThread("insert into MatchGameActionHistory VALUES(" + gameActionToLog.gameId + "," + gameActionToLog.playerIndex + "," + responseToUse + ", 0 ,'" + gameActionToLog.gameAction + "'" + ")");

                }

            }
        }
        catch(Exception e)
        {
            string hej = e.Message;
        }
       
    }

    private  void RequestMatchID()
    {   try
        {
            bool messageRead = false;
            while (!messageRead)
            {
                byte[] buf = new byte[4096];
                string message = "Request match id";
                int messagLength = message.Length;
                char firstChar = (char)(messagLength >> 24);
                char secondChar = (char)(messagLength >> 16);
                char thirdChar = (char)(messagLength >> 8);
                char fourthChar = (char)(messagLength);
                buf = new byte[messagLength + 4];
                buf[0] = (byte)firstChar;
                buf[1] = (byte)secondChar;
                buf[2] = (byte)thirdChar;
                buf[3] = (byte)fourthChar;

                for (int i = 0; i < messagLength; i++)
                {
                    buf[4 + i] = (byte)message[i];
                }

                //      buf.CopyTo(hej.ToCharArray(), hej.Length);

                lock (SQLServerSocket)
                {
                    SQLServerSocket.GetStream().Write(buf, 0, messagLength + 4);
                    //

                    bool continueReading = true;
                    int totalBytesRecieved = 0;
                    byte[] newBuffer = new byte[4096];
                    while (continueReading)
                    {

                        totalBytesRecieved += SQLServerSocket.GetStream().Read(newBuffer, totalBytesRecieved, 4096 - totalBytesRecieved);


                        if ((totalBytesRecieved - 4) == (newBuffer[0] * (1 << 24) + (newBuffer[1] * (1 << 16) + (newBuffer[2] * (1 << 8) + newBuffer[3]))))
                        {
                            char[] castedBuffer = System.Text.Encoding.UTF8.GetString(newBuffer).ToCharArray();
                            string response = new String(castedBuffer, 4, castedBuffer.Length - 4);
                            continueReading = false;
                            messageRead = true;
                            Integer integerToReturn = new Integer(Convert.ToInt32(response));

                            integerToReturn.value += 1;

                            lock (currentGameId)
                            {
                                currentGameId = integerToReturn;
                            }
                        }
                    }
                }

            }
        }
        catch(Exception e)
        {
            string hej = e.Message;
        }


        

    }

    private void SendMessageToSQLServer( string messageToSend)
    {   try
        {
            bool messageRead = false;
            while (!messageRead)
            {
                byte[] buf = new byte[4096];
                string message = messageToSend;

                int messagLength = message.Length;

                char firstChar = (char)(messagLength >> 24);
                char secondChar = (char)(messagLength >> 16);
                char thirdChar = (char)(messagLength >> 8);
                char fourthChar = (char)(messagLength);
                buf = new byte[messagLength + 4];
                buf[0] = (byte)firstChar;
                buf[1] = (byte)secondChar;
                buf[2] = (byte)thirdChar;
                buf[3] = (byte)fourthChar;


                for (int i = 0; i < messagLength; i++)
                {
                    buf[4 + i] = (byte)message[i];
                }


                lock (SQLServerSocket)
                {
                    SQLServerSocket.GetStream().Write(buf, 0, messagLength + 4);
                    //

                    bool continueReading = true;
                    int totalBytesRecieved = 0;
                    byte[] newBuffer = new byte[4096];
                    while (continueReading)
                    {

                        totalBytesRecieved += SQLServerSocket.GetStream().Read(newBuffer, totalBytesRecieved, 4096 - totalBytesRecieved);

                        if ((totalBytesRecieved - 4) == (newBuffer[0] * (1 << 24) + (newBuffer[1] * (1 << 16) + (newBuffer[2] * (1 << 8) + newBuffer[3]))))
                        {
                            char[] castedBuffer = System.Text.Encoding.UTF8.GetString(newBuffer).ToCharArray();
                            string response = new String(castedBuffer, 4, castedBuffer.Length - 4);
                            continueReading = false;
                            messageRead = true;
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            string hej = e.Message;
        }
        
    }

    private void SendMessageToSQLServerThread(string message)
    {   
        try
        {
            //ParameterizedThreadStart threadStart = new ParameterizedThreadStart(SendMessageToSQLServerThread);
            Thread threadToActivate = new Thread(() => SendMessageToSQLServer(message));
            threadToActivate.Start();
        }
        catch(Exception e)
        {
            string hej = e.Message;
        }

    }


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
        int ReadBytes = 0;
        while(ReadBytes < 4)
        {
            int NewBytes = Stream.Read(LengthBuffer, ReadBytes, 4-ReadBytes);
            if(NewBytes == 0)
            {
                throw new Exception("Insufficient bytes to parse JSON object length");
            }
            ReadBytes += NewBytes;
        }
        int DataLength = ParseBigEndianInteger(LengthBuffer, 0);
        byte[] JSONData = new byte[DataLength];
        ReadBytes = 0;
        while(ReadBytes < DataLength)
        {
            int NewBytes = Stream.Read(JSONData, ReadBytes, DataLength-ReadBytes);
            if(NewBytes == 0)
            {
                throw new Exception("Insufficient bytes sent for json object");
            }
            ReadBytes += NewBytes;
        }
        int temp;
        ReturnValue = MBJson.JSONObject.ParseJSONObject(JSONData, 0, out temp);
        return (ReturnValue);
    }

    void p_HandleConnection(object ConnectionToHandle)
    {
        System.Net.Sockets.TcpClient Connection = (System.Net.Sockets.TcpClient)ConnectionToHandle;
        int localUniqueInteger = 0;
        lock (uniqueInteger)
        {
            localUniqueInteger = uniqueInteger.value;
            uniqueInteger.value += 1;
        }
        try
        {
            while (Connection.Connected)
            {
                ClientRequest NewRequest = MBJson.JSONObject.DeserializeObject<ClientRequest>(ParseJsonObject(Connection.GetStream()));
                ServerResponse response = new ServerResponse();
                if (NewRequest is RequestUniqueInteger)
                {
                    ResponseUniqueInteger temp = new ResponseUniqueInteger();
                    temp.UniqueInteger = localUniqueInteger;
                    response = temp;
                }
                else if (NewRequest is RequestHostLobby)
                {
                    StreamWriter temp = File.CreateText("Debug.txt");
                    temp.Write("kommer den hit");
                    temp.Close();

                    ResponseHostLobby responseHostLobby = new ResponseHostLobby();
                    RequestHostLobby castedRequest = (RequestHostLobby)NewRequest;
                    lock (hostedLobbys)
                    {
                        if (hostedLobbys.ContainsKey(castedRequest.uniqueInteger))
                        {
                            responseHostLobby.message = "already hosted lobby";

                            response = responseHostLobby;
                        }
                        else
                        {
                            //response.gameId = requestToHandle.gameId;
                            response.whichPlayer = NewRequest.whichPlayer;

                            responseHostLobby.lobbyName = castedRequest.lobbyName;
                            lock (currentGameId)
                            {
                                response.gameId = currentGameId.value;
                            }

                            responseHostLobby.lobbyId = localUniqueInteger;

                            HostedLobby lobbyTohost = new HostedLobby();
                            lobbyTohost.lobbyName = castedRequest.lobbyName;
                            lobbyTohost.lobbyId = localUniqueInteger;

                            response = responseHostLobby;




                            hostedLobbys.Add(localUniqueInteger, lobbyTohost);

                        }
                    }

                }
                else
                {
                    response = HandleClientRequest(NewRequest);

                }


                byte[] BytesToSend = SerializeJsonObject(MBJson.JSONObject.SerializeObject(response));


                Connection.GetStream().Write(BytesToSend, 0, BytesToSend.Length);
            }
        }
        catch (Exception e)
        {
            lock (hostedLobbys)
            {
                if (hostedLobbys.ContainsKey(localUniqueInteger))
                {
                    hostedLobbys.Remove(localUniqueInteger);
                }
            }
            lock (onGoingGames)
            {
                if (onGoingGames.ContainsKey(localUniqueInteger))
                {
                    onGoingGames.Remove(localUniqueInteger);
                }
            }
            StreamWriter temp = File.CreateText("ErrorMessage.txt");
            temp.Write(e.StackTrace.ToString());
            temp.Write(e.ToString());
            temp.Write(e.Message.ToString());

            temp.Close();
        }

        Connection.Close();
    }
    public void p_Listen()
    {
        Thread connectSQLThread = new Thread(ConnectToSQLServer);
        connectSQLThread.Start();

      //  m_Listener = new TcpListener(63000);
        m_Listener.Start();
        while (!m_Stopping)
        {
            System.Net.Sockets.TcpClient NewConnection = m_Listener.AcceptTcpClient();
            Thread ConnectionThread = new Thread(this.p_HandleConnection);
            ConnectionThread.Start(NewConnection);
        }
    }
    public void p_Listen(int port)
    {
        
        m_Listener = new System.Net.Sockets.TcpListener(port);
        m_Listener.Start();
        Thread connectSQLThread = new Thread(ConnectToSQLServer);
        connectSQLThread.Start();
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
        

        if (requestToHandle is RequestOpponentActions)
        {
            return HandleRequestActions(requestToHandle);
        }
        if (requestToHandle is RequestEndTurn)
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
        if (requestToHandle is RequestSwitchActiveChamps)
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
            RequestAddSpecificCardToHand testRequest = (RequestAddSpecificCardToHand)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleAddSpecificCardToHand(testRequest);
        }
        if (requestToHandle is RequestOpponentDiscardCard)
        {
            RequestOpponentDiscardCard testRequest = (RequestOpponentDiscardCard)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleDiscardCardOpponent(testRequest);
        }
        if (requestToHandle is RequestPlayLandmark)
        {
            RequestPlayLandmark testRequest = (RequestPlayLandmark)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandlePlayLandmark(testRequest);
        }

        if (requestToHandle is RequestGameSetup)
        {
            RequestGameSetup testRequest = (RequestGameSetup)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleGameSetup(testRequest);
        }
        if (requestToHandle is RequestPassPriority)
        {
            RequestPassPriority testRequest = (RequestPassPriority)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandlePassPriority(testRequest);
        }
        if (requestToHandle is RequestJoinLobby)
        {
            RequestJoinLobby testRequest = (RequestJoinLobby)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleJoinLobby(testRequest);
        }
        if (requestToHandle is RequestAvailableLobbies)
        {
            RequestAvailableLobbies testRequest = (RequestAvailableLobbies)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleRequestLobbies(testRequest);
        }
        if (requestToHandle is RequestStopSwapping)
        {
            RequestStopSwapping testRequest = (RequestStopSwapping)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleRequestStopSwapping(testRequest);
        }
        if (requestToHandle is RequestLogGameAction)
        {
            RequestLogGameAction testRequest = (RequestLogGameAction)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleLogAction(testRequest);
        }
        if (requestToHandle is RequestLogGameAction)
        {
            RequestLogGameAction testRequest = (RequestLogGameAction)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleLogAction(testRequest);
        }
        if (requestToHandle is RequestEndGame)
        {
            RequestEndGame testRequest = (RequestEndGame)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleEndGame(testRequest);
        }
        /*
        if (requestToHandle is RequestStopSwapping)
        {
			RequestStopSwapping testRequest = (RequestStopSwapping)requestToHandle;
            testRequest.whichPlayer = requestToHandle.whichPlayer;
            return HandleRequestStopSwapping(testRequest);
        }
        */


        GameAction errorMessage = new GameAction();
        errorMessage.errorMessage = "den kommer inte till ratt handle " + requestToHandle.Type + " " + requestToHandle.GetType() + " " + (requestToHandle is RequestAddSpecificCardToHand);
        ServerResponse errorResponse = new ServerResponse();
        AddGameAction(errorResponse, errorMessage, requestToHandle.gameId);

        ServerResponse blank = new ServerResponse();
        return blank;
    }

    private ServerResponse HandleEndTurn(RequestEndTurn requestToHandle)
    {
        ResponseEndTurn response = new ResponseEndTurn(requestToHandle.whichPlayer);

        response.gameId = requestToHandle.gameId;

        GameActionEndTurn gameAction = new GameActionEndTurn(0);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }


    private ServerResponse HandleDrawCard(RequestDrawCard requestToHandle)
    {
        ResponseDrawCard response = new ResponseDrawCard(requestToHandle.amountToDraw);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDrawCard gameAction = new GameActionDrawCard(requestToHandle.amountToDraw);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    
    private ServerResponse HandleEndGame(RequestEndGame requestToHandle)
    {

        lock(onGoingGames)
        {
            if(onGoingGames.ContainsKey(requestToHandle.gameId))
            {
                onGoingGames.Remove(requestToHandle.gameId);
            }
        }
        if(connectedToSQLServer)
        {
            GameActionToLog toLog = new GameActionToLog(requestToHandle.whichPlayer, "Won game");
            toLog.gameId = requestToHandle.gameId;
            toLog.playerIndex = requestToHandle.whichPlayer;

            SQLaddGameAction(toLog);

            if(requestToHandle.whichPlayer == 1)
            {
                GameActionToLog anotherToLog = new GameActionToLog(requestToHandle.whichPlayer, "lost game");
                toLog.gameId = requestToHandle.gameId;
                toLog.playerIndex = 0;

                SQLaddGameAction(anotherToLog);
            }
            else
            {
                GameActionToLog anotherToLog = new GameActionToLog(requestToHandle.whichPlayer, "lost game");
                toLog.gameId = requestToHandle.gameId;
                toLog.playerIndex = 1;

                SQLaddGameAction(anotherToLog);
            }
        }

        return new ServerResponse();
    }

    private ServerResponse HandlePassPriority(RequestPassPriority requestToHandle)
    {
        ResponsePassPriority response = new ResponsePassPriority(requestToHandle.priority);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionPassPriority gameAction = new GameActionPassPriority(requestToHandle.priority);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandlePlayLandmark(RequestPlayLandmark requestToHandle)
    {
        ResponsePlayLandmark response = new ResponsePlayLandmark(requestToHandle.landmarkToPlace);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionPlayLandmark gameAction = new GameActionPlayLandmark(requestToHandle.landmarkToPlace);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }
    private ServerResponse HandleHostLobby(RequestHostLobby requestToHandle)
    {
        //    ResponseHostLobby response = new ResponseHostLobby();
        //    //response.gameId = requestToHandle.gameId;
        //    response.whichPlayer = requestToHandle.whichPlayer;
        //
        //    response.lobbyName = requestToHandle.lobbyName;
        //    lock(currentGameId)
        //    {
        //        response.gameId = currentGameId.value;
        //    }
        //    lock(lobbyId)
        //    {
        //        response.lobbyId = lobbyId.value;
        //
        //        HostedLobby lobbyTohost = new HostedLobby();
        //        lobbyTohost.lobbyName = requestToHandle.lobbyName;
        //        lobbyTohost.lobbyId = lobbyId.value;
        //        lobbyId.value += 1;
        //    }
        //
        //
        //    lock(hostedLobbys)
        //    {
        //        hostedLobbys.Add(lobbyId.value -1, new HostedLobby());
        //    }
        //    
        //    
        //
        //   // currentGameId += 1;
        //
        //
        //
        //    //AddGameAction(response, gameAction, requestToHandle.gameId);
        //    return response;
        return null;
    }

    private ServerResponse HandleJoinLobby(RequestJoinLobby requestToHandle)
    {
        ResponseJoinLobby response = new ResponseJoinLobby();
        //response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;
        int temp = 0;
        lock (currentGameId)
        {
            response.gameId = currentGameId.value;


            hostedLobbys[requestToHandle.lobbyId].anotherPlayerJoind = true;
            hostedLobbys[requestToHandle.lobbyId].gameId = currentGameId.value;
            temp = currentGameId.value;
            currentGameId.value += 1;
        }


        OngoingGame newGame = new OngoingGame();

        lock (onGoingGames)
        {

            onGoingGames.Add(temp, newGame);
        }

        //hostedLobbys.Add(currentGameId, new HostedLobby());

        //AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleDiscardCardOpponent(RequestOpponentDiscardCard requestToHandle)
    {
        ResponseOpponentDiscardCard response = new ResponseOpponentDiscardCard(requestToHandle.amountOfCardsToDiscard, requestToHandle.discardCardToOpponentGraveyard);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionOpponentDiscardCard gameAction = new GameActionOpponentDiscardCard(requestToHandle.amountOfCardsToDiscard, requestToHandle.discardCardToOpponentGraveyard);
        gameAction.isRandom = requestToHandle.isRandom;
        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleGameSetup(RequestGameSetup requestToHandle)
    {
        ServerResponse response = new ServerResponse();
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        lock (hostedLobbys)
        {   if(hostedLobbys.ContainsKey(requestToHandle.lobbyId))
            {
                hostedLobbys.Remove(requestToHandle.lobbyId);
            }
        }
        
        

        GameActionGameSetup gameAction = new GameActionGameSetup();
        gameAction.reciprocate = requestToHandle.reciprocate;
        gameAction.opponentChampions = requestToHandle.opponentChampions;
        gameAction.firstTurn = requestToHandle.firstTurn;
        AddGameAction(response, gameAction, requestToHandle.gameId);

        if( connectedToSQLServer)
        {
            SendMessageToSQLServerThread("insert into MatchDatabase VALUES( "+ (requestToHandle.gameId) + ",1,2 )");

            foreach (CardAndAmount cardAndAmount in requestToHandle.deckList)
            {
                SendMessageToSQLServerThread("insert into DeckList VALUES("+requestToHandle.gameId+"," + requestToHandle.whichPlayer + "," + "'" + cardAndAmount.cardName + "'," + cardAndAmount.amount + ")");
            }

            foreach (string champ in requestToHandle.opponentChampions)
            {
                SendMessageToSQLServerThread("insert into DeckList VALUES(" +requestToHandle.gameId+","+ requestToHandle.whichPlayer + ",'" + champ + "'," + 1 + ")");
            }

        }

      


        return response;
    }

    private ServerResponse HandleDiscardCard(RequestDiscardCard requestToHandle)
    {
        ResponseDiscardCard response = new ResponseDiscardCard(new List<string>(requestToHandle.listOfCardsDiscarded), requestToHandle.discardCardToOpponentGraveyard);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;
        response.listEnum = requestToHandle.listEnum;
        GameActionDiscardCard gameAction = new GameActionDiscardCard(new List<string>(requestToHandle.listOfCardsDiscarded), requestToHandle.discardCardToOpponentGraveyard);
        gameAction.listEnum = requestToHandle.listEnum;
        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleHeal(RequestHealing requestToHandle)
    {
        ResponseHeal response = new ResponseHeal(requestToHandle.targetsToHeal);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionHeal gameAction = new GameActionHeal(requestToHandle.targetsToHeal);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleDamage(RequestDamage requestToHandle)
    {
        ResponseDamage response = new ResponseDamage(requestToHandle.targetsToDamage);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDamage gameAction = new GameActionDamage(new List<TargetAndAmount>(requestToHandle.targetsToDamage));

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleShield(RequestShield requestToHandle)
    {
        ResponseShield response = new ResponseShield(requestToHandle.targetsToShield);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionShield gameAction = new GameActionShield(new List<TargetAndAmount>(requestToHandle.targetsToShield));

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleSwitchActiveChamp(RequestSwitchActiveChamps requestToHandle)
    {
        ResponseSwitchActiveChamp response = new ResponseSwitchActiveChamp(requestToHandle.targetToSwitch);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionSwitchActiveChamp gameAction = new GameActionSwitchActiveChamp(requestToHandle.targetToSwitch);
        gameAction.championDied = requestToHandle.championDied;
        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleDestroyLandmark(RequestDestroyLandmark requestToHandle)
    {
        ResponseDestroyLandmark response = new ResponseDestroyLandmark(new List<TargetInfo>(requestToHandle.landmarksToDestroy));
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionDestroyLandmark gameAction = new GameActionDestroyLandmark(requestToHandle.landmarksToDestroy);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleRemoveCardsGraveyard(RequestRemoveCardsGraveyard requestToHandle)
    {
        ResponseRemoveCardsGraveyard response = new ResponseRemoveCardsGraveyard(new List<TargetInfo>(requestToHandle.cardsToRemoveGraveyard));
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionRemoveCardsGraveyard gameAction = new GameActionRemoveCardsGraveyard(requestToHandle.cardsToRemoveGraveyard);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandlePlayCard(RequestPlayCard requestToHandle)
    {
        ResponsePlayCard response = new ResponsePlayCard(requestToHandle.cardAndPlacement, requestToHandle.manaCost);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;
        response.manaCost = requestToHandle.manaCost;

        GameActionPlayCard gameAction = new GameActionPlayCard(requestToHandle.cardAndPlacement, requestToHandle.manaCost);


        if(connectedToSQLServer)
        {
            GameActionToLog toLog = new GameActionToLog(requestToHandle.whichPlayer, requestToHandle.cardAndPlacement.CardName);
            toLog.gameId = requestToHandle.gameId;
             SQLaddGameAction(toLog);


        }

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleAddSpecificCardToHand(RequestAddSpecificCardToHand requestToHandle)
    {
        ResponseAddSpecificCardToHand response = new ResponseAddSpecificCardToHand(requestToHandle.cardToAdd);
        response.gameId = requestToHandle.gameId;
        response.whichPlayer = requestToHandle.whichPlayer;

        GameActionAddSpecificCardToHand gameAction = new GameActionAddSpecificCardToHand(requestToHandle.cardToAdd);

        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }

    private ServerResponse HandleRequestActions(ClientRequest requestToHandle)
    {
        ServerResponse response = new ServerResponse();
        response.gameId = requestToHandle.gameId;



        int player = requestToHandle.whichPlayer == 0 ? 1 : 0;
        lock (onGoingGames)
        {
            if (player == 1)
            {
                if (onGoingGames.ContainsKey(requestToHandle.gameId))
                {
                    response.OpponentActions = new List<GameAction>(onGoingGames[requestToHandle.gameId].player2Actions);
                    onGoingGames[requestToHandle.gameId].player2Actions.Clear();
                }

            }
            else
            {
                if (onGoingGames.ContainsKey(requestToHandle.gameId))
                {
                    response.OpponentActions = new List<GameAction>(onGoingGames[requestToHandle.gameId].player1Actions);
                    onGoingGames[requestToHandle.gameId].player1Actions.Clear();
                }
            }
        }

        response.message = "" + "ongoinggames count " + onGoingGames.Count + " " + onGoingGames.ContainsKey(requestToHandle.gameId) + "  " + onGoingGames.ContainsKey(requestToHandle.gameId);

        return response;
    }
    private ServerResponse HandleRequestLobbies(ClientRequest requestToHandle)
    {
        ResponseAvailableLobbies response = new ResponseAvailableLobbies();
        response.Lobbies = hostedLobbys.Values.ToList<HostedLobby>();

        response.SQLIsConnected = connectedToSQLServer;

        return response;
    }
    private ServerResponse HandleRequestStopSwapping(RequestStopSwapping requestToHandle)
    {
        ResponseStopSwapping response = new ResponseStopSwapping();

        response.whichPlayer = requestToHandle.whichPlayer;
        response.canSwap = requestToHandle.canSwap;

        GameActionStopSwapping gameAction = new GameActionStopSwapping(requestToHandle.canSwap);



        AddGameAction(response, gameAction, requestToHandle.gameId);
        return response;
    }
    private ServerResponse HandleLogAction(RequestLogGameAction requestToHandle)
    {
        ServerResponse response = new ServerResponse();

        response.whichPlayer = requestToHandle.whichPlayer;
      
        if(connectedToSQLServer)
        {
            GameActionToLog toLog = new GameActionToLog(requestToHandle.whichPlayer,requestToHandle.gameActionName);
            toLog.gameId = requestToHandle.gameId;

            SQLaddGameAction(toLog);
        }

        return response;
    }


    private void AddGameAction(ServerResponse response, GameAction gameAction, int gameId)
    {

        lock (onGoingGames)
        {

            if (response.whichPlayer == 1)
            {
                onGoingGames[response.gameId].AddGameActionPlayer2(gameAction);
            }
            else
            {
                onGoingGames[response.gameId].AddGameActionPlayer1(gameAction);
            }
        }

    }

    ~Server()
    {
        m_Stopping = true;
    }


    public class GameActionToLog
    {
        public int playerIndex;
        public int gameId;
        public string gameAction;
        public int turnPerformed;
        public int gameActionId;

        public GameActionToLog(int playerIndex, string gameAction)
        {
            this.playerIndex = playerIndex;
            this.gameAction = gameAction;
        }
        public GameActionToLog(int playerIndex, string gameAction, int gameId)
        {
            this.playerIndex = playerIndex;
            this.gameAction = gameAction;
            this.gameId = gameId;
        }
    }
     
    public class OngoingGame
    {

        public List<GameAction> player1Actions = new List<GameAction>();
        public List<GameAction> player2Actions = new List<GameAction>();

        public void AddGameActionPlayer1(GameAction gameAction)
        {
            player1Actions.Add(gameAction);
        }
        public void AddGameActionPlayer2(GameAction gameAction)
        {
            player2Actions.Add(gameAction);
        }
    }

    public class HostedLobby
    {
        public int lobbyId = 0;
        public bool anotherPlayerJoind = false;

        public int uniqueInteger = 0;


        public string lobbyName = "hej";

        public int gameId = 0;

    }
    public class Integer
    {
        public int value = 0;

        public Integer(int value)
        {
            this.value = value;
        }

    }
}
