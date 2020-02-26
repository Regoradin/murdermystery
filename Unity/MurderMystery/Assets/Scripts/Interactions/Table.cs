using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
	public float strength;

	private HashSet<Rigidbody> rbs;
	public void Slam(float speed)
	{
		foreach (Rigidbody rb in rbs)
		{
			rb.AddForce(Vector3.up * speed * strength);
		}
	}

	private void Awake()
	{
		rbs = new HashSet<Rigidbody>();
	}
	public void OnTriggerEnter(Collider other)
	{
		//if (other.gameObject != gameObject)
		//{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if (rb)
			{
				rbs.Add(rb);
			}
		//}
	}

	private void OnTriggerExit(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if (rbs.Contains(rb))
		{
			rbs.Remove(rb);
		}
	}
}
