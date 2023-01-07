using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMessage : MonoBehaviour
{
    private int msgIndex = 0;

    public string[] MessageArray = {};

    public void LoadNextMessage(){
        if(msgIndex < MessageArray.Length) {
            string message = MessageArray[msgIndex];
            msgIndex++;

            Tutorial.Instance.ShowTutorial("How To Play", message);

        };
    }

    public void LoadPreviousMessage(){
        if(msgIndex < MessageArray.Length) {
            string message = MessageArray[msgIndex];
            msgIndex--;

            Tutorial.Instance.ShowTutorial("How To Play", message);

        };
    }
}
