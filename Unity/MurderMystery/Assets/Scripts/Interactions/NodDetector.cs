using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameSynthesis.VR;

public class NodDetector : MonoBehaviour
{
    public string yesInteraction;
    public string noInteraction;

    public float cooldown;
    private float cooldownRemaining;
    private bool coolingDown = false;
    private TakeResponse gaze;
	public bool isListening;


    private void Start()
    {
        gaze = (TakeResponse)FindObjectOfType(typeof(TakeResponse));
        cooldownRemaining = cooldown;
        VRGestureRecognizer.Current.NodHandler += OnNod;
        VRGestureRecognizer.Current.HeadshakeHandler += OnHeadshake;

		isListening = true;
    }

    private void Update()
    {
        if (cooldownRemaining <= 0)
        {
            coolingDown = false;
        }
        else
        {
            cooldownRemaining -= Time.deltaTime;
        }
    }

    private void OnNod()
    {
		Debug.Log("Nodding, cooling: " + coolingDown + " gazing: " + gaze.isGazing() + " listening: " + isListening);
		if (!coolingDown && gaze.isGazing() && isListening)
		if (!coolingDown && isListening)
		{
            Debug.Log("Sending nod");
            StoryStructure.Instance.Interact(yesInteraction);
            coolingDown = true;
            cooldownRemaining = cooldown;
        }
    }
    private void OnHeadshake()
	{
		Debug.Log("Headshaking, cooling: " + coolingDown + " gazing: " + gaze.isGazing() + " listening: " + isListening);
		if (!coolingDown && gaze.isGazing() && isListening)
		if (!coolingDown && isListening)
		{
            Debug.Log("Sending headshake");
            StoryStructure.Instance.Interact(noInteraction);
            coolingDown = true;
            cooldownRemaining = cooldown;
        }
    }

	public void SetListeningFalse()
	{
		Debug.Log("listening false");
		isListening = false;
	}
	public void SetListeningTrue()
	{
		Debug.Log("listening true");
		isListening = true;
	}

}
