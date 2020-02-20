using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeDetecter : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
         StoryStructure.Instance.Interact(collision.collider.tag);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
