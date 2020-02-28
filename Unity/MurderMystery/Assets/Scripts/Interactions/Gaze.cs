using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour
{
	private BoxCollider collider;
	public float distance;


	private void Awake()
	{
		collider = GetComponent<BoxCollider>();
		collider.size = collider.size + (Vector3.forward * distance);
        //collider.center = Vector3.forward * (distance / 2);
        collider.center = Vector3.forward * (distance / 2);
	}

	private void Update()
	{
		Debug.DrawRay(transform.position, transform.forward * distance, Color.red);
	}


}
