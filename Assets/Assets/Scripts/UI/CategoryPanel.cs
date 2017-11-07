using AssembleObject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryPanel : MonoBehaviour
{

	[SerializeField] private Vector3 objectOffset = Vector3.zero;

	[Header("UI Panels")]
	[SerializeField]
	private UIPanel partPanel;
	[SerializeField] private UIPanel settingsPanel;

	[Header("Texts")]
	[SerializeField]
	private Text categoryName;
	[SerializeField] private Text objectName;
	[SerializeField] private Text partName;

	[Header("Background")]
	[SerializeField] private Image backgroundImage;
	[SerializeField] private Color bgShowColor;
	[SerializeField] private Background[] objectBackgrounds;


	private int index = 0;
	private UIPanel uiPanel;
	private Category category;
	private Image background;
	private LinkedList<GameObject> objects = new LinkedList<GameObject>();
	private LinkedListNode<GameObject> selectedObject;
	private GameObject currentGO;

	public Image Background
	{
		get
		{
			if(background == null)
				background = GetComponent<Image>();
			return background;
		}
		set
		{
			background = value;
		}
	}

	public UIPanel PartPanel
	{
		get
		{
			return partPanel;
		}

		set
		{
			partPanel = value;
		}
	}

	private void Awake()
	{
		uiPanel = GetComponent<UIPanel>();
		uiPanel.OnHide += Clear;
		uiPanel.OnHide += settingsPanel.Hide;
	}

	private void Start()
	{
		LangSystem.ins.OnLangChange += OnLangChange;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			uiPanel.Hide();
		}
	}

	public void GetObjects()
	{
		foreach(var v in category.prefabs)
		{
			objects.AddLast(v);
		}
		selectedObject = objects.First;
	}

	public void SetPanel(int newIndex, Category category)
	{
		Background.color = category.BackgroundColor;
		this.category = category;
		index = newIndex;
		OnLangChange();
		GetObjects();
		ShowObject();
	}

	private void OnLangChange()
	{
		categoryName.text = LangSystem.language.categories[index];
		if(currentGO != null)
		{
			ObjectMain currentOM = currentGO.GetComponent<ObjectMain>();
			objectName.text = LangSystem.language.objects[currentOM.objectIndex].name.ToUpper();
			if(currentOM.SelectedPart != null)
			{
				partName.text = LangSystem.language.objects[currentOM.objectIndex].parts[currentOM.SelectedPart.partIndex].name;
			}
			//objectName.text = currentGO.GetComponent<ObjectMain>().ObjectName;
		}
	}

	public void Select(int direction)
	{
		if(direction >= 0)
		{
			if(selectedObject.Next == null)
				return;
			selectedObject = selectedObject.Next;
		}
		else
		{
			if(selectedObject.Previous == null)
				return;
			selectedObject = selectedObject.Previous;
		}
		ShowObject();
	}

	public void ShowObject()
	{
		partPanel.Hide();
		Destroy(currentGO);
		currentGO = Instantiate(selectedObject.Value, transform);
		currentGO.transform.position = transform.position + objectOffset;
		string currentObjectName = LangSystem.language.objects[currentGO.GetComponent<ObjectMain>().objectIndex].name;
		objectName.text = currentObjectName.ToUpper();
		AudioManager.ins.Play(currentObjectName, PlayerPrefs.GetString("Language", "en_US"));
		currentGO.GetComponent<ObjectMain>().InitParts();
		currentGO.GetComponent<ObjectMain>().MyCategoryPanel = this;
		LoadBgImage();
	}

	private void Clear()
	{
		objects.Clear();
		partPanel.Hide();
		Destroy(currentGO);
		HideBgImage();
	}

	public void Assemble()
	{
		currentGO.GetComponent<ObjectMain>().Assemble();
	}

	public void Disassemble()
	{
		currentGO.GetComponent<ObjectMain>().Disassemble();
	}

	public void LoadBgImage()
	{
		if(currentGO != null)
		{
			backgroundImage.sprite = FindBg(currentGO.name);
			//ShowBgImage();
		}
		if(backgroundImage.sprite == null)
		{
			HideBgImage();
		}
	}

	public void HideBgImage()
	{
		backgroundImage.color = Color.clear;
	}

	public void ShowBgImage()
	{
		if(backgroundImage.sprite != null)
		{
			backgroundImage.color = bgShowColor;
		}
	}

	public Sprite FindBg(string name)
	{
		foreach(Background bg in objectBackgrounds)
		{
			if(bg.name + "(Clone)" == name)
			{
				return bg.background;
			}
		}
		return null;
	}

}
