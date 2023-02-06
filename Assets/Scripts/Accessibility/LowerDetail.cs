using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerDetail : MonoBehaviour
{
    private Camera _camera;
    private float intensity = 1.0f;
    private List<Color> baseColorShader = new List<Color>();
    private List<Color> baseColors = new List<Color>();
    private ClearlyOpponentTurn _clearlyOpponentTurn;

    [Header("Background")]
    [SerializeField] private List<GameObject> makeDarker = new List<GameObject>();
    [SerializeField] private Color _Color;

    [Header("Effect")]
    [SerializeField] private List<Material> shaders = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _clearlyOpponentTurn = gameObject.GetComponentInParent<ClearlyOpponentTurn>();
    }

    public void SetUpLowerDetailBkg()
    {
        if (_clearlyOpponentTurn.On)
        {
            _clearlyOpponentTurn.ResetOpponent();       
        }
        _clearlyOpponentTurn.setBool(true);

        //change ground_low base color to black
        _camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Background"));
        foreach(GameObject go in makeDarker)
        {
            MeshRenderer mr= go.GetComponent<MeshRenderer>();
            baseColors.Add(mr.material.color);
            mr.material.color = _Color;
        }
    }

    public void DefaultDetailBkg()
    {
        _clearlyOpponentTurn.setBool(false);
        if (_clearlyOpponentTurn.On)
        {
            _clearlyOpponentTurn.MakeOppoentClearer();
        }
      

        _camera.cullingMask |= 1 << LayerMask.NameToLayer("Background");
        for (int i = 0; i < makeDarker.Count; i++)
        {
            GameObject go = makeDarker[i];
            MeshRenderer mr = go.GetComponent<MeshRenderer>();
            mr.material.color = baseColors[i];
            
        }
        baseColors.Clear();
    }
   
    public void LowerDetailShader()
    {
        float factor = Mathf.Pow(0.2f, intensity);
        foreach (Material m in shaders)
        {  
            Color color = new Color(m.color.r * factor, m.color.g * factor, m.color.b * factor);
            baseColorShader.Add(m.color);
            m.color = color;
        }
    }

    public void DefaultDetailShader()
    {
        for (int i = 0; i < baseColorShader.Count; i++)
        {
            shaders[i].color = baseColorShader[i];
        }
        baseColorShader.Clear();
    }


    public void OnApplicationQuit()
    {
        DefaultDetailShader();
    }
}
