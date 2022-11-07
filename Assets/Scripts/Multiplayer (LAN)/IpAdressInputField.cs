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
        if(Input.GetKeyDown(KeyCode.J))
        {
            Thread messageThread = new Thread(this.ConnectToServer);
            messageThread.Start();


        }
    }

    public void ConnectToServer()
    {
        clientConnection.ConnectToServer("193.10.9.92", 63000);



        ClientRequest request = new ClientRequest();
        if (clientConnection.isHost)
        {
            request.whichPlayer = 0;
            clientConnection.playerId = 0;
        }
        else
        {
            request.whichPlayer = 1;
            clientConnection.playerId = 1;
        }
        request.createScene = true;


        testInternet.hasJoinedLobby = true;


        clientConnection.AddRequest(request, CreateScene);

		if (!clientConnection.isHost)
		{
			ClientRequestGameSetup gameSetup = new ClientRequestGameSetup();
            gameSetup.whichPlayer = 1;
			gameSetup.reciprocate = true;
            gameSetup.Type = 15;

            print("Not In method");
			ClientConnection.Instance.AddRequest(gameSetup, EmptyMethod);
		}
	}

    public void EmptyMethod(ServerResponse response)
    {
        print("In method");
    }

}
