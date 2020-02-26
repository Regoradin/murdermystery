using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSlammer : MonoBehaviour
{
	[SerializeField]
	private OVRInput.Controller controller;

	public float minimumVelocity;
	public string interactionName;

	private Vector3 velocity;

	private void OnTriggerEnter(Collider other)
	{
		Table table = other.GetComponent<Table>();
		if (table)
		{
			float velocity = OVRInput.GetLocalControllerVelocity(controller).magnitude;
			if (velocity >= minimumVelocity)
			{
				table.Slam(OVRInput.GetLocalControllerVelocity(controller).magnitude);
				StoryStructure.Instance.Interact(interactionName);
			}
		}
	}
}
