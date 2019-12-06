using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTracker : ITracker
{

    Vector3 initialPosition;
    Quaternion initialRotation;
    Quaternion firsTrackedRotation;
    private Vector3 firstTrackedPosition;


    public GameObject realWorldReference;

    bool attached;
    public bool reattach;


    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public Quaternion InitialRotation { get => initialRotation; set => initialRotation = value; }

    public Quaternion FirstTrackedRotation { get => firsTrackedRotation; set => firsTrackedRotation = value; }
    public Vector3 FirstTrackedPosition { get => firstTrackedPosition; set => firstTrackedPosition = value; }


    private GameObject master;
    private MasterController masterController;
    private TargetedController targetedController;
    private Logic logic;

    public override void attach()
    {/*
        if (VirtualObject != null)
        {
            if (masterController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING || masterController.currentStage == MasterController.EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING)
            {
                InitialPosition = realWorldReference.transform.position; //+ trackerOffset;
                InitialRotation = Quaternion.identity;
            }
            else
            {
                InitialPosition = VirtualObject.virtualObject.transform.position; //+ trackerOffset;
                InitialRotation = VirtualObject.virtualObject.transform.rotation;
            }
            
            FirstTrackedRotation = gameObject.transform.rotation;
            firstTrackedPosition = gameObject.transform.position;
            attached = true;
        }
     */
    } 
    public override void detach()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

        attached = false;
        master = GameObject.Find("Master");
        targetedController = master.GetComponent<TargetedController>();
        masterController = master.GetComponent<MasterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PositionReference != null && attached)
        {
            Vector3 realPos = transform.position - firstTrackedPosition + initialPosition;
            Vector3 retargPos = targetedController.giveRetargetedPosition(realPos);
            PositionReference.transform.position = retargPos;
            Quaternion rot = transform.rotation * initialRotation * Quaternion.Inverse(FirstTrackedRotation);
        }
            
        if(reattach)
        {
            reattach = false;
            attach();
        }
    }
}
