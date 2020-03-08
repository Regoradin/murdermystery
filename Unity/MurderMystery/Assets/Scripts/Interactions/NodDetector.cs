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


    public List<Animator> interruptingAnimators;

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
        {
            Debug.Log("Nodding detector");
            StoryStructure.Instance.Interact(yesInteraction);
            coolingDown = true;
            cooldownRemaining = cooldown;
        }
    }
    private void OnHeadshake()
    {
        if (!coolingDown && gaze.isGazing() && isListening)
        {
            Debug.Log("Headshake detector");
            StoryStructure.Instance.Interact(noInteraction);
            coolingDown = true;
            cooldownRemaining = cooldown;
        }
    }

}
