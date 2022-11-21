using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnalbeUIButton : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> gameObjectsToEnable = new List<GameObject>();
    public List<GameObject> gameObjectsToDisable = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickEnableObjects()
    {
        foreach( GameObject obj in gameObjectsToEnable)
        {
            obj.SetActive(true);
        }     
        foreach( GameObject obj in gameObjectsToDisable)
        {
            obj.SetActive(false);
        }
    }

}
