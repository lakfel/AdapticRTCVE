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
        objectInHand = false;
        //TODO delete
        allowToGrab = true;
        //trackerMannager.setTrackers();

    }

    // Update is called once per frame
    void Update()
    {

            
    }

    public IEnumerator setHand(bool handActive, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hand.setDraw(handActive);
        yield return null;
    }

    [PunRPC]
    public void process(float segs)
    {
        if (possibleObject != null && allowToGrab && !objectInHand)
        {
            voObject = possibleObject;
            possibleObject = null;
            StartCoroutine(setHand(false, segs));
           // hand.setDraw(false);
            objectInHand = true;
        }
        else if (voObject != null)
        {
            trackerMannager.fLeftTracker.detach();
            trackerMannager.fRightTracker.detach();
            possibleObject = voObject;
            voObject = null;
            //hand.setDraw(true);

            StartCoroutine(setHand(true, segs));
            objectInHand = false;
            PropSpecs propSpecs = possibleObject.GetComponent<PropSpecs>();
            propSpecs.objectGrabbed(false);
            propSpecs.objectGreen(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(allowToGrab && !objectInHand)
        { 
            GameObject possibleObjectT = other.gameObject;
            PropSpecs propSpecs = possibleObjectT.GetComponent<PropSpecs>();
            if(propSpecs != null && !propSpecs.grabbed)
            { 
                propSpecs.objectGrabbed(true);
                propSpecs.objectGreen(true);
                possibleObject = possibleObjectT;
            }      
        }
    }
    private void OnTriggerExit(Collider other)
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
