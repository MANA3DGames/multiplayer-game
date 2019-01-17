using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class GameManager : NetworkBehaviour 
{
	static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance ==  null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();

				if (instance == null)
				{
					instance = new GameObject().AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

}
