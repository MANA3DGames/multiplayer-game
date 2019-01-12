using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShoot : NetworkBehaviour {

	public Rigidbody m_bulletPrefab;

	public Transform m_bulletSpawn;

	public int m_shotsPerBurst = 2;

	int m_shotsLeft;

	bool m_isReloading;

	public float m_reloadTime = 1f;


	// Use this for initialization
	void Start () 
	{
		m_shotsLeft = m_shotsPerBurst;
		m_isReloading = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void Shoot()
	{
		if (m_isReloading || m_bulletPrefab == null)
		{
			return;
		}

		CmdShoot ();

		m_shotsLeft --;

		if (m_shotsLeft <= 0)
		{

			StartCoroutine("Reload");
		}
	}

	[Command]
	void CmdShoot ()
	{
		Bullet bullet = null;
		bullet = m_bulletPrefab.GetComponent<Bullet> ();
		Rigidbody rbody = Instantiate (m_bulletPrefab, m_bulletSpawn.position, m_bulletSpawn.rotation) as Rigidbody;
		if (rbody != null) {
			rbody.velocity = bullet.m_speed * m_bulletSpawn.transform.forward;
			NetworkServer.Spawn(rbody.gameObject);
		}
	}





	IEnumerator Reload()
	{
		m_shotsLeft = m_shotsPerBurst;
		m_isReloading = true;
		yield return new WaitForSeconds(m_reloadTime);
		m_isReloading = false;


	}




}
