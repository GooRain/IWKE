using System.Collections;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectPart : MonoBehaviour
	{

		public bool selected = false;

		[SerializeField]
		private string partName;
		[SerializeField]
		private AudioSource sound;
		[Header("Disassemble properties")]
		[SerializeField]
		private Vector3 disassemblePosition;
		[SerializeField]
		private Vector3 disassembleRotation;

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
			//disassemblePosition += transform.localPosition;
		}

		private void Update()
		{
			//HandleTouch();
			if(disassembled)
			{
				transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
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
			StartCoroutine(PhysicalManipulation.Move(transform, transform.localPosition, UIManager.ins.partPositionOnPanel, 1f));
			selected = true;
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
			StartCoroutine(PhysicalManipulation.Move(transform, transform.localPosition, disassemblePosition, 1f));
			//StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), 1f));
			selected = false;
		}

		//private void HandleTouch()
		//{
		//	if(disassembled && Input.touchCount > 0)
		//	{
		//		if(Input.GetTouch(0).phase == TouchPhase.Began)
		//		{
		//			CheckTouch(Input.GetTouch(0).position);
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
			disassembled = true;
		}

		public void Assemble()
		{
			//if(history.IsEmpty())
			//	return;
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.Move(transform, transform.localPosition, startPosition, 1f));
			StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, Quaternion.identity, 1f));
			myCollider.enabled = false;
			disassembled = false;
		}

		private void StartDisassemble()
		{
			//StartCoroutine(Move(transform.position, new Vector3(Random.Range(-15, 5), Random.Range(-15, 5), transform.position.z), 1f));
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.Move(transform, transform.localPosition, disassemblePosition, 1f));
			StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), 1f));
		}
	}
}