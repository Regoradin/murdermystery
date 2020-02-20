using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GazeDetector : MonoBehaviour
{

	public string interaction;
	public float gazeHeldTime;
	private float gazeHeldTimeRemaining;
	private bool gazed = false;


	private void Awake()
	{
		gazeHeldTimeRemaining = gazeHeldTime;
	}
	private void OnTriggerEnter(Collider other)
	{
		Gaze gaze = other.GetComponent<Gaze>();
		if (gaze != null)
		{

		}
	}

	private void OnTriggerStay(Collider other)
	{
		Gaze gaze = other.GetComponent<Gaze>();
		if (gaze != null)
		{
			gazeHeldTimeRemaining -= Time.deltaTime;

			if (gazeHeldTimeRemaining <= 0)
			{
				ActivateGaze();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Gaze gaze = other.GetComponent<Gaze>();
		if (gaze != null)
		{
			gazeHeldTimeRemaining = gazeHeldTime;
			gazed = false;
		}
	}
	
	private void ActivateGaze()
	{
		if (gazed == false)
		{
			Debug.Log("GAZING");
			gazed = true;
			StoryStructure.Instance.Interact(interaction);
		}
	}

}
