using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugtesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact(string interaction)
    {
        StoryStructure.Instance.Interact(interaction);
    }
}
