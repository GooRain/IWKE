using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IWKE;

public class CategoryButton : MonoBehaviour
{

	[SerializeField] private Text buttonName;
	[SerializeField] private Image icon;

	private int index = 0;
	private Button button;
	private CategoryPanel categoryPanel;
	private Category category;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(() => OpenMenu());
	}

	private void Start()
	{
		LangSystem.ins.OnLangChange += OnLangChange;
		OnLangChange();
	}

	public void SetButton(int newIndex, Category category, CategoryPanel categoryPanel)
	{
		this.category = category;
		this.categoryPanel = categoryPanel;
		index = newIndex;
		buttonName.text = category.CategoryName;
		icon.sprite = category.Icon;
	}

	public void OnLangChange()
	{
		buttonName.text = LangSystem.language.categories[index];
	}

	private void OpenMenu()
	{
		categoryPanel.SetPanel(index, category);
		categoryPanel.gameObject.GetComponent<IUserInterfaceElement>().Show();
	}
}
