using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectSubPart : MonoBehaviour
	{

		ObjectPart.States currentState = ObjectPart.States.Idle;

		private bool selected = false;
		private bool disassembled = false;

		public string subPartName;

		[Header("Disassemble properties")]

		[SerializeField]
		private Vector3 disassemblePosition;
		[SerializeField]
		private Vector3 disassembleRotation;

		[Header("Settings")]

		[SerializeField]
		private float selectedScale = 1f;

		private Vector3 startLocalPosition;
		private Quaternion startLocalRotation;

		private BoxCollider myCollider;
		private ObjectPart myObjectPart;
		private GlowObject myGlowObject;

		public bool Disassembled
		{
			get
			{
				return disassembled;
			}

			set
			{
				disassembled = value;
			}
		}
		public bool Selected
		{
			get
			{
				return selected;
			}

			set
			{
				selected = value;
			}
		}

		private void Start()
		{
			if(GetComponent<BoxCollider>())
			{
				myCollider = GetComponent<BoxCollider>();
				myCollider.enabled = false;
			}

			myObjectPart = GetComponentInParent<ObjectPart>();
			myGlowObject = GetComponent<GlowObject>();

			gameObject.layer = LayerMask.NameToLayer("ObjectPart");

			startLocalPosition = transform.localPosition;
			startLocalRotation = transform.localRotation;
		}

		private void Update()
		{
			if(Disassembled)
			{
				transform.RotateAround(transform.position, Vector3.up, UIManager.ins.RotateSpeed * Time.deltaTime);
			}
		}

		public void Assemble()
		{
			//if(history.IsEmpty())
			//	return;
			StopAllCoroutines();
			currentState = ObjectPart.States.Assembling;
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, startLocalPosition, UIManager.ins.MovementTime));
			StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, startLocalRotation, UIManager.ins.MovementTime));
			if(selectedScale != 1f)
				StartCoroutine(PhysicalManipulation.LocalScale(transform, transform.localScale, Vector3.one, UIManager.ins.MovementTime));
			myCollider.enabled = false;
			myObjectPart.PartAssembled();
			selected = false;
			myGlowObject.GlowOn();
			//disassembled = false;
		}

		public void Disassemble()
		{
			StartDisassemble();
			myGlowObject.GlowOff();
			myCollider.enabled = true;
		}

		private void StartDisassemble()
		{
			//StartCoroutine(Move(transform.position, new Vector3(Random.Range(-15, 5), Random.Range(-15, 5), transform.position.z), 1f));
			StopAllCoroutines();
			currentState = ObjectPart.States.Disassembling;
			StartCoroutine(PhysicalManipulation.Move(transform, transform.position, disassemblePosition, UIManager.ins.MovementTime));
			StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, Quaternion.Euler(disassembleRotation), UIManager.ins.MovementTime));
		}

		private void Select()
		{
			if(Disassembled)
			{
				currentState = ObjectPart.States.Selecting;
				myObjectPart.SelectPart(this);
				myGlowObject.GlowOn();
			}
		}

		public void GetSelected()
		{
			StopAllCoroutines();
			StartCoroutine(PhysicalManipulation.Move(transform, transform.position, UIManager.ins.PartPositionOnPanel, UIManager.ins.MovementTime));
			if(selectedScale != 1f)
				StartCoroutine(PhysicalManipulation.LocalScale(transform, transform.localScale, Vector3.one * selectedScale, UIManager.ins.MovementTime));
		}

		public void GetUnSelected()
		{
			if(selected)
			{
				StopAllCoroutines();
				StartCoroutine(PhysicalManipulation.Move(transform, transform.position, disassemblePosition, UIManager.ins.MovementTime));
				if(selectedScale != 1f)
					StartCoroutine(PhysicalManipulation.LocalScale(transform, transform.localScale, Vector3.one, UIManager.ins.MovementTime));
				myGlowObject.GlowOff();
			}
		}

		private void UnSelect()
		{
			if(Disassembled)
			{
				currentState = ObjectPart.States.UnSelecting;
				myObjectPart.DisselectPart();
				GetUnSelected();
			}
		}

		public void PlaySound()
		{
			AudioManager.ins.Play(subPartName);
		}

		public void OnCoroutineEnd()
		{
			switch(currentState)
			{
				case ObjectPart.States.Assembling:
					Disassembled = false;
					break;
				case ObjectPart.States.Disassembling:
					Disassembled = true;
					break;
				case ObjectPart.States.Selecting:
					GetComponent<GlowObject>().GlowOn();
					Selected = true;
					//PlaySound();
					break;
				case ObjectPart.States.UnSelecting:
					GetComponent<GlowObject>().GlowOff();
					Selected = false;
					break;
				default:
					break;
			}
			currentState = ObjectPart.States.Idle;
		}

		public void RotateAround(float rotX, float rotY)
		{
			transform.RotateAround(transform.position, Vector3.up, -rotX);
			transform.RotateAround(transform.position, Vector3.right, rotY);
		}

	}

}