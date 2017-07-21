using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : ScriptableObject {

	public string CategoryName;
	public List<GameObject> prefabs;
	public Color BackgroundColor;
	public Sprite Icon;
}
