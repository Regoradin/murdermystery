using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sitHand : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform anchor;
    void Start()
    {
        this.GetComponent<Transform>().position = anchor.position;
        this.GetComponent<Transform>().rotation = anchor.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Transform>().position = anchor.position;
        this.GetComponent<Transform>().rotation = anchor.rotation;
    }
}
