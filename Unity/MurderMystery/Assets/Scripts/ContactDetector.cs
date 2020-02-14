using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDetector : MonoBehaviour
{

	public string input;
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		StoryStructure.Instance.Interact(input);
	}

}
