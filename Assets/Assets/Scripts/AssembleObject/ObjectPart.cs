using System.Collections.Generic;
using UnityEngine;

namespace AssembleObject
{

	public class ObjectPart : MonoBehaviour
	{
		public enum States { Idle, Assembling, Disassembling, Selecting, UnSelecting };
		States currentState = States.Idle;

		private bool selected = false;
		private bool disassembled = false;

		public string partName;
		[HideInInspector]
		public int partIndex = 0;

		[Header("Disassemble properties")]
		[SerializeField]
		private Vector3 disassemblePosition;
		[SerializeField] private Vector3 disassembleRotation;

		[Header("Settings")]

		[SerializeField]
		private float selectedScale = 1f;

		private int amountOfAssembledSubParts = 0;
		private bool assembled = true;
		private bool hasSubParts = false;

		//private float soundRecoil = 1f;
		//private float soundCooldown;

		private Vector3 startLocalPosition;
		private Quaternion startLocalRotation;
		private Vector3 assembledPosition;

		//private float rotateSpeed;
		//private PartHistory history;

		private BoxCollider myCollider;
		private ObjectMain myObjectMain;

		private ObjectSubPart[] subParts;
		private ObjectSubPart selectedSubPart;

		private GlowObject[] myGlowObjects;

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
		public Vector3 AssembledPosition
		{
			get
			{
				return assembledPosition;
			}

			set
			{
				assembledPosition = value;
			}
		}
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
		public bool Assembled
		{
			get
			{
				return assembled;
			}

			set
			{
				assembled = value;
			}
		}

		private void Awake()
		{
			InitSubParts();
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

			myGlowObjects = GetComponentsInChildren<GlowObject>();

			gameObject.layer = LayerMask.NameToLayer("ObjectPart");

			startLocalPosition = transform.localPosition;
			startLocalRotation = transform.localRotation;
		}

		public void InitAssembledPosition()
		{
			assembledPosition = transform.position;
		}

		private void InitSubParts()
		{
			if(transform.childCount > 1)
			{
				subParts = GetComponentsInChildren<ObjectSubPart>();
				hasSubParts = true;
			}
		}

		private void Update()
		{
			if(disassembled && assembled)
			{
				transform.RotateAround(transform.position, Vector3.up, UIManager.ins.RotateSpeed * Time.deltaTime / 2);
			}
		}

		public void OnLangChange(int mainIndex, int partIndex)
		{
			if(transform.childCount > 1)
			{
				for(int i = 0; i < subParts.Length; i++)
				{
					subParts[i].subPartName = LangSystem.language.objects[mainIndex].parts[partIndex].subParts[i];
				}
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
			if(selectedScale != 1f)
				StartCoroutine(PhysicalManipulation.LocalScale(transform, transform.localScale, Vector3.one * selectedScale, UIManager.ins.MovementTime));

			//selected = true;
			//PlaySound();
		}

		private void UnSelect()
		{
			if(disassembled)
			{
				currentState = States.UnSelecting;
				SubAssemble();
				myObjectMain.DisselectPart();
				GetUnSelected();
			}
		}

		public void GetUnSelected()
		{
			//currentState = States.UnSelecting;
			if(assembled)
			{
				StopAllCoroutines();
				StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, Quaternion.Euler(disassembleRotation), 1f));
				StartCoroutine(PhysicalManipulation.Move(transform, transform.position, disassemblePosition, UIManager.ins.MovementTime));
				if(selectedScale != 1f)
					StartCoroutine(PhysicalManipulation.LocalScale(transform, transform.localScale, Vector3.one, UIManager.ins.MovementTime));
				//selected = false;
			}

			GlowOff();
		}

		public void RotateAround(float rotX, float rotY)
		{
			transform.RotateAround(transform.position, Vector3.up, -rotX);
			transform.RotateAround(transform.position, Vector3.right, rotY);
		}

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
			if(selected)
			{
				UnSelect();
			}
			StopAllCoroutines();
			currentState = States.Assembling;
			//Debug.Log(soundName + " @ start local position: " + startLocalPosition);
			StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, startLocalPosition, UIManager.ins.MovementTime));
			StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, startLocalRotation, UIManager.ins.MovementTime));

			if(selectedScale != 1f)
			{
				StartCoroutine(PhysicalManipulation.LocalScale(transform, transform.localScale, Vector3.one, UIManager.ins.MovementTime));
			}

			if(!assembled)
			{
				SubAssemble();
			}

			myCollider.enabled = false;
			selected = false;
			//disassembled = false;
		}

		public void SinglePartAssemble()
		{
			Assemble();
			myObjectMain.PartAssembled();
		}

		private void StartDisassemble()
		{
			//StartCoroutine(Move(transform.position, new Vector3(Random.Range(-15, 5), Random.Range(-15, 5), transform.position.z), 1f));
			StopAllCoroutines();
			currentState = States.Disassembling;
			StartCoroutine(PhysicalManipulation.Move(transform, transform.position, disassemblePosition, UIManager.ins.MovementTime));
			StartCoroutine(PhysicalManipulation.Rotate(transform, transform.rotation, Quaternion.Euler(disassembleRotation), UIManager.ins.MovementTime));

			//StartCoroutine(PhysicalManipulation.LocalMove(transform, transform.localPosition, disassemblePosition, UIManager.ins.MovementTime));
			//StartCoroutine(PhysicalManipulation.LocalRotate(transform, transform.localRotation, Quaternion.Euler(disassembleRotation), UIManager.ins.MovementTime));
		}

		public void SubDisassemble()
		{
			if(hasSubParts)
			{
				StartCoroutine(PhysicalManipulation.Rotate(transform, transform.localRotation, startLocalRotation, UIManager.ins.MovementTime));
				foreach(var c in subParts)
				{
					c.Disassemble();
				}
				myCollider.enabled = false;
				assembled = false;
				amountOfAssembledSubParts = 0;
			}
		}

		public void SubAssemble()
		{
			if(hasSubParts)
			{
				DisselectPart();
				foreach(ObjectSubPart c in subParts)
				{
					c.gameObject.SetActive(true);
					c.GetUnSelected();
					c.Assemble();
				}
			}
			assembled = true;
		}

		public void PlaySound()
		{
			//if(soundCooldown <= 0 && currentState == States.Idle)
			//{
			AudioManager.ins.Play(partName, PlayerPrefs.GetString("Language", "en_US"));
			//	soundCooldown = soundRecoil;
			//}
		}

		public void OnCoroutineEnd()
		{
			switch(currentState)
			{
				case States.Assembling:
					disassembled = false;
					assembledPosition = transform.position;
					break;
				case States.Disassembling:
					disassembled = true;
					break;
				case States.Selecting:
					GlowOn();
					selected = true;
					//PlaySound();
					break;
				case States.UnSelecting:
					GlowOff();
					selected = false;
					break;
				default:
					break;
			}
			currentState = States.Idle;
		}

		public void PartAssembled()
		{
			amountOfAssembledSubParts++;
			if(amountOfAssembledSubParts >= subParts.Length)
			{
				//UIManager.ins.HidePartPanel();
				DisselectPart();
				myCollider.enabled = true;
				assembled = true;
			}
		}

		public void SelectPart(ObjectSubPart subPart)
		{
			foreach(ObjectSubPart obj in subParts)
			{
				if(obj != subPart)
				{
					obj.gameObject.SetActive(false);
				}
			}
			selectedSubPart = subPart;
			selectedSubPart.GetSelected();
			UIManager.ins.ShowPartPanel(subPart.subPartName);
		}

		public void DisselectPart()
		{
			foreach(ObjectSubPart obj in subParts)
			{
				obj.gameObject.SetActive(true);
			}
			UIManager.ins.ShowPartPanel(partName);
		}

		public void GlowOn()
		{
			foreach(GlowObject go in myGlowObjects)
			{
				go.GlowOn();
			}
		}

		public void GlowOff()
		{
			foreach(GlowObject go in myGlowObjects)
			{
				go.GlowOff();
			}
		}
	}
}