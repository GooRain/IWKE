using UnityEngine;
using System.Collections.Generic;

public class GlowObject : MonoBehaviour
{

	private List<Material> materials = new List<Material>();

	private void Start()
	{
		foreach(Material mat in GetComponent<MeshRenderer>().sharedMaterials)
		{
			materials.Add(mat);
			mat.SetColor("Rim Color", UIManager.ins.GlowColor);
		}
		GlowOff();
	}

	public void GlowOn()
	{
		foreach(Material mat in materials)
		{
			mat.SetFloat("Rim Power", 10f);
		}
	}

	public void GlowOff()
	{
		foreach(Material mat in materials)
		{
			mat.SetFloat("Rim Power", 2.5f);
		}
	}

}
