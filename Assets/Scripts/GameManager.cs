using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private void Awake()
	{
		transform.SetParent(null);

		if (UnityEngine.Object.FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length == 1)
		{
			GameManager.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return;
		}

		UnityEngine.Object.Destroy(gameObject);
	}

	public static GameManager Instance;
}
