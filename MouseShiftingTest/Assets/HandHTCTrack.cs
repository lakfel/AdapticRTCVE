using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHTCTrack : MonoBehaviour, IGenericHand
{
    // Object attached to the physical tracker. It's transform should chnge as the tracker does
    public GameObject tracker;

    // First tracked rotation
    public Quaternion firstTrackedRotation;


    // First tracked rotation
    public Quaternion initialRotation;

    // Flag to configure initial rotation.
    public bool configured;

    public TargetedController targeted;

    public GameObject handRepresentation;

    public Vector3 giveRealPosition()
    {
        Debug.Log("RealPosition - Object - " + transform.position);
        Debug.Log("RealPosition - Model - " + handRepresentation.transform.position);
        Debug.Log("RealPosition - Model Center - " + handRepresentation.transform.GetChild(0).transform.position);
        return handRepresentation.transform.GetChild(0).transform.position;
    }

    //public GameObject camera;

    [PunRPC]
    public void setDrawRPC(bool canDraw)
    {
        handRepresentation.SetActive(canDraw);
    }

    public void setDraw(bool canDraw)
    {
        GetComponent<PhotonView>().RPC("setDrawRPC", PhotonTargets.All, canDraw);
       
    }

    public bool canDraw()
    {
        return handRepresentation.activeSelf;
    }

    public void initialStatus()
    {
        firstTrackedRotation = tracker.transform.rotation;
        configured = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (targeted == null)
            targeted = gameObject.transform.parent.gameObject.GetComponent<TargetedController>();
        if (!GetComponent<PhotonView>().isMine)
        {
            tracker.SetActive(false);
        }
        initialRotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if (!configured)
            {
                initialStatus();
            }
            Vector3 offsetCenterHand = handRepresentation.transform.GetChild(0).transform.position - transform.position;
            transform.position = targeted.giveRetargetedPosition(tracker.transform.position+ offsetCenterHand) - offsetCenterHand;
            transform.rotation = tracker.transform.rotation * Quaternion.Inverse(firstTrackedRotation)* initialRotation;
        }
    }
}
