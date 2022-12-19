using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialV4 : MonoBehaviour
{
    Animator animTutorial;
    public Button[] bArray;

    private void Start(){
        animTutorial = GetComponent<Animator>();

        foreach(Button b in bArray) {
            b.interactable = false;
        }
        Time.timeScale = 0f;
   
    }

    void ChangeAnimation() {
        animTutorial.SetInteger("Change", animTutorial.GetInteger("Change") + 1);
    }

    private void Update() {
        if (Input.anyKeyDown){
            ChangeAnimation();
        }
    }
}
