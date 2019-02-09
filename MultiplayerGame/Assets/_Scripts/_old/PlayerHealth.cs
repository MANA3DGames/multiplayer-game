using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealth : NetworkBehaviour {

	[SyncVar(hook = "UpdateHealthBar")]
	float m_currentHealth;

	public float m_maxHealth = 3;

	public GameObject m_deathPrefab;

	[SyncVar]
	public bool m_isDead = false;

	public RectTransform m_healthBar;

	public PlayerController m_lastAttacker;

	// Use this for initialization
	void Start () 
	{
		Reset();

	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	void UpdateHealthBar(float value)
	{

		if (m_healthBar !=null)
		{
			m_healthBar.sizeDelta = new Vector2 (value/m_maxHealth * 150f, m_healthBar.sizeDelta.y);

		}
	}

	public void Damage (float damage, PlayerController pc = null)
	{
		if (!isServer)
		{
			return;
		}

		if (pc !=null && pc!= this.GetComponent<PlayerController>())
		{
			m_lastAttacker = pc;

		}

		m_currentHealth -= damage;
		UpdateHealthBar(m_currentHealth);

		if (m_currentHealth <= 0 && !m_isDead)
		{
			if (m_lastAttacker !=null)
			{
				m_lastAttacker.m_score ++;
				m_lastAttacker = null;
			}

			GameManager.Instance.UpdateScoreboard();

			m_isDead = true;
			RpcDie();
		}
	}

	[ClientRpc]
	void RpcDie()
	{
		if (m_deathPrefab)
		{
			GameObject deathFX = Instantiate (m_deathPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			GameObject.Destroy (deathFX, 3f);
		}

		SetActiveState(false);

		gameObject.SendMessage("Disable");
	}

	void SetActiveState(bool state)
	{
		foreach (Collider c in GetComponentsInChildren<Collider>())
		{
			c.enabled = state;
		}

		foreach (Canvas c in GetComponentsInChildren<Canvas>())
		{
			c.enabled = state;
		}

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.enabled = state;
		}
	}

	public void Reset()
	{
		m_currentHealth = m_maxHealth;

		SetActiveState(true);

		m_isDead = false;

	}

}
