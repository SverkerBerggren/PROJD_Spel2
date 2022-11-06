using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private GameObject testChamp;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            gameObject.GetComponent<EffectController>().ActiveShield(testChamp, 10);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<EffectController>().DestoryShield(testChamp);
        }
    }
}
