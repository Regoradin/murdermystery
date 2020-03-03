using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 spawnPosition;
    public Vector3 montageSpawnPosition;
    public GameObject currentSpawn;
    public GameObject currentMontage;

    public GameObject knifeBag;
    public GameObject knife;
    public GameObject knifeMontage;
    public GameObject jugglingMontage;
    
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
        currentSpawn.transform.position = spawnPosition;
        
    }

    //This is for montages
    void spawnKnifeMontage()
    {
        if (currentMontage != null)
        {
            Destroy(currentMontage);
        }
        currentMontage = Instantiate(knifeMontage);
        currentMontage.transform.position = montageSpawnPosition;

    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
