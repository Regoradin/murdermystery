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


    public List<Animator> interruptingAnimators;

    private void Start()
    {
        gaze = (TakeResponse)FindObjectOfType(typeof(TakeResponse));
        cooldownRemaining = cooldown;
        VRGestureRecognizer.Current.NodHandler += OnNod;
        VRGestureRecognizer.Current.HeadshakeHandler += OnHeadshake;
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
        if (!coolingDown && gaze.isGazing() && IsAnimatorsAtBase())
        {
            Debug.Log("Nodding detector");
            StoryStructure.Instance.Interact(yesInteraction);
            coolingDown = true;
            cooldownRemaining = cooldown;
        }
    }
    private void OnHeadshake()
    {
        if (!coolingDown && gaze.isGazing() && IsAnimatorsAtBase())
        {
            Debug.Log("Headshake detector");
            StoryStructure.Instance.Interact(noInteraction);
            coolingDown = true;
            cooldownRemaining = cooldown;
        }
    }

    private bool IsAnimatorsAtBase()
    {
        foreach (Animator anim in interruptingAnimators)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Base"))
            {
                return false;
            }
        }
        return true;
    }


}
