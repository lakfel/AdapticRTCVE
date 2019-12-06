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
                else if(voObject != null)
                {
                    trackerMannager.fLeftTracker.detach();
                    trackerMannager.fRightTracker.detach();
                    voObject = null;
                    hand.setDraw(true);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (GetComponent<PhotonView>().isMine)
        {
            if (allowToGrab)
            {

                GameObject possibleObjectT = other.gameObject;
                PropSpecs propSpecs = possibleObjectT.GetComponent<PropSpecs>();
                if(!propSpecs.grabbed)
                { 
                   propSpecs.objectGreen(true);
                   propSpecs.objectGrabbed(true);
                    possibleObject = possibleObjectT;
                }
               
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if (allowToGrab)
            {
                if (possibleObject != null)
                {

                    PropSpecs propSpecs = possibleObject.GetComponent<PropSpecs>();
                    propSpecs.objectGreen(false);
                    possibleObject = null;
                }
                if (voObject != null)
                {

                    PropSpecs propSpecs = voObject.GetComponent<PropSpecs>();
                    propSpecs.objectGreen(false);

                    //voObject = null;
                }

            }
        }
    }
    



}
