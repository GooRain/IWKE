using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	public static UIManager ins;

	public void Awake()
	{
		if(!ins)
		{
			ins = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	[SerializeField]
	private UIPanel partPanel;
	[SerializeField]
	private Text partPanelText;

	public Vector3 partPositionOnPanel;
	public float movementTime;

	public void ShowPartPanel(string name)
	{
		partPanel.Show();
		partPanelText.text = name.ToUpper();
	}

	public void HidePartPanel()
	{
		partPanel.Hide();
	}

}
