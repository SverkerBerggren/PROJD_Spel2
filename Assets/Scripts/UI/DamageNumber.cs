using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using DG.Tweening;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    public TMP_Text text;
    private void OnEnable()
    {
        transform.localPosition = startPos;
        text.alpha = 1.0f;
        float y = transform.localPosition.y;
        transform.DOLocalMoveY(y + 20, 1f).OnComplete( () =>
        {
            text.DOFade(0f, 1.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }); 
        });        
    }
}
