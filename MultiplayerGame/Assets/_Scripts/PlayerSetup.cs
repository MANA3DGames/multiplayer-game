using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSetup : NetworkBehaviour {

	public Color m_playerColor;
	public string m_basename = "PLAYER";
	public int m_playerNum = 1;
	public Text m_playerNameText;


	void Start()
	{

	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		if (m_playerNameText != null)
		{
			m_playerNameText.enabled = false;
		}
	}
		


	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in meshes)
		{
			r.material.color = m_playerColor;
		}

		if (m_playerNameText != null)
		{
			m_playerNameText.enabled = true;
			m_playerNameText.text = m_basename + m_playerNum.ToString();

		}


	}

}
