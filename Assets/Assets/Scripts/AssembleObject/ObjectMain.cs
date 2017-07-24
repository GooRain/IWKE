using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectMain : MonoBehaviour
	{
		[SerializeField] private string soundName;
		[SerializeField] private float rotateSpeed = 5f;
		[SerializeField] private float doubleTapRecoil = 0.25f;

		private float doubleTapCurrentRecoil;
		private int tapCount = 0;

		public bool assembled = true;

		private List<ObjectPart> parts;
		private ObjectPart selectedPart;

		private BoxCollider myCollider;
		private Quaternion startRotation;

		public float RotateSpeed
		{
			get
			{
				return rotateSpeed;
			}

			private set
			{
				rotateSpeed = value;
			}
		}

		public string SoundName
		{
			get
			{
				return soundName;
			}

			set
			{
				soundName = value;
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
			parts = new List<ObjectPart>();
			SetChildren();
			startRotation = transform.rotation;
		}

		public void Disassemble()
		{
			StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, startRotation, 1f));
			foreach(var c in parts)
			{
				c.Disassemble();
			}
			myCollider.enabled = false;
			assembled = false;
		}

		public void Assemble()
		{
			UIManager.ins.HidePartPanel();
			DisselectPart();
			foreach(var c in parts)
			{
				c.Assemble();
			}
			myCollider.enabled = true;
			assembled = true;
		}

		private void Update()
		{
			HandleTouch();
			if(assembled)
			{
				transform.RotateAround(transform.position, transform.up, rotateSpeed / 2 * Time.deltaTime);
			}
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
			UIManager.ins.ShowPartPanel(part.soundName);
		}

		public void DisselectPart()
		{
			//StopAllCoroutines();
			//StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, startRotation, 1f));
			foreach(ObjectPart obj in parts)
			{
				obj.gameObject.SetActive(true);
			}
			if(selectedPart != null)
			{
				selectedPart.GetUnSelected();
			}
			UIManager.ins.HidePartPanel();
		}

		void HandleTouch()
		{
			if(Input.touchCount > 0 && assembled)
			{
				Touch touch0 = Input.GetTouch(0);

				if(touch0.phase == TouchPhase.Began)
				{
					Ray ray = Camera.main.ScreenPointToRay(touch0.position);
					RaycastHit hit;

					if(Physics.Raycast(ray, out hit))
					{
						if(hit.transform.gameObject == gameObject)
						{
							if(doubleTapCurrentRecoil > 0 && tapCount == 1)
							{
								Disassemble();
							}
							else
							{
								doubleTapCurrentRecoil = doubleTapRecoil;
								tapCount++;
							}
						}
					}
				}

				if(Input.touchCount == 1 && touch0.phase == TouchPhase.Moved)
				{
					//if(Mathf.Abs(touch0.deltaPosition.x) > Mathf.Abs(touch0.deltaPosition.y))
					//{
					//	transform.RotateAround(transform.position, transform.up, -touch0.deltaPosition.x * rotateSpeed * Time.deltaTime);
					//	//Debug.Log("Touch0.x: " + firstTouch.deltaPosition.x);
					//}
					float rotX = touch0.deltaPosition.x * rotateSpeed * Mathf.Deg2Rad;
					//float rotY = touch0.deltaPosition.y * rotateSpeed * Mathf.Deg2Rad;

					transform.RotateAround(transform.position, Vector3.up, -rotX);
					//transform.RotateAround(transform.position, Vector3.right, rotY);
				}

				if(Input.touchCount == 2)
				{
					if(touch0.phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
					{
						Disassemble();
					}
				}

			}
			if(doubleTapCurrentRecoil > 0)
			{
				doubleTapCurrentRecoil -= Time.deltaTime;
			}
			else
			{
				tapCount = 0;
			}
		}

	}
}
