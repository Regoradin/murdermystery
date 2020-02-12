using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour
{
    float y;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        y += Time.deltaTime * 10;
        transform.rotation = Quaternion.Euler(0, y, 0);
       
        
    }
}
