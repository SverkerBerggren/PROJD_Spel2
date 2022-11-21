using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    public TargetInfo targetInfo;
    public void OnClick()
    {
        Choice.Instance.AddTargetInfo(targetInfo);
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
