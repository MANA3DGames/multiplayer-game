using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

	public Rigidbody m_rigidbody;

	public Transform m_chassis;

	public Transform m_turret;

	public float m_moveSpeed = 100f;

	public float m_chassisRotateSpeed = 1f;
	public float m_turretRotateSpeed = 3f;


	void Start () 
	{
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void MovePlayer(Vector3 dir)
	{
		Vector3 moveDirection = dir * m_moveSpeed * Time.deltaTime;
		m_rigidbody.velocity = moveDirection;

	}

	public void FaceDirection(Transform xform, Vector3 dir, float rotSpeed)
	{
		if (dir != Vector3.zero && xform != null)
		{
			Quaternion desiredRot = Quaternion.LookRotation(dir);
			xform.rotation = Quaternion.Slerp(xform.rotation, desiredRot, rotSpeed * Time.deltaTime);
		}
	}

	public void RotateChassis(Vector3 dir)
	{
		FaceDirection(m_chassis, dir, m_chassisRotateSpeed);
	}

	public void RotateTurret(Vector3 dir)
	{
		FaceDirection(m_turret, dir, m_turretRotateSpeed);
	}


}
