using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectTouchReaction : MonoBehaviour
{
    Material mat;
    bool highLightOn;
    bool isHighLighting;
    float currentLightPower;
    [SerializeField]
    float OuterRing;
    float time;

    private void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;
        currentLightPower = mat.GetFloat("_HighLightPower");
       
        
    }
    private void Update()
    {
        if (highLightOn)
        {
            isHighLighting = true;
        }
        if (isHighLighting)
        {
            currentLightPower = Mathf.Lerp(currentLightPower, OuterRing, .02f);
            mat.SetFloat("_HighLightPower", currentLightPower);
            if(currentLightPower > OuterRing - .002f)
            {
                isHighLighting = false;
            }
        }
        if(!highLightOn && !isHighLighting)
        {
            currentLightPower = Mathf.Lerp(currentLightPower, .1f, .02f);
            mat.SetFloat("_HighLightPower", currentLightPower);
            if(currentLightPower < .12f)
            {
                mat.SetInt("_HighLight", 0);
            }
        }
        Debug.Log(highLightOn);
        Debug.Log(isHighLighting);
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit");
        if (other.gameObject.tag == "MainCamera")
        {
            highLightOn = true;
            mat.SetInt("_HighLight", 1);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        Debug.Log("left");
        if(other.gameObject.tag == "MainCamera")
        {
            highLightOn = false;
        }
    }
}
