using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMessage : MonoBehaviour
{
    int msgIndex = 0;

    public string[] messageArray = {};

    public void LoadNextMessage(){
        if(msgIndex < messageArray.Length) {
            string message = messageArray[msgIndex];
            msgIndex++;

            Tutorial.instance.ShowTutorial("How To Play", message);

        };
    }

    public void LoadPreviousMessage(){
        if(msgIndex < messageArray.Length) {
            string message = messageArray[msgIndex];
            msgIndex--;

            Tutorial.instance.ShowTutorial("How To Play", message);

        };
    }
}
