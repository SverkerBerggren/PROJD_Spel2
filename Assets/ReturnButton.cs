using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [SerializeField] private Texture returnImage; 
    [SerializeField] private Texture xImage;
    [SerializeField] private GameObject pausMenu;
    private RawImage rawImage;
    private Stack<GameObject> objectsToReturnTo = new Stack<GameObject>();
    [SerializeField] private GameObject currentActiveObject;
    

    public void Start()
    {
        rawImage = GetComponent<RawImage>();     
        
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnAStep();
        }
    }

    public void ChangeImageToReturn(bool changeImageToReturn)
    {
        if(changeImageToReturn)
        {
            rawImage.texture = returnImage;
        }
        else
        {
            rawImage.texture = xImage;
        }
    }
    
    public void AddObjectToReturnTo(GameObject objectToReturnTo, GameObject objectToDisable)
    {
        objectsToReturnTo.Push(objectToReturnTo);
        currentActiveObject = objectToDisable;
        ChangeImageToReturn(true);
    }

    public void ReturnAStep()
    {   
        if(objectsToReturnTo.Count < 1)
        {
            pausMenu.SetActive(false);
            return;
        }
        currentActiveObject.SetActive(false);
        currentActiveObject = objectsToReturnTo.Pop();
        currentActiveObject.SetActive(true);
        
        if(objectsToReturnTo.Count < 1)
        {
            ChangeImageToReturn(false);
        }
        
    }
    public void OnClick()
    {
        ReturnAStep();
    }
}
