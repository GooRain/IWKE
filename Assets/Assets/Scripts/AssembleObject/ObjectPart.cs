using System.Collections;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectPart : MonoBehaviour
	{

		[HideInInspector]
		public bool selected = false;

		public string soundName;

		[Header("Disassemble properties")]
		[SerializeField]
		private Vector3 disassemblePosition;
		[SerializeField]
		private Vector3 disassembleRotation;

		[Header("Settings")]
		private float soundRecoil = 2f;
		private float soundCooldown;

		private Vector3 startPosition;
		private float rotateSpeed;
		//private PartHistory history;
		private bool disassembled = false;
		private BoxCollider myCollider;
		private ObjectMain myObjectMain;

		private void Start()
		{
			//history = new PartHistory();
			if(GetComponent<BoxCollider>())
			{
				myCollider = GetComponent<BoxCollider>();
				myCollider.enabled = false;
			}
			myObjectMain = GetComponentInParent<ObjectMain>();
			rotateSpeed = myObjectMain.rotateSpeed;
			gameObject.layer = LayerMask.NameToLayer("ObjectPart");
			startPosition = transform.localPosition;
			soundCooldown = soundRecoil;
			//disassemblePosition += transform.localPosition;
		}

		private void Update()
		{
			HandleTouch();
			if(disassembled)
			{
				transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
			}
			if(soundCooldown >= 0)
			{
				soundCooldown -= Time.deltaTime;
			}
		}

		private void Select()
		{
			if(disassembled)
			{
				myObjectMain.SelectPart(this);
			}
		}
		public void GetSelected()
		{
			//history.SaveState(new PartMemento(transform.localPosition));
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.Move(transform, transform.position, UIManager.ins.partPositionOnPanel, UIManager.ins.movementTime));
			selected = true;
			//PlaySound();
		}
		private void Disselect()
		{
			if(disassembled)
			{
				myObjectMain.DisselectPart();
			}
		}
		public void GetDisselected()
		{
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, disassemblePosition, UIManager.ins.movementTime));
			//StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), 1f));
			selected = false;
		}

		private void HandleTouch()
		{
			if(Input.touchCount > 0 && disassembled && selected)
			{
				Touch touch0 = Input.GetTouch(0);

				if(Input.touchCount == 1 && touch0.phase == TouchPhase.Moved)
				{
					//if(Mathf.Abs(touch0.deltaPosition.x) > Mathf.Abs(touch0.deltaPosition.y))
					//{
					//	transform.RotateAround(transform.position, transform.up, -touch0.deltaPosition.x * rotateSpeed * Time.deltaTime);
					//	//Debug.Log("Touch0.x: " + firstTouch.deltaPosition.x);
					//}
					float rotX = touch0.deltaPosition.x * rotateSpeed * Mathf.Deg2Rad;
					float rotY = touch0.deltaPosition.y * rotateSpeed * Mathf.Deg2Rad;

					transform.RotateAround(transform.position, Vector3.up, -rotX);
					transform.RotateAround(transform.position, Vector3.right, rotY);
				}
			}
		}

		//private void CheckTouch(Vector2 pos)
		//{
		//	Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pos);
		//	Vector2 touchPosition = new Vector2(worldPosition.x, worldPosition.y);
		//	Collider2D hit = Physics2D.OverlapPoint(touchPosition);
		//	if(hit)
		//	{
		//		Debug.Log(hit.transform.gameObject.name);
		//		myObjectMain.SelectPart(this);
		//		PhysicalManipulation.Move(transform, transform.localPosition, new Vector3(-125, 50), 1f);
		//		//hit.transform.gameObject.SendMessage("Select");
		//	}
		//}

		public void Disassemble()
		{
			//history.SaveState(new PartMemento(transform.localPosition));
			StartDisassemble();
			myCollider.enabled = true;
			disassembled = true;
		}

		public void Assemble()
		{
			//if(history.IsEmpty())
			//	return;
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, startPosition, UIManager.ins.movementTime));
			StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, Quaternion.identity, UIManager.ins.movementTime));
			myCollider.enabled = false;
			disassembled = false;
		}

		private void StartDisassemble()
		{
			//StartCoroutine(Move(transform.position, new Vector3(Random.Range(-15, 5), Random.Range(-15, 5), transform.position.z), 1f));
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, disassemblePosition, UIManager.ins.movementTime));
			StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), UIManager.ins.movementTime));
		}

		public void PlaySound()
		{
			if(soundCooldown<=0)
			{
				AudioManager.ins.Play(soundName);
				soundCooldown = soundRecoil;
			}
		}
	}
}