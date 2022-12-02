using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    [SerializeField] private GameObject Builder;
    [SerializeField] private EffectController VFXController;


    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //EffectController.Instance.DestoryShield(championToProtect);
            //slash.SetActive(true);
            //builderAnim.Play("Attack");
            //StartCoroutine(SlashAttack());
            VFXController.
                DestoryGraveRoEffect(Builder);
        }

 
    }

    //IEnumerator SlashAttack()
    //{
    //    yield return new WaitForSeconds(slashDelay);
    //    slash.SetActive(true);
    //    yield return new WaitForSeconds(1);
    //    DIsableSlash();


    //}

    //void DIsableSlash()
    //{
    //    slash.SetActive(false);
    //}
    
}
