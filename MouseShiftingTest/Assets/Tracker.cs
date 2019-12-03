using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    // Trackers are palcer away to the center of the real object
    // This variable represent that offset and helps to compensate
    public Vector3 trackedOffset;

    // This bool that is public and editable in runtime, allowsmthe user to "reboot" the pair of the object
    // to the tracker
    public bool reatach;

    // Says either the tracker is or not attached to an object in VR
    public bool atached;

    // This is the position reference of the object must be in VR.
    // When retargeting is not applied, this must match with the objetc in real world either in the left or in the right
    // When reatergetin is applied, this is left or right. In real world the object is placed in the RealWorldReference position
    private PropController positionReference;
    public PropController PositionReference { get => positionReference; set => positionReference = value; }

    //This is the reference of the retargeted goal point. It use to be the middle point betwen left and right prop
    // However, it can be used to retarget to any position
    public GameObject realWorldReference;

    // This represents the initial position that the object tracked has in real world
    // This is either PositionReference (NOT RETARGETING) or realWorldReference (RETARGETING APPLIED)
    Vector3 initialPosition;

    // This represents the initial rotation of the object tracked in real world
    Quaternion initialRotation;

    // This is the first tracked position information.
    // The tracker system may have different coordinate system than the scenario one.
    // So we want in fact is the difference of positions tracked. We substrack ths value to evey measure
    // and add the initial position so our zero point is the initial position
    private Vector3 firstTrackedPosition;

    //First tracked rotation of the tracker object.
    Quaternion firsTrackedRotation;

    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public Quaternion InitialRotation { get => initialRotation; set => initialRotation = value; }
    public Quaternion FirstTrackedRotation { get => firsTrackedRotation; set => firsTrackedRotation = value; }
    public Vector3 FirstTrackedPosition { get => firstTrackedPosition; set => firstTrackedPosition = value; }

    private GameObject master;
    private MasterController masterController;
    private TargetedController targetedController;
    private Logic logic;

    // This object changes the position and orientation in function of the tracker movements in real world
    public GameObject trackerRep;

    // 
    public GameObject objectTracked;

    // Start is called before the first frame update
    void Start()
    {
        atached = false;
        master = GameObject.Find("Master");
        targetedController = master.GetComponent<TargetedController>();
        masterController = master.GetComponent<MasterController>();
        reatach = false;
        objectTracked.transform.localPosition = trackedOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (PositionReference != null && atached)
        {
            
            Vector3 realPos = Vector3.zero;
            realPos = trackerRep.transform.position - firstTrackedPosition + initialPosition;

            transform.position = targetedController.giveRetargetedPosition(realPos);
            transform.rotation = trackerRep.transform.rotation * InitialRotation * Quaternion.Inverse(FirstTrackedRotation);
            
            PositionReference.RealPosition = realPos + trackedOffset;
            PositionReference.transform.position = objectTracked.transform.position;
            PositionReference.transform.rotation = objectTracked.transform.rotation;
        }
        if (reatach)
        {
            reatach = false;
            attach();
        }
    }

    public  void attach()
    {
       /*ool att = false;
        if (PositionReference != null)
        {
            PositionReference.dTracker = this;
            if (masterController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING || masterController.currentStage == MasterController.EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING)
            {
                InitialPosition = realWorldReference.transform.position; 
                InitialRotation = Quaternion.identity;
                att = true;
            }
            if (!att)
            {
                InitialPosition = PositionReference.positionReference.transform.position;
                InitialRotation = PositionReference.positionReference.transform.rotation;
            }
            
            FirstTrackedRotation = objectTracked.transform.rotation;
            firstTrackedPosition = objectTracked.transform.position;
            atached = true;
        }*/
    }


    public void detach()
    {
        PositionReference = null;
        InitialPosition = Vector3.zero;
        firstTrackedPosition = Vector3.zero;
        InitialRotation = Quaternion.identity;
        atached = false;
    }

}
