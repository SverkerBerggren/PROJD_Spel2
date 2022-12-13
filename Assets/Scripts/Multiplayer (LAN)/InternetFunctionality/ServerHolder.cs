using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHolder : MonoBehaviour
{
    // Start is called before the first frame update
    ClientConnection clientConnection;
    Server server = new Server();
    void Start()
    {
        clientConnection = FindObjectOfType<ClientConnection>();
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void StartServer()
    {
        server.StartServer(63000);

        clientConnection.playerId = 0;

        print("Server har startat");
    }
}
