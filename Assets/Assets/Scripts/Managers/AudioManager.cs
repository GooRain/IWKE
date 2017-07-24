using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

	public static AudioManager ins;

	[SerializeField] private SoundPack[] soundPacks;

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

		foreach(SoundPack pack in soundPacks)
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

	public void Play(string name)
	{
		foreach(SoundPack pack in soundPacks)
		{
			Sound snd = Array.Find(pack.sounds, sound => sound.name == name);
			if(snd != null && !snd.source.isPlaying)
			{
				snd.source.Play();
				return;
			}
		}
		Debug.LogWarning("Sound: " + name + " not found!");
	}

}
