using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	public static UIManager ins;

	public void Awake()
	{
		if(ins == null)
		{
			ins = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}


	[SerializeField] private UIPanel partPanel;
	[SerializeField] private Text partPanelText;
	
	[Header("Glow Settings")]
	[SerializeField] private Color outlineColor;
	[SerializeField] private float outlineWidth;
	[Header("Objects Settings")]
	[SerializeField] private float dualTouchLength;
	[SerializeField] private float distanceToAssembledPosition;
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
	public Color OutlineColor
	{
		get
		{
			return outlineColor;
		}

		set
		{
			outlineColor = value;
		}
	}
	public float OutlineWidth
	{
		get
		{
			return outlineWidth;
		}

		set
		{
			outlineWidth = value;
		}
	}
	public float DualTouchLength
	{
		get
		{
			return dualTouchLength;
		}

		set
		{
			dualTouchLength = value;
		}
	}
	public float DistanceToAssembledPosition
	{
		get
		{
			return distanceToAssembledPosition;
		}

		set
		{
			distanceToAssembledPosition = value;
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

	public void ToggleMusic()
	{
		AudioManager.ins.ToggleMusic();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

}
