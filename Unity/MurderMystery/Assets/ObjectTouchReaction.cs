﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//** THIS SCRIPT ONLY WORKS WITH THE HIGHLIGHT SHADER, DO NOT TRY THIS WITH OTHER SHADERS**
public class ObjectTouchReaction : MonoBehaviour
{
    Material mat;
    bool highLightOn;
    bool isHighLighting;
    float currentLightPower;
    [SerializeField]
    float OuterRing;
    [SerializeField]
    [ColorUsage(true, true)]
    Color highLightColor;
    float time;
    [SerializeField]
    string hitObjectTag;

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
        mat.SetColor("_EmmisionColor", highLightColor);
    }


    //Pre: Requires the object hitting to have a rigidbody/box collider(might need both)
    //Must have the same tag as the one declared as the object tag variable
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit");
        if (other.gameObject.tag == hitObjectTag)
        {
            highLightOn = true;
            mat.SetInt("_HighLight", 1);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        Debug.Log("left");
        if(other.gameObject.tag == hitObjectTag)
        {
            highLightOn = false;
        }
    }
}
