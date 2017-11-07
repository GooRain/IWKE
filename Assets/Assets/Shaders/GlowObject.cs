using UnityEngine;

public class GlowObject : MonoBehaviour
{

	public bool isTransparent = false;

	//private List<Material> materials = new List<Material>();
	private Renderer myRenderer;

	private void Start()
	{
		myRenderer = GetComponent<Renderer>();
		if(!isTransparent)
		{
			foreach(Material mat in myRenderer.materials)
			{
				Color previousColor = mat.GetColor("_Color");
				mat.shader = Shader.Find("Outlined/Silhouetted Bumped Mobile");
				mat.SetColor("_Color", previousColor);
				mat.SetColor("_OutlineColor", UIManager.ins.OutlineColor);
				//mat.SetColor("_EmissionColor", UIManager.ins.GlowColor);
			}
			GlowOff();
		}
	}

	public void GlowOn()
	{
		if(!isTransparent)
		{
			foreach(Material mat in myRenderer.materials)
			{
				mat.SetFloat("_Outline", UIManager.ins.OutlineWidth / transform.localScale.x);
				mat.SetColor("_OutlineColor", UIManager.ins.OutlineColor);
			}
			Debug.Log("Outline on");
		}
	}

	public void GlowOff()
	{
		if(!isTransparent)
		{
			foreach(Material mat in myRenderer.materials)
			{
				mat.SetFloat("_Outline", 0f);
				mat.SetColor("_OutlineColor", Color.clear);
			}
			Debug.Log("Outline off");
		}
	}

}
