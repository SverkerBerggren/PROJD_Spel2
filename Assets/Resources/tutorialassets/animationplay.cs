using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationplay : MonoBehaviour
{
    public void PlayAnimationT(){
        GetComponent<Animator>().Play("tutorialcontroller)");
    }
}
