using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;

public class Tracker : MonoBehaviour, IPunObservable
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
    public GameObject virtualObject;
    public GameObject VirtualObject { get => virtualObject; set => virtualObject = value; }

    //This is the reference of the retargeted goal point. It use to be the middle point betwen left and right prop
    // However, it can be used to retarget to any position
    public GameObject realWorldReference;

    // This represents the initial position that the object tracked has in real world
    // This is either VirtualObject (NOT RETARGETING) or realWorldReference (RETARGETING APPLIED)
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

    //Where the tracker should start to track.
    public GameObject initialReference;

    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public Quaternion InitialRotation { get => initialRotation; set => initialRotation = value; }
    public Quaternion FirstTrackedRotation { get => firsTrackedRotation; set => firsTrackedRotation = value; }
    public Vector3 FirstTrackedPosition { get => firstTrackedPosition; set => firstTrackedPosition = value; }

    public GameObject master;
    private MasterController masterController;
    private TargetedController targetedController;
    private Logic logic;
    public float yOffset;
    // This object changes the position and orientation in function of the tracker movements in real world
    public GameObject trackerRep;

     
    public GameObject objectTracked;

    // Swipe recognition
    private Vector2 swipeStart, swipeEnd, swipe;
    private float startTime;
    public bool swiping;
    public float minSwipeDist = 0.2f;
    public float minVelocity = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<PhotonView>().isMine)
            this.enabled = false;
        atached = false;
        targetedController = master.GetComponent<TargetedController>();
        masterController = master.GetComponent<MasterController>();
        realWorldReference = GameObject.Find("MiddlePropPosition");
        reatach = false;
        objectTracked.transform.localPosition = trackedOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<PhotonView>().isMine)
        { 
            if (VirtualObject != null && atached)
            {
                if ((ViveInput.GetPressEx(HandRole.RightHand, ControllerButton.PadTouch) && ViveInput.GetPadTouchDeltaEx(HandRole.RightHand).y > 0.2f) ||
                    (ViveInput.GetPressEx(HandRole.LeftHand, ControllerButton.PadTouch) && ViveInput.GetPadTouchDeltaEx(HandRole.LeftHand).y > 0.2f))
                    yOffset += 0.02f;

                if ((ViveInput.GetPressEx(HandRole.RightHand, ControllerButton.PadTouch) && ViveInput.GetPadTouchDeltaEx(HandRole.RightHand).y < -0.2f) ||
                    (ViveInput.GetPressEx(HandRole.LeftHand, ControllerButton.PadTouch) && ViveInput.GetPadTouchDeltaEx(HandRole.LeftHand).y < -0.2f))
                    yOffset -= 0.02f;

                Vector3 realPos = Vector3.zero; 
                realPos = trackerRep.transform.position - firstTrackedPosition + initialPosition + new Vector3(0f,yOffset,0f);

                transform.position = targetedController.giveRetargetedPosition(realPos);

               


                transform.rotation = trackerRep.transform.rotation* Quaternion.Inverse(FirstTrackedRotation) * InitialRotation  ;
                refreshPosition(objectTracked.transform.position, objectTracked.transform.rotation);
            }
            if (reatach)
            {
                reatach = false;
                restartTrackerRef();
            }
        }
    }


    [PunRPC]
    public void refreshPosition(Vector3 pos, Quaternion rota)
    {
        VirtualObject.transform.position = pos;
        VirtualObject.transform.rotation = rota;

    }

    public void restartTrackerRef()
    {

        FirstTrackedRotation = trackerRep.transform.rotation;
        firstTrackedPosition = trackerRep.transform.position;
    }

    public  void attach()
    {
        bool att = false;
        if (VirtualObject != null)
        {
            /*if (masterController.condition == MasterController.CONDITION.NM_RT || masterController.condition == MasterController.CONDITION.SM_RT)
            {
                InitialPosition = realWorldReference.transform.position;
                InitialRotation = VirtualObject.transform.rotation;
                att = true;
            }
            else
            {*/
            InitialPosition = VirtualObject.transform.position;
            InitialRotation = VirtualObject.transform.rotation;
            Debug.Log("Attaching -- Orientation " + InitialRotation);
            //}
            yOffset = 0f;
            restartTrackerRef();
            atached = true;
        }
    }


    public void detach()
    {
        atached = false;
        VirtualObject = null;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
