using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    public TargetInfo targetInfo;

    [SerializeField] private GameObject clickedEffect;

    [Header("Accessability")]
    [SerializeField] private GameObject hoverEffect;

    [Header("Cards")]
    public GameObject CardPrefab;
    public GameObject ChampionPrefab;

    public void OnClick()
    {
        if (clickedEffect.activeSelf)
        {
            Choice.Instance.RemoveTargetInfo(targetInfo);
            clickedEffect.SetActive(false);
        }
        else
        {
            Choice.Instance.AddTargetInfo(targetInfo);
            clickedEffect.SetActive(true);
        }
    }

    public void OneSwitchHoverChoice()
    {
        if (hoverEffect.activeSelf)
            hoverEffect.SetActive(false);
        else
            hoverEffect.SetActive(true);
    }
}
