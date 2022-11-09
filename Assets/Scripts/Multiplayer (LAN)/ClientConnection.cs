using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using Unity.VisualScripting;

public class ClientConnection : MonoBehaviour
{
    private static ClientConnection instance;
    Semaphore requestCount = new Semaphore(0, 100);

    bool notStopping = true;

    public Queue<Tuple<ClientRequest, Action<ServerResponse>>> queuedRequests = new Queue<Tuple<ClientRequest, Action<ServerResponse>>>();
    public Queue<Tuple<ServerResponse, Action<ServerResponse>>> recievedRespones = new Queue<Tuple<ServerResponse, Action<ServerResponse>>>();

    public int playerId = -1;
    private System.Net.Sockets.TcpClient m_TCPClient = new System.Net.Sockets.TcpClient();

    public bool isHost = false;

    public static ClientConnection Instance { get; set; }

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
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Thread messageThread = new Thread(this.messageLoop);
        messageThread.Start();
    }

    public void ConnectToServer(string Adress, int port)
    {
        m_TCPClient.Connect("193.10.9.84", port);
        print("lyckas den connecta");
    }
    private void messageLoop()
    {

        while (notStopping)
        {
            
            requestCount.WaitOne();

            Tuple<ClientRequest, Action<ServerResponse>> tuple = null;
            lock (queuedRequests)
            {
                tuple = queuedRequests.Dequeue();
            }
            ServerResponse response = sendRequest(tuple.Item1);
            lock (recievedRespones)
            {
                recievedRespones.Enqueue(new Tuple<ServerResponse, Action<ServerResponse>>(response, tuple.Item2));
            }



        }
    }

    // Update is called once per frame
    void Update()
    {
        lock (recievedRespones)
        {
            if (recievedRespones.Count > 0)
            {
                Tuple<ServerResponse, Action<ServerResponse>> response = recievedRespones.Dequeue();
                response.Item2(response.Item1);
            }
        }
    }

    public void AddRequest(ClientRequest request, Action<ServerResponse> action)
    {
        lock (queuedRequests)
        {
            queuedRequests.Enqueue(new Tuple<ClientRequest, Action<ServerResponse>>(request, action));

            requestCount.Release();
        }

        
    }

    public ServerResponse sendRequest(ClientRequest data)
    {
        MBJson.JSONObject ObjectToSend = MBJson.JSONObject.SerializeObject(data);
        byte[] DataToSend = Server.SerializeJsonObject(ObjectToSend);
        m_TCPClient.GetStream().Write(DataToSend, 0, DataToSend.Length);

        MBJson.JSONObject Response = Server.ParseJsonObject(m_TCPClient.GetStream());
        ServerResponse ReturnValue = MBJson.JSONObject.DeserializeObject<ServerResponse>(Response);
        return (ReturnValue);
    }
}
