using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


[RequireComponent(typeof(Rigidbody))]
public class GazeDetectorMontage : MonoBehaviour
{

	public string interaction;
	public float gazeHeldTime;
    public VideoPlayer montagePlayer;
    public ParticleSystem montageFX;

    private float gazeHeldTimeRemaining;
	private bool gazed = false;


	private void Awake()
	{
		gazeHeldTimeRemaining = gazeHeldTime;
        montagePlayer.Pause();
        
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

            //Stop the montageplayer
            montagePlayer.Pause();
            Debug.Log("Paused video");
        }
	}
	
	private void ActivateGaze()
	{
		if (gazed == false)
		{
			Debug.Log("Playing Video");
			gazed = true;
            //StoryStructure.Instance.Interact(interaction);
            //Start the montageplayer
            montagePlayer.Play();
            montageFX.Stop();
            

		}
	}

}
