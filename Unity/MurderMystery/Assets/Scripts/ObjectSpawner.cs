using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform spawnPosition;
    public GameObject currentSpawn;


    public GameObject knifeBag;
    public GameObject knife;
    public GameObject peanut;
    public GameObject unicycle;

    
    void Start()
    {
        
    }

    //This is for objects
    void spawnKnifeBag()
    {
        if(currentSpawn != null)
        {
            Destroy(currentSpawn);
        }
        currentSpawn = Instantiate(knifeBag);
        currentSpawn.transform.position = spawnPosition.position;
        
    }
    void spawnKnife()
    {
        if (currentSpawn != null)
        {
            Destroy(currentSpawn);
        }
        currentSpawn = Instantiate(knife);
        currentSpawn.transform.position = spawnPosition.position;

    }
    void spawnPeanut()
    {
        if (currentSpawn != null)
        {
            Destroy(currentSpawn);
        }
        currentSpawn = Instantiate(peanut);
        currentSpawn.transform.position = spawnPosition.position;

    }
    void spawnunicycle()
    {
        if (currentSpawn != null)
        {
            Destroy(currentSpawn);
        }
        currentSpawn = Instantiate(unicycle);
        currentSpawn.transform.position = spawnPosition.position;

    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
