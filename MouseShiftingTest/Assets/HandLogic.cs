using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class HandLogic : MonoBehaviour
{

    public IGenericHand hand;

    public bool allowToGrab;

    public bool objectInHand;

    public GameObject possibleObject;

    public GameObject voObject;

    public GameObject parentVOObjects;

    public TrackerMannager trackerMannager;

    public MasterController masterController;
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<PhotonView>().isMine)
            this.enabled = false;
        hand = gameObject.GetComponent<IGenericHand>();
        allowToGrab = false;
        objectInHand = true;
        //TODO delete
        allowToGrab = true;
        trackerMannager.setTrackers();

    }

    // Update is called once per frame
    void Update()
    {

            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)
                || ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
            {
                process();
            }
    }
    [PunRPC]
    public void process()
    {
        if (possibleObject != null && allowToGrab)
        {
            voObject = possibleObject;
            possibleObject = null;
            hand.setDraw(false);
            PropSpecs propSpecs = voObject.GetComponent<PropSpecs>();
            if (propSpecs.currentSide == PropSpecs.SIDE.LEFT)
            {
                trackerMannager.fLeftTracker.VirtualObject = voObject;
                trackerMannager.fLeftTracker.attach();
            }
            else
            {
                trackerMannager.fRightTracker.VirtualObject = voObject;
                trackerMannager.fRightTracker.attach();
            }
            if(voObject.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
            {
                voObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
            }
            objectInHand = true;
        }
        else if (voObject != null)
        {
             trackerMannager.fLeftTracker.detach();
             trackerMannager.fRightTracker.detach();
            possibleObject = voObject;
             voObject = null;
             hand.setDraw(true);
            objectInHand = false;
        }
    }
    

    [PunRPC]
    public void pairController( )
    {
        PropSpecs propSpecs = voObject.GetComponent<PropSpecs>();
        if (propSpecs.currentSide == PropSpecs.SIDE.LEFT)
        {
            trackerMannager.fLeftTracker.VirtualObject = voObject;
            trackerMannager.fLeftTracker.attach();
        }
        else
        {
            trackerMannager.fRightTracker.VirtualObject = voObject;
            trackerMannager.fRightTracker.attach();
        }
    }

    [PunRPC]
    public void detachController()
    {
        trackerMannager.fLeftTracker.detach();
        trackerMannager.fRightTracker.detach();
        voObject = null;
        hand.setDraw(true);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (allowToGrab)
        { 
            GameObject possibleObjectT = other.gameObject;
            PropSpecs propSpecs = possibleObjectT.GetComponent<PropSpecs>();
            if(!propSpecs.grabbed)
            { 
                propSpecs.objectGrabbed(true);
                propSpecs.objectGreen(true);
                possibleObject = possibleObjectT;
            }      
        }
    }
    private void OnTriggerExit(Collider other)
    {
            if (allowToGrab)
            {
                if (possibleObject != null)
                {
                    PropSpecs propSpecs = possibleObject.GetComponent<PropSpecs>();
                    propSpecs.objectGreen(false);
                    propSpecs.objectGrabbed(false);
                    possibleObject = null;
                }
           
            }
    }
    



}
