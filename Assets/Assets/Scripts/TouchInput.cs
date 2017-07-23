using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{

	public LayerMask objectPartTouchInputMask;

	private float doubleTapRecoil = 0.25f;
	private float doubleTapCooldown;
	private int tapCount = 0;

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
					GameObject recipient = hit.transform.gameObject;

					if(touch.phase == TouchPhase.Ended)
					{
						if(recipient.GetComponent<AssembleObject.ObjectPart>())
						{
							if(doubleTapCooldown > 0 && tapCount >= 1)
							{
								if(recipient.GetComponent<AssembleObject.ObjectPart>().selected)
								{
									recipient.SendMessage("Disselect");
								}
								else
								{
									recipient.SendMessage("Select");
								}
							}
							else
							{
								recipient.SendMessage("PlaySound");
								doubleTapCooldown = doubleTapRecoil;
								tapCount++;
							}
						}
					}
				}
			}
		}

		if(doubleTapCooldown > 0)
		{
			doubleTapCooldown -= Time.deltaTime;
		}
		else
		{
			tapCount = 0;
		}
	}
}
