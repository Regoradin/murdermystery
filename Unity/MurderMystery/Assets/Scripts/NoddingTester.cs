using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameSynthesis.VR; // https://github.com/korinVR/VRGestureRecognizer by korinVR

public class NoddingTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VRGestureRecognizer.Instance.NodHandler += OnNod;
        VRGestureRecognizer.Instance.HeadshakeHandler += OnHeadshake;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNod()
    {
        Debug.Log("nod");
        StoryStructure.Instance.Interact("yes");
    }

    void OnHeadshake()
    {
        Debug.Log("headshake");
        StoryStructure.Instance.Interact("no");
    }
}
