using System.Collections.Generic;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectMain : MonoBehaviour
	{
		[SerializeField] private string objectName;

		private ObjectPart.States currentState = ObjectPart.States.Idle;

		//[SerializeField] private float doubleTapRecoil = 0.25f;

		//private float doubleTapCurrentRecoil;
		//private int tapCount = 0;

		public int objectIndex;

		public bool assembled = true;
		private bool touched = false;

		private List<ObjectPart> parts;
		private ObjectPart selectedPart;

		private BoxCollider myCollider;
		private Quaternion startRotation;
		private int amountOfAssembledParts = 0;
		private float dualTouchOffset = 0f;

		public string ObjectName
		{
			get
			{
				return objectName;
			}

			set
			{
				objectName = value;
			}
		}
		public ObjectPart SelectedPart
		{
			get
			{
				return selectedPart;
			}

			private set
			{
				selectedPart = value;
			}
		}

		private CategoryPanel myCategoryPanel;
		public CategoryPanel MyCategoryPanel
		{
			get
			{
				return myCategoryPanel;
			}

			set
			{
				myCategoryPanel = value;
			}
		}

		private void Awake()
		{
			if(GetComponent<BoxCollider>())
			{
				myCollider = GetComponent<BoxCollider>();
			}
		}

		private void Start()
		{
			startRotation = transform.rotation;
			LangSystem.ins.OnLangChange += OnLangChange;
			OnLangChange();
		}

		private void Update()
		{
			HandleTouch();
			if(assembled)
			{
				transform.RotateAround(transform.position, transform.up, UIManager.ins.RotateSpeed / 1.5f * Time.deltaTime);
			}
			if(Input.GetKeyDown(KeyCode.D))
			{
				Disassemble();
			}
		}

		private void OnLangChange()
		{
			objectName = LangSystem.language.objects[objectIndex].name;
			for(int i = 0; i < parts.Count; i++)
			{
				if(parts[i] != null)
				{
					parts[i].partName = LangSystem.language.objects[objectIndex].parts[i].name;
					parts[i].OnLangChange(objectIndex, i);
				}
			}
		}

		public void InitParts()
		{
			parts = new List<ObjectPart>();
			SetChildren();
			for(int i = 0; i < parts.Count; i++)
			{
				parts[i].partIndex = i;
			}
			//InitAssembledPositions();
		}

		public void InitAssembledPositions()
		{
			foreach(var c in parts)
			{
				c.InitAssembledPosition();
			}
		}

		public void Assemble()
		{
			if(IsAnyPartSubDisassembled())
			{
				selectedPart.SubAssemble();
				//selectedPart.SendMessage("UnSelect");
			}
			else
			{
				UIManager.ins.HidePartPanel();
				DisselectPart();
				foreach(var c in parts)
				{
					if(c.Disassembled)
					{
						amountOfAssembledParts++;
						c.Assemble();
					}
				}
				currentState = ObjectPart.States.Assembling;
				StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, startRotation, UIManager.ins.MovementTime));
			}
			
		}

		public void Disassemble()
		{
			currentState = ObjectPart.States.Disassembling;
			StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, startRotation, UIManager.ins.MovementTime));
			foreach(var c in parts)
			{
				c.Disassemble();
			}
			amountOfAssembledParts = 0;
			myCategoryPanel.ShowBgImage();
		}

		public void OnCoroutineEnd()
		{
			switch(currentState)
			{
				case ObjectPart.States.Assembling:
					assembled = true;
					myCollider.enabled = true;
					myCategoryPanel.HideBgImage();
					break;
				case ObjectPart.States.Disassembling:
					assembled = false;
					myCollider.enabled = false;
					myCategoryPanel.ShowBgImage();
					break;
				default:
					break;
			}
			currentState = ObjectPart.States.Idle;
		}

		public void SetChildren()
		{
			foreach(Transform c in transform)
			{
				parts.Add(c.gameObject.GetComponent<ObjectPart>());
			}
		}

		public void SelectPart(ObjectPart part)
		{
			//StopAllCoroutines();
			//StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, Quaternion.identity, 1f));
			foreach(ObjectPart obj in parts)
			{
				if(obj != part)
				{
					obj.gameObject.SetActive(false);
				}
			}
			selectedPart = part;
			selectedPart.GetSelected();
			UIManager.ins.ShowPartPanel(part.partName);
		}

		public void DisselectPart()
		{
			//StopAllCoroutines();
			//StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, startRotation, 1f));
			foreach(ObjectPart obj in parts)
			{
				obj.gameObject.SetActive(true);
			}
			UIManager.ins.HidePartPanel();
		}

		private void HandleTouch()
		{
			if(assembled)
			{
				if(Input.touchCount == 1)
				{
					Touch firstTouch = Input.GetTouch(0);

					if(firstTouch.phase == TouchPhase.Began)
					{
						Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
						RaycastHit hit;

						if(Physics.Raycast(ray, out hit))
						{
							if(hit.transform.gameObject == gameObject)
							{
								touched = true;
							}
						}
					}

					if(touched && firstTouch.phase == TouchPhase.Moved)
					{
						float rotX = firstTouch.deltaPosition.x * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;
						float rotY = firstTouch.deltaPosition.y * UIManager.ins.RotateSpeed * Mathf.Deg2Rad;

						transform.RotateAround(transform.position, Vector3.up, -rotX);
						transform.RotateAround(transform.position, Vector3.right, rotY);
					}

					if(firstTouch.phase == TouchPhase.Ended)
					{
						touched = false;
					}
				}

				if(Input.touchCount == 2)
				{
					Touch firstTouch = Input.GetTouch(0);
					Touch secondTouch = Input.GetTouch(1);

					float touchLength = 100f;

					if(secondTouch.phase == TouchPhase.Began)
					{
						Ray ray = Camera.main.ScreenPointToRay(secondTouch.position);
						RaycastHit hit;

						if(Physics.Raycast(ray, out hit))
						{
							if(hit.transform.gameObject == gameObject)
							{
								dualTouchOffset = Vector2.Distance(firstTouch.position, secondTouch.position);
								touched = true;
							}
							else
							{
								touched = false;
							}
						}
					}

					if(touched && secondTouch.phase == TouchPhase.Ended)
					{
						//Debug.Log("Touch #" + touch.fingerId + " last pos: " + touch.position);
						//Debug.Log("Delta Touch: " + deltaTouch);

						touchLength = Vector2.Distance(firstTouch.position, secondTouch.position) - dualTouchOffset;
						Debug.Log("Touch movement length: " + touchLength +
							"\nOffset: " + dualTouchOffset);
						if(touchLength >= UIManager.ins.DualTouchLength)
						{
							Disassemble();
						}

						touched = false;
					}

				}
			}
			//if(doubleTapCurrentRecoil > 0)
			//{
			//	doubleTapCurrentRecoil -= Time.deltaTime;
			//}
			//else
			//{
			//	tapCount = 0;
			//}
		}

		public void PartAssembled()
		{
			amountOfAssembledParts++;
			if(amountOfAssembledParts >= parts.Count)
			{
				UIManager.ins.HidePartPanel();
				DisselectPart();
				currentState = ObjectPart.States.Assembling;
				StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, startRotation, UIManager.ins.MovementTime));
			}
		}

		private bool IsAnyPartSubDisassembled()
		{
			foreach(var c in parts)
			{
				if(!c.Assembled)
				{
					return true;
				}
			}
			return false;
		}

	}
}
