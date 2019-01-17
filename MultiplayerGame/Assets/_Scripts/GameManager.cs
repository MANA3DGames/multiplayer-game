using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class GameManager : NetworkBehaviour 
{
	
	public Text m_messageText;

	public int m_minPlayers = 2;
	int m_maxPlayers = 4;

	[SyncVar]
	public int m_playerCount = 0;

	public Color[] m_playerColors = {Color.red, Color.blue, Color.green, Color.magenta};

	static GameManager instance;

	public List<PlayerController> m_allPlayers;

	public List<Text> m_nameLabelText;
	public List<Text> m_playerScoreText;

	public int m_maxScore = 3;

	[SyncVar]
	bool m_gameOver = false;

	PlayerController m_winner;

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

	void Start()
	{
		StartCoroutine("GameLoopRoutine");
	}

	IEnumerator GameLoopRoutine()
	{
		yield return StartCoroutine("EnterLobby");
		yield return StartCoroutine("PlayGame");
		yield return StartCoroutine("EndGame");
		StartCoroutine("GameLoopRoutine");
	}

	IEnumerator EnterLobby()
	{


		while (m_playerCount < m_minPlayers)
		{
			UpdateMessage ("waiting for players");

			DisablePlayers();
			yield return null;
		}
	}

	IEnumerator PlayGame()
	{
		yield return new WaitForSeconds(2f);
		UpdateMessage ("3");
		yield return new WaitForSeconds(1f);
		UpdateMessage ("2");
		yield return new WaitForSeconds(1f);
		UpdateMessage ("1");
		yield return new WaitForSeconds(1f);
		UpdateMessage ("FIGHT");
		yield return new WaitForSeconds(1f);

		EnablePlayers();
		UpdateScoreboard();

		UpdateMessage ("");

		PlayerController winner = null;

		while (m_gameOver == false)
		{
			yield return null;
		}

	}

	IEnumerator EndGame()
	{
		DisablePlayers();

		UpdateMessage ("GAME OVER \n " + m_winner.m_pSetup.m_playerNameText.text + " wins!");
		Reset();

		yield return new WaitForSeconds (3f);
		UpdateMessage("Restarting...");
		yield return new WaitForSeconds (3f);
	}

	[ClientRpc]
	void RpcSetPlayerState(bool state)
	{
		PlayerController[] allPlayers = GameObject.FindObjectsOfType<PlayerController>();
		foreach (PlayerController p in allPlayers)
		{
			p.enabled = state;
		}
	}

	void EnablePlayers()
	{
		if (isServer)
		{
			RpcSetPlayerState(true);
		}
	}

	void DisablePlayers()
	{
		if (isServer)
		{
			RpcSetPlayerState(false);
		}
	}

	public void AddPlayer(PlayerSetup pSetup)
	{
		if (m_playerCount < m_maxPlayers)
		{
			m_allPlayers.Add(pSetup.GetComponent<PlayerController>());
			pSetup.m_playerColor = m_playerColors[m_playerCount];
			pSetup.m_playerNum = m_playerCount + 1;
		}
	}

	[ClientRpc]
	void RpcUpdateScoreboard(string[] playerNames, int[] playerScores)
	{
		for (int i = 0; i < m_playerCount; i++)
		{
			if (playerNames[i] != null)
			{
				m_nameLabelText[i].text = playerNames[i];
			}

			if (playerScores[i] != null)
			{
				m_playerScoreText[i].text = playerScores[i].ToString();
			}
		}

	}

	public void UpdateScoreboard()
	{
		if (isServer)
		{

			m_winner = GetWinner();
			if (m_winner !=null)
			{
				m_gameOver = true;
			}

			string[] names = new string[m_playerCount];
			int[] scores = new int[m_playerCount];
			for (int i = 0; i < m_playerCount; i++)
			{
				names[i] = m_allPlayers[i].GetComponent<PlayerSetup>().m_playerNameText.text;
				scores[i] = m_allPlayers[i].m_score;
			}

			RpcUpdateScoreboard(names,scores);
		}
	}


	[ClientRpc]
	void RpcUpdateMessage (string msg)
	{
		if (m_messageText != null) 
		{
			m_messageText.gameObject.SetActive (true);
			m_messageText.text = msg;
		}
	}

	public void UpdateMessage(string msg)
	{
		if (isServer)
		{
			RpcUpdateMessage(msg);

		}
	}


	PlayerController GetWinner()
	{
		if (isServer)
		{
			for (int i = 0; i < m_playerCount; i++)
			{
				if (m_allPlayers[i].m_score >= m_maxScore)
				{
					return m_allPlayers[i];
				}
			}
		}

		return null;
	}

	void Reset()
	{
		if (isServer)
		{
			RpcReset();
			UpdateScoreboard();
			m_winner = null;
			m_gameOver = false;
		}

	}

	[ClientRpc]
	void RpcReset()
	{
		PlayerController[] allPlayers = GameObject.FindObjectsOfType<PlayerController>();
		foreach (PlayerController p in allPlayers)
		{
			p.m_score = 0;
		}

	}

}
