using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneToLoad; 

    public void OnClick()
    {
        ClientConnection.Instance = null;
        SceneManager.LoadScene(sceneToLoad);
    }
    public void OnClickNotNull()
    {

        SceneManager.LoadScene(sceneToLoad);
    }
}
