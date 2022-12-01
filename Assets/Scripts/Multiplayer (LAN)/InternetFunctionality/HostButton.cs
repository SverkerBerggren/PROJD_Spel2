using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;
using System.Threading;

public class HostButton : MonoBehaviour
{
    // public Server server = new Server(); 
    ClientConnection clientConnection;
    public TMP_Text ipAdressText;
    // Start is called before the first frame update
    void Start()
    {
        clientConnection = FindObjectOfType<ClientConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    public void HostServer()
    {

        print("Hostar den flera gangar");
        FindObjectOfType<ServerHolder>().StartServer();
        ipAdressText.gameObject.SetActive(true);
        ipAdressText.text = LocalIPAddress();

        Thread connectThread = new Thread(ConnectToServer);
        connectThread.Start();


   //     clientConnection.isHost = true;
        clientConnection.gameId = 0;
        RequestUniqueInteger request = new RequestUniqueInteger();

        clientConnection.AddRequest(request, UniqueIntegerCallback);

        
    }
    public void UniqueIntegerCallback(ServerResponse response)
    {
        ResponseUniqueInteger castedResponse = (ResponseUniqueInteger)response;

        clientConnection.uniqueInteger = castedResponse.UniqueInteger;
    }

    private void ConnectToServer()
    {
        clientConnection.ConnectToServer(LocalIPAddress(), 63000);
    }
}
