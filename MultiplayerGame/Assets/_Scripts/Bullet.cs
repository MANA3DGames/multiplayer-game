using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : NetworkBehaviour 
{
	Rigidbody m_rigidbody;

	Collider m_collider;

	public int m_speed = 100;

	List<ParticleSystem> m_allParticles;

	public float m_lifetime = 5f;

	public ParticleSystem m_explosionFX;

	public List<string> m_bounceTags;

	public int m_bounces = 2;


	// Use this for initialization
	void Start () 
	{
		m_allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
		m_rigidbody = GetComponent<Rigidbody>();
		m_collider = GetComponent<Collider>();
		StartCoroutine("SelfDestruct");
	}

	IEnumerator SelfDestruct()
	{
		yield return new WaitForSeconds(m_lifetime);

		Explode ();

	}

	void Explode ()
	{
		m_collider.enabled = false;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.Sleep ();

		foreach (ParticleSystem ps in m_allParticles) 
		{
			ps.Stop ();
		}

		if (m_explosionFX != null) 
		{
			m_explosionFX.transform.parent = null;
			m_explosionFX.Play ();
		}

		if (isServer)
		{
			Destroy (gameObject);
			foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer> ()) 
			{
				m.enabled = false;
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionExit(Collision collision)
	{
		if (m_rigidbody.velocity != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(m_rigidbody.velocity);
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (m_bounceTags.Contains(collision.gameObject.tag))
		{
			if (m_bounces <= 0)
			{
				Explode();
			}

			m_bounces --;
		}

	}





}
