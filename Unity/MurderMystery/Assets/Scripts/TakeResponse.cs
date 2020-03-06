using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TakeResponse : MonoBehaviour
{
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
                gazed = true;
                Debug.Log("Has gazed at the judge for " + gazeHeldTime + " seconds");
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

    public bool isGazing()
    {
        return gazed;
    }
}
