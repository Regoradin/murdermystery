using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameSynthesis.VR;

public class NoddingTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VRGestureRecognizer.Current.NodHandler += OnNod;
        VRGestureRecognizer.Current.HeadshakeHandler += OnHeadshake;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNod()
    {
        Debug.Log("nod");
    }

    void OnHeadshake()
    {
        Debug.Log("headshake");
    }
}
