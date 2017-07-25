using UnityEngine;
using AssembleObject;

public class TouchInput : MonoBehaviour
{

	[SerializeField] private LayerMask objectPartTouchInputMask;

	private float doubleTapRecoil = 0.25f;
	private float doubleTapCooldown;
	private int tapCount = 0;
	private ObjectPart part;
	private bool isPartRotating = false;

	private void Update()
	{
		HandleTouch();
	}

	private void HandleTouch()
	{
		if(Input.touchCount > 0)
		{
			foreach(Touch touch in Input.touches)
			{
				RaycastHit hit;
				if(touch.phase == TouchPhase.Began && IsHitObjectPart(touch, out hit))
				{
					part = hit.transform.gameObject.GetComponent<ObjectPart>();
					isPartRotating = true;
				}

				if(isPartRotating)
				{
					if(touch.phase == TouchPhase.Moved)
					{
						float rotX = touch.deltaPosition.x * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;
						float rotY = touch.deltaPosition.y * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;

						part.RotateAround(rotX, rotY);
					}

					if(touch.phase == TouchPhase.Ended && IsHitObjectPart(touch))
					{
						if(doubleTapCooldown > 0)
						{
							tapCount++;
							if(tapCount >= 2)
							{
								if(part.Selected)
								{
									part.SendMessage("UnSelect");
								}
								else
								{
									part.SendMessage("Select");
								}
								//doubleTapCooldown = doubleTapRecoil;
							}
						}
						else
						{
							tapCount++;
							doubleTapCooldown = doubleTapRecoil;
						}
						isPartRotating = false;
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
			if(part != null && tapCount == 1)
			{
				part.SendMessage("PlaySound");
			}
			tapCount = 0;
		}
	}

	private bool IsHitObjectPart(Touch touch, out RaycastHit _hit)
	{
		Ray ray = Camera.main.ScreenPointToRay(touch.position);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100f, objectPartTouchInputMask))
		{
			_hit = hit;
			return true;
		}
		_hit = new RaycastHit();
		return false;
	}

	private bool IsHitObjectPart(Touch touch)
	{
		Ray ray = Camera.main.ScreenPointToRay(touch.position);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100f, objectPartTouchInputMask))
		{
			return true;
		}
		return false;
	}

}
