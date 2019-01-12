using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	public Transform testObject;
	public float testHeight = 1f;


	public static Vector3 GetWorldPointFromScreenPoint(Vector3 screenPoint, float height)
	{
		
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);

		Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));

		float distance;

		if (plane.Raycast(ray, out distance))
		{
			return ray.GetPoint(distance);
		}
		return Vector3.zero;

	}

	void Update()
	{
		testObject.position = GetWorldPointFromScreenPoint(Input.mousePosition, testHeight);

	}


}
