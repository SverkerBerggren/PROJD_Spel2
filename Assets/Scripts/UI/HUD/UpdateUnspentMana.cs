using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUnspentMana : MonoBehaviour
{
    private string unspentManaText;
    [SerializeField] private TMP_Text textToUpdate;
    private void OnEnable()
    {
        unspentManaText = ActionOfPlayer.Instance.UnspentMana.ToString();
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
