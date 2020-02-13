using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSlammer : MonoBehaviour
{
	[SerializeField]
	private OVRInput.Controller controller;

	private Vector3 velocity;

	private void OnTriggerEnter(Collider other)
	{
		Table table = other.GetComponent<Table>();
		if (table)
		{
			table.Slam(OVRInput.GetLocalControllerVelocity(controller).magnitude);
		}
	}
}
