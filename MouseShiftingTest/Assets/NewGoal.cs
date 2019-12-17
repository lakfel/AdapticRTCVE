using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGoal : MonoBehaviour
{
    public bool handOnInitialPosition;
 
    // Start is called before the first frame update
    void Start()
    {
        //master = GameObject.Find("Master");
        //logic = master.GetComponent<Logic>();
    }
    
    private void OnTriggerEnter1(Collider other)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        Collider spehre = gameObject.GetComponent<SphereCollider>();
        rend.material.color = Color.green;
        handOnInitialPosition = true;

    }

    private void OnTriggerExit1(Collider other)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        Collider spehre = gameObject.GetComponent<SphereCollider>();
        rend.material.color = Color.white;
        handOnInitialPosition = false;
    }
    /*
    private void OnBecameInvisible()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.white;
        masterController.handOnInitialPosition = false;
    }

    private void OnBecameVisible()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.white;
        masterController.handOnInitialPosition = false;
    }*/

   [PunRPC]
   public void spehereColor(Color color)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = color;
    }

    void Update()
    {
        Collider spehre = gameObject.GetComponent<SphereCollider>();
        if (Physics.OverlapBox(spehre.transform.position, new Vector3(0.05f, 0.05f, 0.05f)).Length == 1)
        {
            GetComponent<PhotonView>().RPC("spehereColor", PhotonTargets.All, Color.white);
            //rend.material.color = Color.white;
            handOnInitialPosition = false;
        }
        else
        {
            GetComponent<PhotonView>().RPC("spehereColor", PhotonTargets.All, Color.green);
            //rend.material.color = Color.green   ;
            handOnInitialPosition = true;
        }
    }

   
}
