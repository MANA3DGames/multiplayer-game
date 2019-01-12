using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerSetup))]
public class PlayerController : NetworkBehaviour {

	PlayerHealth m_pHealth;
	PlayerMotor m_pMotor;
	PlayerSetup m_pSetup;
	PlayerShoot m_pShoot;




	void Start () 
	{
		m_pHealth = GetComponent<PlayerHealth>();
		m_pMotor = GetComponent<PlayerMotor>();
		m_pSetup = GetComponent<PlayerSetup>();
		m_pShoot = GetComponent<PlayerShoot>();
	}

	Vector3 GetInput()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		return new Vector3 (h, 0, v);

	}

	void FixedUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		Vector3 inputDirection = GetInput();
		m_pMotor.MovePlayer(inputDirection);
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			m_pShoot.Shoot();
		}

		Vector3 inputDirection = GetInput();
		if (inputDirection.sqrMagnitude > 0.25f)
		{
			m_pMotor.RotateChassis(inputDirection);
		}

		Vector3 turretDir = Utility.GetWorldPointFromScreenPoint(Input.mousePosition, m_pMotor.m_turret.position.y) - m_pMotor.m_turret.position;
		m_pMotor.RotateTurret(turretDir);

	}



}






