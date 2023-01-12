using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    private Choice choice;

    [SerializeField] private GameObject clickedEffect;
	[SerializeField] private GameObject mulliganEffect;
    public TargetInfo targetInfo;

	[Header("Accessability")]
    [SerializeField] private GameObject hoverEffect;

    [Header("Cards")]
    public GameObject CardPrefab;
    public GameObject ChampionPrefab;

    public void OnClick()
    {
        choice = Choice.Instance;
        if (clickedEffect.activeSelf)
        {
			choice.RemoveTargetInfo(targetInfo);
            clickedEffect.SetActive(false);
            if(choice.whichMethod == WhichMethod.Mulligan)
			    mulliganEffect.SetActive(false);

		}
        else
        {
			choice.AddTargetInfo(targetInfo);
            clickedEffect.SetActive(true);
			if (choice.whichMethod == WhichMethod.Mulligan)
				mulliganEffect.SetActive(true);
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
