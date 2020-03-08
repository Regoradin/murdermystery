using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontageVideoManager : MonoBehaviour
{
    public Transform spawnpoint;
    public Animator gazeAnim;

    public GameObject dazzleKnives;
    public GameObject BumpingIntoBunches;
    public GameObject jugglingKnives;
    public GameObject unicycle;
    public PlayOnlyOnce playOnlyOnce;

    public GameObject currentMontage;
    // Start is called before the first frame update
    void Start()
    {
        currentMontage = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //Spawn the montages


    public void spawnDazzleKnives()
    {
        currentMontage = (GameObject)Instantiate(dazzleKnives, spawnpoint);
        PlayOnlyOnce playOnceScript = currentMontage.GetComponentInChildren<PlayOnlyOnce>();
        playOnceScript.setMontageManager(this.gameObject);
        StopInteraction();
    }

    public void spawnBumpingIntoBunches()
    {
        currentMontage = (GameObject)Instantiate(BumpingIntoBunches, spawnpoint);
        PlayOnlyOnce playOnceScript = currentMontage.GetComponentInChildren<PlayOnlyOnce>();
        playOnceScript.setMontageManager(this.gameObject);
        StopInteraction();
    }

    public void spawnJugglingKnives()
    {
        currentMontage = (GameObject)Instantiate(jugglingKnives, spawnpoint);
        PlayOnlyOnce playOnceScript = currentMontage.GetComponentInChildren<PlayOnlyOnce>();
        playOnceScript.setMontageManager(this.gameObject);
        StopInteraction();
    }

    public void spawnUnicycle()
    {
        currentMontage = (GameObject)Instantiate(unicycle, spawnpoint);
        PlayOnlyOnce playOnceScript = currentMontage.GetComponentInChildren<PlayOnlyOnce>();
        playOnceScript.setMontageManager(this.gameObject);
        StopInteraction();
    }

    //Destroy the montage after it is done


    //Start or stop the interaction
    public void StopInteraction()
    {
        gazeAnim.SetTrigger("disable");
    }
    
    public void StartInteraction()
    {
        gazeAnim.SetTrigger("enable");
    }


}
