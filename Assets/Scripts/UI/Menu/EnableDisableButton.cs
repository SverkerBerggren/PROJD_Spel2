using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableButton : MonoBehaviour
{
    
    public List<GameObject> gameObjectsToDisable = new List<GameObject>();
    public List<GameObject> gameObjectsToEnable = new List<GameObject>();

    [SerializeField] private ReturnButton returnButton;


    public void Start()
    {
        returnButton = FindObjectOfType<ReturnButton>();
    }

    public void OnClick()
    {
        foreach(GameObject obj in gameObjectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in gameObjectsToEnable)
        {
            obj.SetActive(true);
        }

        returnButton.AddObjectToReturnTo(gameObjectsToDisable[0], gameObjectsToEnable[0]);
    }
}
