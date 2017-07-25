using System.Collections;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectPart : MonoBehaviour
	{
		enum States { Idle, Assembling, DisAssembling, Selecting, UnSelecting };
		States currentState = States.Idle;

		private bool selected = false;
		private bool disassembled = false;

		public string soundName;

		[Header("Disassemble properties")]
		[SerializeField]
		private Vector3 disassemblePosition;
		[SerializeField] private Vector3 disassembleRotation;

		[Header("Settings")]

		private float soundRecoil = 1f;
		private float soundCooldown;

		private Vector3 startLocalPosition;
		//private float rotateSpeed;
		//private PartHistory history;

		private BoxCollider myCollider;
		private ObjectMain myObjectMain;
		private GlowObject myGlowObject;

		public bool Selected
		{
			get
			{
				return selected;
			}

			private set
			{
				selected = value;
			}
		}

		private void Start()
		{
			//history = new PartHistory();
			if(GetComponent<BoxCollider>())
			{
				myCollider = GetComponent<BoxCollider>();
				myCollider.enabled = false;
			}
			myObjectMain = GetComponentInParent<ObjectMain>();
			myGlowObject = GetComponentInChildren<GlowObject>();
			gameObject.layer = LayerMask.NameToLayer("ObjectPart");
			startLocalPosition = transform.localPosition;
			soundCooldown = soundRecoil;
			//disassemblePosition += transform.localPosition;
		}

		private void Update()
		{
			//HandleTouch();
			if(disassembled)
			{
				transform.RotateAround(transform.position, Vector3.up, UIManager.ins.RotateSpeed * Time.deltaTime);
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
				currentState = States.Selecting;
				myObjectMain.SelectPart(this);
			}
		}
		public void GetSelected()
		{
			//history.SaveState(new PartMemento(transform.localPosition));
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.Move(transform, transform.position, UIManager.ins.PartPositionOnPanel, UIManager.ins.MovementTime));
			//selected = true;
			//PlaySound();
		}
		private void UnSelect()
		{
			if(disassembled)
			{
				currentState = States.UnSelecting;
				myObjectMain.DisselectPart();
			}
		}

		public void GetUnSelected()
		{
			//currentState = States.UnSelecting;
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, disassemblePosition, UIManager.ins.MovementTime));
			//StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), 1f));
			//selected = false;
		}

		public void RotateAround(float rotX, float rotY)
		{
			transform.RotateAround(transform.position, Vector3.up, -rotX);
			transform.RotateAround(transform.position, Vector3.right, rotY);
		}

		//private void HandleTouch()
		//{
		//	if(Input.touchCount > 0 && disassembled)
		//	{
		//		Touch touch0 = Input.GetTouch(0);

		//		if(Input.touchCount == 1 && touch0.phase == TouchPhase.Moved)
		//		{
		//			//if(Mathf.Abs(touch0.deltaPosition.x) > Mathf.Abs(touch0.deltaPosition.y))
		//			//{
		//			//	transform.RotateAround(transform.position, transform.up, -touch0.deltaPosition.x * rotateSpeed * Time.deltaTime);
		//			//	//Debug.Log("Touch0.x: " + firstTouch.deltaPosition.x);
		//			//}
		//			float rotX = touch0.deltaPosition.x * rotateSpeed * Mathf.Deg2Rad;
		//			float rotY = touch0.deltaPosition.y * rotateSpeed * Mathf.Deg2Rad;

		//			transform.RotateAround(transform.position, Vector3.up, -rotX);
		//			transform.RotateAround(transform.position, Vector3.right, rotY);
		//		}
		//	}
		//}

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
			//disassembled = true;
		}

		public void Assemble()
		{
			//if(history.IsEmpty())
			//	return;
			StopAllCoroutines();
			currentState = States.Assembling;
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, startLocalPosition, UIManager.ins.MovementTime));
			StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, Quaternion.identity, UIManager.ins.MovementTime));
			myCollider.enabled = false;
			//disassembled = false;
		}

		private void StartDisassemble()
		{
			//StartCoroutine(Move(transform.position, new Vector3(Random.Range(-15, 5), Random.Range(-15, 5), transform.position.z), 1f));
			StopAllCoroutines();
			currentState = States.DisAssembling;
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, disassemblePosition, UIManager.ins.MovementTime));
			StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), UIManager.ins.MovementTime));
		}

		public void PlaySound()
		{
			if(soundCooldown <= 0 && currentState == States.Idle)
			{
				AudioManager.ins.Play(soundName);
				soundCooldown = soundRecoil;
			}
		}

		public void OnCoroutineEnd()
		{
			switch(currentState)
			{
				case States.Assembling:
					disassembled = false;
					break;
				case States.DisAssembling:
					disassembled = true;
					break;
				case States.Selecting:
					myGlowObject.GlowOn();
					selected = true;
					break;
				case States.UnSelecting:
					myGlowObject.GlowOff();
					selected = false;
					break;
				default:
					break;
			}
			currentState = States.Idle;
		}
	}
}