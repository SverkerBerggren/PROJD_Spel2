using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialV4 : MonoBehaviour
{
    Animator animTutorial;
    
    private void Start(){
        animTutorial = GetComponent<Animator>();

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
