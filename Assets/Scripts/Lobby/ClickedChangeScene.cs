using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickedChangeScene : MonoBehaviour
{
    [SerializeField] private InternetLoop internetLoop;
    [SerializeField] private TMP_Text textToChange;

    private string forest = "OutsideScene";
    private string arena = "GameScene";


    public void OnClickedChangeScene()
    {
        if (internetLoop.loadScene.Equals(forest))
        {
            textToChange.text = "Load Scene: Arena";
            internetLoop.ChangeSceneToLoad(arena);
        }
        else if (internetLoop.loadScene.Equals(arena))
        {
            textToChange.text = "Load Scene: Forest";
            internetLoop.ChangeSceneToLoad(forest);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
