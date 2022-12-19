using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChampionAttributes : MonoBehaviour
{
    public TMP_Text championName;
    public TMP_Text description;
    public TMP_Text hpText;

    public void UpdateChampionCard(Champion champion)
    {
        championName.text = champion.championName;
        description.text = champion.description;
        hpText.text = champion.health.ToString();

    }
}
