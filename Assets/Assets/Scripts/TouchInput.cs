using UnityEngine;
using AssembleObject;

public class TouchInput : MonoBehaviour
{

	[SerializeField] private LayerMask objectPartTouchInputMask;
	[SerializeField] private float maxTouchDeltaMove;
	[SerializeField] private float playSoundRecoil = 1f;
	[SerializeField] private float doubleTapRecoil = 0.25f;

	private float playSoundCooldown;

	private int tapCount = 0;
	private float doubleTapCooldown;

	private ObjectPart part;
	private ObjectPart selectedPart;

	private ObjectSubPart subPart;
	private ObjectSubPart selectedSubPart;

	private bool isSubPart = false;

	private bool partTouched = false;
	//private bool isPartRotating = false;
	private bool toPlaySound = false;
	private bool toAssemblePart = false;

	private Vector2 deltaTouch = Vector2.zero;
	private Vector3 offset;
	private Vector3 newPos;

	private float dualTouchOffset = 0f;

	private float distanceToAssembledPos;

	private void Start()
	{
		playSoundCooldown = playSoundRecoil;
		doubleTapCooldown = doubleTapRecoil;
	}

	private void Update()
	{
		HandleTouch();

		if(Input.GetKeyDown(KeyCode.D))
		{
			if(part != null && part.Selected)
			{
				selectedPart.SendMessage("Disassemble");
			}
			else if(subPart != null && subPart.Selected)
			{
				selectedSubPart.SendMessage("Disassemble");
			}
		}

		playSoundCooldown -= Time.deltaTime;
	}

	private void HandleTouch()
	{
		if(Input.touchCount == 1)
		{
			HandleOneTouch();
		}
		else
		if(Input.touchCount == 2 && !isSubPart)
		{
			HandleDualTouch();
		}

		if(doubleTapCooldown >= 0)
		{
			doubleTapCooldown -= Time.deltaTime;
		}
		else
		{
			if(!isSubPart && toPlaySound)
			{
				if(part != null)
				{
					part.SendMessage("PlaySound");
				}
				playSoundCooldown = playSoundRecoil;
				toPlaySound = false;

			}
			else if(isSubPart && toPlaySound)
			{
				if(subPart != null)
				{
					subPart.SendMessage("PlaySound");
				}
				playSoundCooldown = playSoundRecoil;
				toPlaySound = false;
			}
			tapCount = 0;
		}
	}

	private void HandleOneTouch()
	{
		float touchLength = 100f;

		Touch FirstTouch = Input.GetTouch(0);
		RaycastHit hit;
		if(FirstTouch.phase == TouchPhase.Began && IsHitObjectPart(FirstTouch, out hit))
		{
			deltaTouch = FirstTouch.position;
			//Debug.Log("Touch #" + touch.fingerId + " start pos: " + deltaTouch);
			if(hit.transform.GetComponent<ObjectPart>())
			{
				part = hit.transform.GetComponent<ObjectPart>();
				isSubPart = false;

				CalculateMovingPartPosition(FirstTouch, part.transform);
				partTouched = true;
				offset = part.transform.position - newPos;
			}
			else
			{
				subPart = hit.transform.GetComponent<ObjectSubPart>();
				isSubPart = true;

				CalculateMovingPartPosition(FirstTouch, subPart.transform);
				partTouched = true;
				offset = subPart.transform.position - newPos;
			}

		}

		if(partTouched)
		{

			if(FirstTouch.phase == TouchPhase.Moved)
			{
				TouchMoved(FirstTouch);
			}

			if(FirstTouch.phase == TouchPhase.Ended)
			{
				//Debug.Log("Touch #" + touch.fingerId + " last pos: " + touch.position);
				//Debug.Log("Delta Touch: " + deltaTouch);

				tapCount++;

				touchLength = Vector2.Distance(deltaTouch, FirstTouch.position);
				//Debug.Log("Touch movement length: " + touchLength);

				if(!isSubPart)
				{
					Vector2 partAssembledPos = part.AssembledPosition;
					Debug.Log("Part Assembled Pos: " + part.AssembledPosition);
					Vector2 partCurrentPos = part.transform.position;
					distanceToAssembledPos = Vector2.Distance(partAssembledPos, partCurrentPos);
					Debug.Log("Distance to assembled position:" + distanceToAssembledPos);
					if(distanceToAssembledPos <= UIManager.ins.DistanceToAssembledPosition)
					{
						Debug.Log("ASSEMBLED::Distance to assembled position:" + distanceToAssembledPos);
						toAssemblePart = true;
					}
				}

				if(tapCount == 1)
				{
					doubleTapCooldown = doubleTapRecoil;
					if(!isSubPart)
					{
						if(touchLength <= maxTouchDeltaMove && playSoundCooldown <= 0)
						{
							//Debug.Log("Play sound");
							toPlaySound = true;
						}
						else
						if(toAssemblePart && !part.Selected)
						{
							part.SinglePartAssemble();
						}
					}
					else
					{
						if(touchLength <= maxTouchDeltaMove && playSoundCooldown <= 0)
						{
							//Debug.Log("Play sound");
							toPlaySound = true;
						}
						else
						if(toAssemblePart && !subPart.Selected)
						{
							subPart.Assemble();
						}
					}
				}
				else
				if(tapCount >= 2)
				{
					if(!isSubPart)
					{
						if(touchLength <= maxTouchDeltaMove && !part.Selected && IsHitObjectPart(FirstTouch))
						{
							part.SendMessage("Select");
							selectedPart = part;
							doubleTapCooldown = 0;
							tapCount = 0;
							//part.SendMessage("PlaySound");
						}
						else
						if(touchLength <= maxTouchDeltaMove && part.Selected && IsHitObjectPart(FirstTouch))
						{
							selectedPart.SendMessage("UnSelect");
							selectedPart = null;
							doubleTapCooldown = 0;
							tapCount = 0;
						}
					}
					else
					{
						if(touchLength <= maxTouchDeltaMove && !subPart.Selected && IsHitObjectPart(FirstTouch))
						{
							subPart.SendMessage("Select");
							selectedSubPart = subPart;
							doubleTapCooldown = 0;
							tapCount = 0;
							//part.SendMessage("PlaySound");
						}
						else
						if(touchLength <= maxTouchDeltaMove && subPart.Selected && IsHitObjectPart(FirstTouch))
						{
							selectedSubPart.SendMessage("UnSelect");
							selectedSubPart = null;
							doubleTapCooldown = 0;
							tapCount = 0;
						}
					}
					toPlaySound = false;
				}

				partTouched = false;
				toAssemblePart = false;

			}

			#region Дабл тап (мб когда-нибудь понадобится)
			//else
			//{
			//	if(touch.phase == TouchPhase.Began && !IsHitObjectPart(touch))
			//	{
			//		deltaTouch = touch.position;
			//	}

			//	if(touch.phase == TouchPhase.Ended)
			//	{
			//		float touchLength = Mathf.Sqrt((deltaTouch.x - touch.position.x) * (deltaTouch.x - touch.position.x) +
			//			(deltaTouch.y - touch.position.y) * (deltaTouch.y - touch.position.y));
			//		Debug.Log("Touch movement length: " + touchLength);
			//		if(touchLength <= maxTouchDeltaMove && doubleTapCooldown > 0 && !IsHitObjectPart(touch))
			//		{	
			//			tapCount++;
			//			if(tapCount >= 2)
			//			{
			//				if(selectedPart != null)
			//				{
			//					selectedPart.SendMessage("UnSelect");
			//					selectedPart = null;
			//				}
			//			}
			//			doubleTapCooldown = doubleTapRecoil;
			//		}
			//		else
			//		{
			//			tapCount++;
			//			doubleTapCooldown = doubleTapRecoil;
			//		}
			//	}
			//}
			#endregion

		}
	}

	private void HandleDualTouch()
	{
		Touch firstTouch = Input.GetTouch(0);
		Touch secondTouch = Input.GetTouch(1);
		float touchLength = 100f;

		RaycastHit hit;
		if(secondTouch.phase == TouchPhase.Began)
		{
			if(IsHitObjectPart(secondTouch, out hit))
			{
				dualTouchOffset = Vector2.Distance(firstTouch.position, secondTouch.position);
				part = hit.transform.gameObject.GetComponent<ObjectPart>();
				partTouched = true;
			}
			else
			{
				partTouched = false;
			}
			//Debug.Log("Touch #" + touch.fingerId + " start pos: " + deltaTouch);

			//CalculateMovingPartPosition(SecondTouch, part.transform);
			//offset = part.transform.position - newPos;
		}

		if(partTouched)
		{
			if(secondTouch.phase == TouchPhase.Ended)
			{
				//Debug.Log("Touch #" + touch.fingerId + " last pos: " + touch.position);
				//Debug.Log("Delta Touch: " + deltaTouch);

				//tapCount++;

				touchLength = Vector2.Distance(firstTouch.position, secondTouch.position) - dualTouchOffset;
				Debug.Log("2nd touch movement length: " + touchLength);

				if(touchLength >= UIManager.ins.DualTouchLength)
				{
					if(part.Selected)
					{
						part.SendMessage("SubDisassemble");
						//Debug.Log("SUB-DISASSEMBLE");
					}
				}

				partTouched = false;
				//Debug.Log("2nd Touch: " + SecondTouch.position);
			}
		}
	}

	private void TouchMoved(Touch FirstTouch)
	{
		if(!isSubPart)
		{
			if(part.Selected && part.Assembled)
			{
				float rotX = FirstTouch.deltaPosition.x * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;
				float rotY = FirstTouch.deltaPosition.y * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;

				part.RotateAround(rotX, rotY);
			}
			else
			if(part.Disassembled)
			{
				//toAssemblePart = true;
				CalculateMovingPartPosition(FirstTouch, part.transform);
				part.transform.position = newPos + offset;
			}
		}
		else
		{
			if(subPart.Selected && subPart.Disassembled)
			{
				float rotX = FirstTouch.deltaPosition.x * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;
				float rotY = FirstTouch.deltaPosition.y * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;

				subPart.RotateAround(rotX, rotY);
			}
			else
			{
				//toAssemblePart = true;
				CalculateMovingPartPosition(FirstTouch, subPart.transform);
				subPart.transform.position = newPos + offset;
			}
		}
	}

	private void CalculateMovingPartPosition(Touch touch, Transform partT)
	{
		newPos = new Vector3(touch.position.x, touch.position.y, partT.position.z - Camera.main.transform.position.z);
		newPos = Camera.main.ScreenToWorldPoint(newPos);
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
