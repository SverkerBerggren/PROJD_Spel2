using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HealthBarShake : MonoBehaviour
{
    public void StartShake()
    {
        transform.DOShakePosition(0.7f, 80, 30);
    }
}
