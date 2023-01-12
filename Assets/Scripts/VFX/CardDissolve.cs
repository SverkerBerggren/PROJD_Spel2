using System.Threading.Tasks;
using UnityEngine;
public class CardDissolve : MonoBehaviour
{
    private MeshRenderer meshMaterial;
    private ActionOfPlayer actionOfPlayer;
    private CardDisplay display;
    private float dissolveRate = 0.0225f;
    private int refreshRate = 30;

    [SerializeField] private GameObject cardDissolve_VFX;
    [SerializeField] private GameObject glow;
    [SerializeField] private GameObject textCanvas;
    [SerializeField] private string alpha = "_AlphaClipThreshold";

    // Start is called before the first frame update
    void Start()
    {
       actionOfPlayer = ActionOfPlayer.Instance;
       display = GetComponentInParent<CardDisplay>();
       meshMaterial = GetComponent<MeshRenderer>();

    }


    // Update is called once per frame

    public async Task DissolveCard()
    {
		if (display.OpponentCard) return;

		glow.GetComponent<MeshRenderer>().enabled = false;
        textCanvas.SetActive(false);

        float counter = meshMaterial.material.GetFloat(alpha);

        cardDissolve_VFX.SetActive(true);
        while (meshMaterial.material.GetFloat(alpha) < 1f)
        {
            // Debug.Log(counter);
            counter += dissolveRate;
            meshMaterial.material.SetFloat(alpha, counter);
            if (meshMaterial.material.GetFloat(alpha) >= 0.99f)
            {
                Debug.Log("Down");
                cardDissolve_VFX.SetActive(false);
                meshMaterial.material.SetFloat(alpha, 0);
                glow.GetComponent<MeshRenderer>().enabled = true;
                textCanvas.SetActive(true);               
                break;

            }
            
            await Task.Delay(refreshRate);            
        }
        //actionOfPlayer.ChangeCardOrder(true, display);
    }

/*    private IEnumerator DissolveCoro()
    {
        //alphaclip starts with 0. 0 means no clip and 1 is all clip
        //make a smooth transition from 0 to 1
       

        glow.GetComponent<MeshRenderer>().enabled = false;
        textCanvas.SetActive(false);

        float counter = meshMaterial.material.GetFloat(alpha);

        cardDissolve_VFX.SetActive(true);
        while ( meshMaterial.material.GetFloat(alpha) < 1f)
        {
           // Debug.Log(counter);
            counter += dissolveRate;
            meshMaterial.material.SetFloat(alpha, counter);
            if (meshMaterial.material.GetFloat(alpha) >= 0.99f)
            {
                Debug.Log("Down");
                cardDissolve_VFX.SetActive(false);
                meshMaterial.material.SetFloat(alpha, 0);
                glow.GetComponent<MeshRenderer>().enabled = true;
                textCanvas.SetActive(true);
                break;

            }
            yield return new WaitForSeconds(refreshRate);
        }
    }*/
       

   
    private void PlayDissVFX()
    {

        cardDissolve_VFX.SetActive(true);
    }



 
}
