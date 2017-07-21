using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IWKE;

public class CategoryButton : MonoBehaviour {

	[SerializeField] private Text buttonName;
	[SerializeField] private Image icon;

	private Button button;
	private CategoryPanel categoryPanel;
	private Category category;

	private void Awake() {
		button = GetComponent<Button>();
		button.onClick.AddListener(()=> OpenMenu());
	}

	public void SetButton(Category category, CategoryPanel categoryPanel){
		this.category = category;
		this.categoryPanel = categoryPanel;
		buttonName.text = category.CategoryName;
		icon.sprite = category.Icon;
	}

	private void OpenMenu() {
		categoryPanel.SetPanel(category);
		categoryPanel.gameObject.GetComponent<IUserInterfaceElement>().Show();
	}
}
