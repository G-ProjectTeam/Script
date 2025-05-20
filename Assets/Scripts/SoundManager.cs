using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private void Awake()
	{
		transform.SetParent(null);

		if (UnityEngine.Object.FindObjectsByType<SoundManager>(FindObjectsSortMode.None).Length == 1)
		{
			SoundManager.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return;
		}

		UnityEngine.Object.Destroy(gameObject);
	}

	public void BGM(int id)
	{
		Debug.Log("BGM Change to --> " + id.ToString());
		src.Stop();
		if (id < 0)
		{
			return;
		}
		src.clip = BGMList[id];
		src.Play();
	}

	public static SoundManager Instance;
	public List<AudioClip> BGMList;
	public AudioSource src;
}
