using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPosition : MonoBehaviour
{
    [SerializeField]
    Transform followTransform;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Transform>().position = followTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Transform>().position = followTransform.position;
        this.GetComponent<Transform>().rotation = followTransform.rotation;
    }
}
