using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class Sound
{

	public string name = "sound";
	public AudioClip clip;

	[Range(0, 1)]
	public float volume = .75f;
	[Range(-3, 3)]
	public float pitch = 1f;

	public bool loop;

	[HideInInspector]
	public AudioSource source;

}
