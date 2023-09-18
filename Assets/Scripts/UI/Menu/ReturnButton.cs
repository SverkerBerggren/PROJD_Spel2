using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    private Image image;
    private Stack<GameObject> objectsToReturnTo = new Stack<GameObject>();
    private PauseMenu pauseMenuScript;

    [SerializeField] private Sprite returnImage; 
    [SerializeField] private Sprite xImage;
    [SerializeField] private GameObject pausMenu;
    [SerializeField] private GameObject oneSwitchSelect;
    [SerializeField] private GameObject currentActiveObject;
    

    public void Start()
    {
        image = GetComponent<Image>();
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
            image.sprite = returnImage;
        else
            image.sprite = xImage;
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
        NewOneSwitch.Instance.ResetBools();
        if (image.sprite == returnImage)
            NewOneSwitch.Instance.options = true;
        ReturnAStep();
    }
}
