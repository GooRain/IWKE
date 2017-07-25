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


	[SerializeField] private UIPanel partPanel;
	[SerializeField] private Text partPanelText;
	
	[Header("Glow Settings")]
	[SerializeField] private Color glowColor;
	[SerializeField] private float rimPower;
	[Header("Objects Settings")]
	[SerializeField] private Vector3 partPositionOnPanel;
	[SerializeField] private float movementTime;
	[SerializeField] private float rotateSpeed;

	public Vector3 PartPositionOnPanel
	{
		get
		{
			return partPositionOnPanel;
		}

		set
		{
			partPositionOnPanel = value;
		}
	}
	public float MovementTime
	{
		get
		{
			return movementTime;
		}

		set
		{
			movementTime = value;
		}
	}
	public Color GlowColor
	{
		get
		{
			return glowColor;
		}

		set
		{
			glowColor = value;
		}
	}
	public float RotateSpeed
	{
		get
		{
			return rotateSpeed;
		}

		set
		{
			rotateSpeed = value;
		}
	}
	public float RimPower
	{
		get
		{
			return rimPower;
		}

		set
		{
			rimPower = value;
		}
	}

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
