using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{

	public LayerMask objectPartTouchInputMask;

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

					if(touch.phase == TouchPhase.Began)
					{
						if(recipient.GetComponent<AssembleObject.ObjectPart>())
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
					}
				}
			}
		}
	}
}
