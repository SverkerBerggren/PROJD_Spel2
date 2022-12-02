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
        animator.SetBool("ShieldOn", true);
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
