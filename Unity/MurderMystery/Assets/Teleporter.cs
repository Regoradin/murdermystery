using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;
    [SerializeField]
    private GameObject[] lights;
    private int currentIndex;
    private bool RightGrabButton;
    private bool triggerPressed;
    private float delay;
    [SerializeField]
    private float delayCounter;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RightGrabButton = OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.D);
        triggerPressed = OVRInput.GetDown(OVRInput.RawButton.B) || Input.GetKeyDown(KeyCode.A);
        if (RightGrabButton && currentIndex < positions.Length - 1)
        {
            lights[currentIndex].SetActive(false);
            currentIndex++;
            delay = delayCounter;
        }
        else if (triggerPressed && currentIndex > 0)
        {
            lights[currentIndex].SetActive(false);
            currentIndex--;
            delay = delayCounter;
        }

        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            lights[currentIndex].SetActive(true);
        }

        transform.position = positions[currentIndex].position;
        

    }
}
