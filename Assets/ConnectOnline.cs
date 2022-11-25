using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectOnline : MonoBehaviour
{
    public void OnClick()
    {
        ClientConnection.Instance.ConnectToServer("mrboboget.se", 63000);
    }
}
