using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

	public static AudioManager ins;

	public bool playMusic = true;
	public GameObject musicWaves;

	[SerializeField] private SoundLanguagePack[] soundLanguagePacks;

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

		foreach(SoundLanguagePack SLP in soundLanguagePacks)
		{
			foreach(SoundPack pack in SLP.soundPacks)
			{
				foreach(Sound sound in pack.sounds)
				{
					sound.source = gameObject.AddComponent<AudioSource>();
					sound.source.clip = sound.clip;

					sound.source.volume = sound.volume;
					sound.source.pitch = sound.pitch;
					sound.source.loop = sound.loop;
				}
			}
		}

	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.S))
		{
			ScreenCapture.CaptureScreenshot("Screenshots/screenshot" + DateTime.Now.Second.ToString() + ".png");
			Debug.Log("Screenshot" + DateTime.Now.Second.ToString() + " has been captured!");
		}
	}

	public void Play(string name)
	{
		foreach(SoundLanguagePack SLP in soundLanguagePacks)
		{
			foreach(SoundPack pack in SLP.soundPacks)
			{
				Sound snd = Array.Find(pack.sounds, sound => sound.name == name);
				if(snd != null && !snd.source.isPlaying)
				{
					snd.source.Play();
					return;
				}
			}
		}
		Debug.LogWarning("Sound: " + name + " not found!");
	}

	public void Play(string name, string language)
	{
		SoundLanguagePack currentSLP = null;
		foreach(SoundLanguagePack SLP in soundLanguagePacks)
		{
			if(SLP.name == language)
			{
				currentSLP = SLP;
				break;
			}
		}
		if(currentSLP != null)
		{
			foreach(SoundPack pack in currentSLP.soundPacks)
			{
				Sound snd = Array.Find(pack.sounds, sound => sound.name == name);
				if(snd != null && !snd.source.isPlaying)
				{
					snd.source.Play();
					return;
				}
			}
		}
	}

	public void ToggleMusic()
	{
		playMusic = !playMusic;
		musicWaves.SetActive(playMusic);
	}

}
