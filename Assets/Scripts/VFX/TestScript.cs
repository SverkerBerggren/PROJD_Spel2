using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject championToProtect;
    [SerializeField] private Animator builderAnim;
    [SerializeField] private GameObject slash;
    [SerializeField] private float slashDelay;

    // Start is called before the first frame update
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(Screen.width / 2 - 50, 5, 100, 30), "Test"))
    //    {
    //        EffectController.Instance.ActiveShield(championToProtect, 10);
    //    }
    //}
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //EffectController.Instance.DestoryShield(championToProtect);
            //slash.SetActive(true);
            builderAnim.SetTrigger("Attack");
            StartCoroutine(SlashAttack());
            
        }
    }

    IEnumerator SlashAttack()
    {
        yield return new WaitForSeconds(slashDelay);
        slash.SetActive(true);
        yield return new WaitForSeconds(1);
        DIsableSlash();


    }

    void DIsableSlash()
    {
        slash.SetActive(false);
    }
    
}
