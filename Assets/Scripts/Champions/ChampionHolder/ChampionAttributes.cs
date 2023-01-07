using TMPro;
using UnityEngine;

public class ChampionAttributes : MonoBehaviour
{
    public TMP_Text championName;
    public TMP_Text description;
    public TMP_Text hpText;

    public void UpdateChampionCard(Champion champion)
    {
        championName.text = champion.ChampionName;
        description.text = champion.Description;
        hpText.text = champion.Health.ToString();
    }
}
