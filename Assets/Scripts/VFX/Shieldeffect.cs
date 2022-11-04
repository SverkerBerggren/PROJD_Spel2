using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldeffect : MonoBehaviour
{
    [SerializeField] private int shaderValue = 100;
    [SerializeField] private Renderer armorMaterial;
    [SerializeField] private int currentShaderValue;

    private MaterialPropertyBlock m_PropetyBlock;
    private bool isDamaging;
    private bool timeToGo;
    private float targetDiss;//target dissolve value
    private float currentDiss; //current dissolve value

    private void Start()
    {
        m_PropetyBlock = new MaterialPropertyBlock();
        currentShaderValue = 0;
        currentDiss = targetDiss = 1f;
    }
    private void Update()
    {

        if (isDamaging)
        {
            //the propety health starts with 1, when it is 0, armor disappears (0min,1max,1 default)
            DamageArmor(10);
            //from 1-0,9
            currentDiss = Mathf.Lerp(currentDiss, targetDiss, Time.deltaTime); // give a fade out effect
            m_PropetyBlock.SetFloat("_HealthC", currentDiss);
            armorMaterial.SetPropertyBlock(m_PropetyBlock);
            if (currentShaderValue == 100 && m_PropetyBlock.GetFloat("_HealthC") <= 0.1f)
            {
                m_PropetyBlock.SetFloat("_HealthC", 0f);
                armorMaterial.SetPropertyBlock(m_PropetyBlock);
                isDamaging = false;
                timeToGo = true;

            }
        }
        if (timeToGo && !isDamaging)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update


    public void DamageArmor(int dmg)
    {
        isDamaging = true;
        SetArmor(Mathf.Min(currentShaderValue + dmg, 100));
    }
    public void SetArmor(int value)
    {
        currentShaderValue = value;
        targetDiss = (float)(shaderValue - currentShaderValue) / shaderValue;
    }

    public void Disslove()
    {
        isDamaging = true;
        m_PropetyBlock.SetFloat("_T_ScrollSpeed", 0f);
    }
}




