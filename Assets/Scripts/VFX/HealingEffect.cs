using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : MonoBehaviour
{
    [SerializeField] private GameObject controllObject;
    private ParticleSystem pS;
    // Start is called before the first frame update

    private void Start()
    {
        pS = controllObject.GetComponent<ParticleSystem>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!pS.IsAlive())
        {
            Destroy(gameObject,1f);
        }
    }
}
