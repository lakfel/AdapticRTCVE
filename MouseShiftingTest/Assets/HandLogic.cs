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

        if (GetComponent<PhotonView>().isMine)
        {
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)
                || ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
            {
                GetComponent<PhotonView>().RPC("process", PhotonTargets.All);
                /*if (possibleObject != null && allowToGrab)
                {
                    voObject = possibleObject;
                    possibleObject = null;
                    hand.setDraw(false);
                    /*PropSpecs propSpecs = voObject.GetComponent<PropSpecs>();
                    if (propSpecs.currentSide == PropSpecs.SIDE.LEFT)
                    {
                        trackerMannager.fLeftTracker.VirtualObject = voObject;
                        trackerMannager.fLeftTracker.attach();
                    }
                    else
                    {
                        trackerMannager.fRightTracker.VirtualObject = voObject;
                        trackerMannager.fRightTracker.attach();
                    }*/

                /*     GetComponent<PhotonView>().RPC("pairController", PhotonTargets.All,);
                 }
                 else if (voObject != null)
                 {
                     /* trackerMannager.fLeftTracker.detach();
                      trackerMannager.fRightTracker.detach();
                      voObject = null;
                      hand.setDraw(true);*/
                /*   GetComponent<PhotonView>().RPC("detachController", PhotonTargets.All);
               }*/
            }
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

            
        }
        else if (voObject != null)
        {
             trackerMannager.fLeftTracker.detach();
             trackerMannager.fRightTracker.detach();
            possibleObject = voObject;
             voObject = null;
             hand.setDraw(true);
           
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

        //if (GetComponent<PhotonView>().isMine)
        //{
        Debug.Log("Trigger entered");
            if (allowToGrab)
            {
                
                GameObject possibleObjectT = other.gameObject;
                PropSpecs propSpecs = possibleObjectT.GetComponent<PropSpecs>();
                if(!propSpecs.grabbed)
                { 
                    propSpecs.objectGrabbedRPC(true);
                    propSpecs.objectGreenRPC(true);
                    possibleObject = possibleObjectT;
                   // PhotonView photonView = propSpecs.gameObject.GetPhotonView();
                   // photonView.TransferOwnership(PhotonNetwork.player.ID);
                }
               
            }
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        //if (GetComponent<PhotonView>().isMine)
        //{
            if (allowToGrab)
            {
                if (possibleObject != null)
                {

                    PropSpecs propSpecs = possibleObject.GetComponent<PropSpecs>();
                    propSpecs.objectGreen(false);
                    propSpecs.objectGrabbed(false);
                    possibleObject = null;
                }
               /* if (voObject != null)
                {

                    PropSpecs propSpecs = voObject.GetComponent<PropSpecs>();
                    propSpecs.objectGreen(false);
                    propSpecs.objectGrabbed(false);

                    //voObject = null;
                }*/

            }
        //}
    }
    



}
