using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryView : MonoBehaviour
{

	[SerializeField] List<Category> categories;
	[SerializeField] GameObject buttonPrefab;
	[SerializeField] CategoryPanel categoryPanel;
	private void Awake()
	{
		InitButtons();
	}

	private void InitButtons()
	{
		for(int i = 0; i < categories.Count; i++)
		{
			var go = Instantiate(buttonPrefab, transform);
			go.GetComponent<CategoryButton>().SetButton(i, categories[i], categoryPanel);
			go.transform.SetParent(transform);
			go.transform.localScale = new Vector3(1, 1, 1);
			go.GetComponent<RectTransform>().localPosition = Vector3.zero;
		}
	}
}
