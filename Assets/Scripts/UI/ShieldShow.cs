using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldShow : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text shieldText; 
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (gameObject.name.Equals("Shield"))
            animator.SetBool("ShieldOn", true);
        else
        {
            animator.SetBool("ShieldOnInstantly", true);
            animator.SetBool("ShieldOn", true);
        }
    }

    private void OnDisable()
    {
        animator.SetBool("ShieldOn", false);
    }
    public void ChangeShieldTextTo(int amountOfShield)
    {
        shieldText.text = amountOfShield.ToString();
    }
}
