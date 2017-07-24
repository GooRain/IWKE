using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{

	[SerializeField] private LayerMask objectPartTouchInputMask;

	private float doubleTapRecoil = 0.25f;
	private float doubleTapCooldown;
	private int tapCount = 0;
	private GameObject recipient;

	private void Update()
	{
		if(Input.touchCount > 0)
		{
			foreach(Touch touch in Input.touches)
			{
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, objectPartTouchInputMask))
				{
					recipient = hit.transform.gameObject;

					if(touch.phase == TouchPhase.Ended)
					{
						if(recipient.GetComponent<AssembleObject.ObjectPart>())
						{
							if(doubleTapCooldown > 0)
							{
								tapCount++;
								if(tapCount >= 2)
								{
									if(recipient.GetComponent<AssembleObject.ObjectPart>().Selected)
									{
										recipient.SendMessage("UnSelect");
									}
									else
									{
										recipient.SendMessage("Select");
									}
									//doubleTapCooldown = doubleTapRecoil;
								}
							}
							else
							{
								tapCount++;
								doubleTapCooldown = doubleTapRecoil;
							}
						}
					}
				}
			}
		}

		if(doubleTapCooldown >= 0)
		{
			doubleTapCooldown -= Time.deltaTime;
		}
		else
		{
			if(recipient != null && recipient.GetComponent<AssembleObject.ObjectPart>() && tapCount == 1)
			{
				recipient.SendMessage("PlaySound");
			}
			tapCount = 0;
		}
	}
}
