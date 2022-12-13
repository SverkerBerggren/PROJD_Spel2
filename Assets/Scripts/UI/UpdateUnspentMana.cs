using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUnspentMana : MonoBehaviour
{
    [SerializeField] private TMP_Text textToUpdate;
    private string unspentManaText;
    private void OnEnable()
    {
        unspentManaText = ActionOfPlayer.Instance.unspentMana.ToString();
        textToUpdate.text = "Unspent mana: " + unspentManaText;
        InvokeRepeating(nameof(InvokeTextUpdate), 1f, 1f);
    }

    private void InvokeTextUpdate()
    {
        textToUpdate.text = "Unspent mana: " + unspentManaText;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
