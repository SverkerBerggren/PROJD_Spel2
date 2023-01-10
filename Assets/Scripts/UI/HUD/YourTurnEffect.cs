using UnityEngine;
using UnityEngine.UI;

public class YourTurnEffect : MonoBehaviour
{
    [SerializeField] private GameObject yourTurnEffect;
    [SerializeField] private Image currentPicture;

    public void ActivateEffect()
    {
        if (yourTurnEffect != null)
        {
            yourTurnEffect.SetActive(true);
            Invoke(nameof(YourTurnEffectHide), 1.5f);
        }
    }

    public void ChangePicture(AvailableChampion swappedChamp)
    {
        currentPicture.sprite = swappedChamp.Champion.ChampBackground;
    }


    private void YourTurnEffectHide()
    {
         yourTurnEffect.SetActive(false);
    }

}
