using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 


public class IpAdressInputField : MonoBehaviour
{
    public TextMeshProUGUI inputField;

    ClientConnection clientConnection;

    TestInternet testInternet;
    private string ip;

 //   public  inputFieldText; 

    // Start is called before the first frame update
    void Start()
    {
      //  inputField = GetComponent<InputField>();

        clientConnection = FindObjectOfType<ClientConnection>();

        testInternet = FindObjectOfType<TestInternet>();
        
    }

    public void CreateScene(ServerResponse response)
    {
        print("Creater den sccene");
        testInternet.CreateScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ip = GetComponent<TMP_InputField>().text;
            Thread messageThread = new Thread(this.ConnectToServer);
            messageThread.Start();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ip = "127.0.0.1";
			Thread messageThread = new Thread(this.ConnectToServer);
			messageThread.Start();
		}
    }

    public void ConnectToServer()
    {
        clientConnection.ConnectToServer(ip, 63000);

        if (clientConnection.isHost)
        {  
            clientConnection.playerId = 0;
        }
        else
        {          
            clientConnection.playerId = 1;
        }


        testInternet.hasJoinedLobby = true;


        

		if (!clientConnection.isHost)
		{
			ClientRequestGameSetup gameSetup = new ClientRequestGameSetup();
            gameSetup.whichPlayer = 1;
			gameSetup.reciprocate = true;
           // gameSetup.Type = 15;
           List<string> ownChampions = new List<string>();

            foreach (string stringen in Setup.Instance.myChampions)
            {
                ownChampions.Add(stringen);
            }
            gameSetup.opponentChampions = ownChampions;

            print("Not In method");
			ClientConnection.Instance.AddRequest(gameSetup, EmptyMethod);
		}
	}


    public void EmptyMethod(ServerResponse response)
    {
        print("In method");
    }

}
