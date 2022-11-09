using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialLayout : LayoutGroup
{
    public float fDistance;
    [Range(0f, 360f)]
    public float MinAngle, MaxAngle, StartAngle;
    protected override void OnEnable() 
    { 
        base.OnEnable();    
        CalculateRadial(); 
    }
    public override void SetLayoutHorizontal()
    {
    }
    public override void SetLayoutVertical()
    {
    }
    public override void CalculateLayoutInputVertical()
    {
        CalculateRadial();
    }
/*    public override void CalculateLayoutInputHorizontal()
    {
        CalculateRadial();
    }*/
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        CalculateRadial();
    }
#endif
    void CalculateRadial()
    {
        StartAngle = 105;
        MinAngle = 50;
        int activeChildCount = 5;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child && child.gameObject.activeSelf)
            {
                if (activeChildCount >= 5)
                {
                    if (i == activeChildCount)
                    {
                        StartAngle += 2;
                        MinAngle += 3;
                        activeChildCount++;
                    }
                }
            }
        }

        m_Tracker.Clear();
        if (activeChildCount == 0)
            return;
        float fOffsetAngle = ((MaxAngle - MinAngle)) / (activeChildCount - 1);
        float fAngle = StartAngle;


        for (int i = 0; i < transform.childCount; i++)
        {
            float zValue = -2;
            float zValueMult = i * 0.25f;
            RectTransform child = (RectTransform)transform.GetChild(i);
            if (child != null && child.gameObject.activeSelf)
            {
                Vector2 vPos = new(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad));
                child.localPosition = vPos * fDistance;
                child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                fAngle += fOffsetAngle;
            }
            child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, zValue + zValueMult);
        }
    }
}