using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpecs : MonoBehaviour, IPunObservable
{
    public enum SIDE { LEFT, RIGHT, UNDEFINED }

    public Vector3 trackerOffset;
    public GameObject voCap;

    public SIDE currentSide;
    
    public PropMannager.PRESET_TYPE type;

    // This game object contains a children with the mesh of the object
    public GameObject objctPar;
    public GameObject objct;

    //The ghost object contains a children ith a transparent mesh of the object
    public GameObject ghost;

    public bool grabbed;

    private void Update()
    {
        //Debug.Log(transform.rotation);
    }
    private void Start()
    {
        currentSide = SIDE.UNDEFINED;
        grabbed = false;
        resetProp(false);
    }

    [PunRPC]
    public void objectGrabbedRPC(bool isGrabbed)
    {
        grabbed = isGrabbed;
    }


    public void objectGrabbed(bool isGreen)
    {
        GetComponent<PhotonView>().RPC("objectGrabbedRPC", PhotonTargets.All, isGreen);
    }


    [PunRPC]
    public void objectGreenRPC(bool isGreen)
    {
        Renderer rend =objct.GetComponent<Renderer>();
        if (isGreen)
            rend.material.color = Color.green;
        else
            rend.material.color = Color.white;
    }


    


    public void objectGreen(bool isGreen)
    {
        GetComponent<PhotonView>().RPC("objectGreenRPC", PhotonTargets.All, isGreen);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }

    public void activeChildren(bool activate)
    {
        objct.SetActive(activate);
    }

    public float distanceToDock()
    {
        float distance = 0f;
        distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(ghost.transform.position.x, ghost.transform.position.z));
        //distance = Vector3.Distance(transform.position, ghost.transform.position);
        return distance;
    }

    public float distanceToInitialPoint()
    {
        float distance = 0f;
        //distance = Vector3.Distance(transform.position, virtualObject.transform.position);
        return distance;
    }
    public void resetProp(bool isHome)
    {
        voCap.transform.localPosition = isHome? trackerOffset: -trackerOffset;
        objctPar.transform.localPosition = isHome ? -trackerOffset : trackerOffset;
    }


}
