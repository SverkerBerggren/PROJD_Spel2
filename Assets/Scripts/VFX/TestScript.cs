using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject championToProtect;
  
    // Start is called before the first frame update
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 5, 100, 30), "Test"))
        {
            EffectController.Instance.ActiveShield(championToProtect, 10);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EffectController.Instance.DestoryShield(championToProtect);
        }
    }
}
