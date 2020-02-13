using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour
{
    float y;
    float scalevalue;
    // Start is called before the first frame update
    void Start()
    {
        scalevalue = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        y += Time.deltaTime * 10;
        transform.rotation = Quaternion.Euler(0, y, 0);

        if(OVRInput.Get(OVRInput.RawButton.X) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.localScale += new Vector3(scalevalue,scalevalue, scalevalue);
        }
        else if (OVRInput.Get(OVRInput.RawButton.Y) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.localScale -= new Vector3(scalevalue, scalevalue, scalevalue);
        }


    }
}
