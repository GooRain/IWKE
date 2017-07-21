using AssembleObject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryPanel : MonoBehaviour {

    public UIPanel partPanel;

    [SerializeField] private Text categoryName;
	[SerializeField] private Text objectName;
    [SerializeField] private Text partName;
	private UIPanel uiPanel;
	private Category category;
	private Image background;
	private LinkedList<GameObject> objects = new LinkedList<GameObject>();
	private LinkedListNode<GameObject> selectedObject;
	private GameObject currentGO;
	public Image Background {
		get {
			if (background == null)
				background = GetComponent<Image>();
			return background;
		}
		set {
			background = value;
		}
	}

	private void Awake() {
		uiPanel = GetComponent<UIPanel>();
		uiPanel.OnHide += Clear;
	}

	public void GetObjects() {
		foreach(var v in category.prefabs) {
			objects.AddLast(v);
		}
		selectedObject = objects.First;
	}

	public void SetPanel(Category category) {
		Background.color = category.BackgroundColor;
		this.category = category;
		categoryName.text = category.CategoryName.ToUpper();
		GetObjects();
		ShowObject();
	}

	public void Select(int direction) {
		if (direction >= 0) {
			if (selectedObject.Next == null)
				return;
			selectedObject = selectedObject.Next;
		}
		else {
			if (selectedObject.Previous == null)
				return;
			selectedObject = selectedObject.Previous;
		}
		ShowObject();
	}

	public void ShowObject() {
		Destroy(currentGO);
		currentGO = Instantiate(selectedObject.Value, transform);
		currentGO.transform.position = new Vector3(transform.position.x, transform.position.y-5f, transform.position.z -70f);
		objectName.text = currentGO.GetComponent<ObjectMain>().Name.ToUpper();
	}

	private void Clear() {
		objects.Clear();
		Destroy(currentGO);
	}

	public void Assemble() {
		currentGO.GetComponent<ObjectMain>().Assemble();
	}
	public void Disassemble() {
		currentGO.GetComponent<ObjectMain>().Disassemble();
	}
}
