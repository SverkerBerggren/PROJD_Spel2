using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    [SerializeField] private GameObject testObj;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private EffectController VFXController;
    [SerializeField] private GameObject cardPrefab;
    


    private void Start()
    {
     
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testObj.GetComponent<Animator>().Play("Death");
            attackObj.GetComponent<Animator>().Play("Death");


            testObj.GetComponent<Effect_Champions>().StartDisolve();
            attackObj.GetComponent<Effect_Champions>().StartDisolve();
        }
            //EffectController.Instance.DestoryShield(championToProtect);
            //slash.SetActive(true);
            //builderAnim.Play("Attack");
            //StartCoroutine(SlashAttack());
            //testObj.GetComponent<Animator>().Play("AttackMagic");
            //VFXController.GainHealingEffect(testObj);

            // GetComponent<LowerDetail>().setUpLowerDetailBkg();
            //    VFXController.DiscardCardEffect(testObj);
            //}
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    GetComponent<LowerDetail>().DefaultDetailShader();
            //}


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
