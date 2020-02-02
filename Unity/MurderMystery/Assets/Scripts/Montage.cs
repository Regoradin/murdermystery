using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Montage : MonoBehaviour
{
    public Collider boundingCollider;
    private List<Renderer> renderers;
    
    void Start()
    {
        BuildRendererList();
        
        Disappear();
        if (!IsVisibleFromMainCam(boundingCollider)){
            AttractAttention();
        }
        StartCoroutine(AppearWhenOffScreen());
    }

    private IEnumerator AppearWhenOffScreen(){
        while (IsVisibleFromMainCam(boundingCollider)){
            yield return null;
        }
        Appear();
        while (!IsVisibleFromMainCam(boundingCollider))
        {
            yield return null;
        }
        TriggerAction();
    }

    private void AttractAttention(){
        //Something to activate sound/lights attract the players attention
        Debug.Log("Look at " + name + "!");
    }

    private void TriggerAction(){
        //Something to trigger whatever animations and sound we want to happen once the player sees it
        Debug.Log("You're looking at " + name);
    }

    private void BuildRendererList(){
        renderers = new List<Renderer>();
        foreach (Renderer rend in GetComponentsInChildren<Renderer>()){
            renderers.Add(rend);
        }

    }
    
    private void Disappear(){
        foreach(Renderer rend in renderers){
            rend.enabled = false;
        }
    }

    private void Appear(){
        foreach(Renderer rend in renderers){
            rend.enabled = true;
        }
    }
    
    public bool IsVisibleFromMainCam(Collider collider)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, collider.bounds);
    }

}
