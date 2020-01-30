using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Montage : MonoBehaviour
{
    private Renderer rend;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        Disappear();
        if (!IsVisibleFromMainCam(rend)){
            AttractAttention();
        }
        StartCoroutine(AppearWhenOffScreen());
    }

    void Update(){
        Debug.Log("Visible: " + IsVisibleFromMainCam(rend));
    }

    private IEnumerator AppearWhenOffScreen(){
        while (IsVisibleFromMainCam(rend)){
            yield return null;
        }
        Appear();
    }

    private void AttractAttention(){
        //Something to activate sound/lights will eventually go here
        Debug.Log("Look at " + name + "!");
    }

    private void Disappear(){
        rend.enabled = false;
    }

    private void Appear(){
        rend.enabled = true;
    }
    
    public bool IsVisibleFromMainCam(Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

}
