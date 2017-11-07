using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LangSystem : MonoBehaviour
{

	private string jsonData;

	public static Lang language = new Lang();

	public static LangSystem ins;

	public delegate void ChooseLanguage();

	public event ChooseLanguage OnLangChange = () => { };

	public UIPanel languagePanel;
	public UITextChanger[] UITexts;

	//private string[] langArray = { "ru_RU", "en_US" };
	//private int langIndex = 1;

	private void Awake()
	{
		if(ins == null)
		{
			ins = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		if(!PlayerPrefs.HasKey("Language"))
		{
			if(Application.systemLanguage == SystemLanguage.Russian)
			{
				PlayerPrefs.SetString("Language", "ru_RU");
			}
			else
			{
				PlayerPrefs.SetString("Language", "en_US");
			}
			languagePanel.Show();
		}
		OnLangChange += ChangeTexts;
		//LangLoad();
	}

	private void Start()
	{
		LangLoad();
	}

	private void LangLoad()
	{

//#if UNITY_ANDROID
		//string path = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerPrefs.GetString("Language", "en_US") + ".json");
		//WWW reader = new WWW(path);
		//while(!reader.isDone) { }
		//jsonData = reader.text;
		//language = JsonUtility.FromJson<Lang>(jsonData);

		TextAsset file = Resources.Load<TextAsset>("Languages/" + PlayerPrefs.GetString("Language", "en_US"));
		jsonData = file.text;
		language = JsonUtility.FromJson<Lang>(jsonData);
//#endif

//#if UNITY_EDITOR
//		jsonData = File.ReadAllText(Application.streamingAssetsPath + "/Languages/" + PlayerPrefs.GetString("Language", "en_US") + ".json");
//		language = JsonUtility.FromJson<Lang>(jsonData);
//#endif

		OnLangChange();
	}

	public void ChangeTexts()
	{
		foreach(UITextChanger item in UITexts)
		{
			item.ChangeText();
		}
	}

	public void SelectLanguage(string lang)
	{
		PlayerPrefs.SetString("Language", lang);
		if(languagePanel != null)
		{
			languagePanel.Hide();
		}
		LangLoad();
	}

}

[System.Serializable]
public class UITextChanger
{
	public string name;
	public int index;
	public Text textComponent;

	public void ChangeText()
	{
		textComponent.text = LangSystem.language.ui[index];
	}
}

public class Lang
{
	public string[] ui = new string[5];
	public string[] categories = new string[4];
	//public string[] objects = new string[7];
	public LangObject[] objects = new LangObject[7];
}

[System.Serializable]
public class LangObject
{
	public string name;
	public LangPart[] parts = new LangPart[10];
}

[System.Serializable]
public class LangPart
{
	public string name;
	public string[] subParts = new string[10];
}