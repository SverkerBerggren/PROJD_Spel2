using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestScript : MonoBehaviour
{

    [SerializeField] private GameObject testObj;
    [SerializeField] private GameObject hitObj;
    [SerializeField] private GameObject HealingPrefab;
    [SerializeField] private EffectController VFXController;
    [SerializeField] private GameObject cardPrefab;
    


    private void Start()
    {
     
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlayAnim());
            testObj.GetComponent<Animator>().Play("Attack");
           testObj.GetComponentInChildren<Effect_Champions>().attackVFX.Play();




        }


     }

    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(2.5f);
        hitObj.GetComponent<Animator>().Play("GetHit");
    }
    
}
