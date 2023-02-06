using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static UnityEngine.Rendering.PostProcessing.PostProcessResources;

public class ClearlyOpponentTurn : MonoBehaviour
{
	private float factor = 0.40f;
	private List<Color> baseColorShader = new List<Color>();
	[SerializeField] private List<Material> shaders = new List<Material>();

	public bool On;
	public bool onLowerDetail;

	public void MakeOppoentClearer()
	{
		if (onLowerDetail) return;
		foreach (Material m in shaders)
		{
			Color color = m.GetColor("_BaseColor");		
			baseColorShader.Add(color);
			m.color = color * factor;
		}
		On = true;
	}

	public void ResetOpponent()
	{
		print("??");
		for (int i = 0; i < baseColorShader.Count; i++)
		{
			shaders[i].color = baseColorShader[i];
		}
		On = false;
		baseColorShader.Clear();
	}

	public void OnApplicationQuit()
	{
		ResetOpponent();
	}

	public void setBool(bool bo)
	{
		onLowerDetail = bo;
	}
}
