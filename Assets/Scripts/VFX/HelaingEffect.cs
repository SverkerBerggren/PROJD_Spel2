using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelaingEffect : MonoBehaviour
{
    [SerializeField] private GameObject controllObject;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (!controllObject.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject,1f);
        }
    }
}
