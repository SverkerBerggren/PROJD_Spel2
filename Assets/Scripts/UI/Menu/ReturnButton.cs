using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    private RawImage rawImage;
    private Stack<GameObject> objectsToReturnTo = new Stack<GameObject>();
    private PauseMenu pauseMenuScript;

    [SerializeField] private Texture returnImage; 
    [SerializeField] private Texture xImage;
    [SerializeField] private GameObject pausMenu;
    [SerializeField] private GameObject oneSwitchSelect;
    [SerializeField] private GameObject currentActiveObject;
    

    public void Start()
    {
        rawImage = GetComponent<RawImage>();
        pauseMenuScript = FindObjectOfType<PauseMenu>();     
    }

    public void OneSwitchButtonSelect()
    {
        if (!oneSwitchSelect.activeSelf)
            oneSwitchSelect.SetActive(true);
        else
            oneSwitchSelect.SetActive(false);
    }

    public void ChangeImageToReturn(bool changeImageToReturn)
    {
        if(changeImageToReturn)
            rawImage.texture = returnImage;
        else
            rawImage.texture = xImage;
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
            pauseMenuScript.SetIsPauseMenuActive(false);
            return;
        }
        currentActiveObject.SetActive(false);
        currentActiveObject = objectsToReturnTo.Pop();
        currentActiveObject.SetActive(true);
        
        if(objectsToReturnTo.Count < 1)
            ChangeImageToReturn(false);
    }
    public void OnClick()
    {
        ReturnAStep();
    }
}
