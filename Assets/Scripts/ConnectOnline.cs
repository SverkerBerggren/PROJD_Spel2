using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ConnectOnline : MonoBehaviour
{
    public EnalbeUIButton button;
    public bool enableNewRoom = false;
    public void OnClick()
    {
        Thread connectToServer = new Thread(ConnectToServer);

        connectToServer.Start();

      //  button.ClickEnableObjects();

    }

    private void Update()
    {
        if(enableNewRoom)
        {
            button.ClickEnableObjects();
        }
    }

    public void ConnectToServer()
    {
        
        Debug.Log("hej");
        ClientConnection.Instance.ConnectToServer("mrboboget.se", 63000);
        enableNewRoom = true;
        Debug.Log("hej igen ");

        RequestUniqueInteger request = new RequestUniqueInteger();
        ClientConnection.Instance.AddRequest(request, UniqueIntegerCallback);

    }

    public void UniqueIntegerCallback(ServerResponse response)
    {
        ResponseUniqueInteger castedResponse = (ResponseUniqueInteger)response;

        ClientConnection.Instance.uniqueInteger = castedResponse.UniqueInteger;
    }
}
