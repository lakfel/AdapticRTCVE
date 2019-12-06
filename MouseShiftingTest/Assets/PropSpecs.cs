using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpecs : MonoBehaviour, IPunObservable
{
    public enum SIDE { LEFT, RIGHT, UNDEFINED }

    public SIDE currentSide;
    
    public PropMannager.PRESET_TYPE type;

    public GameObject objct;

    public GameObject ghost;

    public bool grabbed;

    private void Start()
    {
        currentSide = SIDE.UNDEFINED;
        grabbed = false;
    }

    [PunRPC]
    public void objectGrabbedRPC(bool isGrabbed)
    {
        grabbed = isGrabbed;
    }


    public void objectGrabbed(bool isGreen)
    {
        GetComponent<PhotonView>().RPC("objectGreenRPC", PhotonTargets.All, isGreen);
    }


    [PunRPC]
    public void objectGreenRPC(bool isGreen)
    {
        Renderer rend = gameObject.transform.GetChild(0).GetComponent<Renderer>();
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
}
