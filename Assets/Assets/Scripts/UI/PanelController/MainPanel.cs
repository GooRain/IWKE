using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IWKE;

public class MainPanel : MonoBehaviour {

	[SerializeField] private GameObject obj;

	private IUserInterfaceElement panel;
	private CategoryPanel categoryPanel;

	//private void Awake() {
	//	panel = obj.GetComponent<IUserInterfaceElement>();
	//	categoryPanel = obj.GetComponent<CategoryPanel>();
	//}

	//public void OpenPanel(Color color, Category category) {
	//	panel.Show();
	//	categoryPanel.SetPanel(color, category);
	//}
}
