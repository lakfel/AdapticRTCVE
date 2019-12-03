using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMannager : MonoBehaviour
{
    public enum TRCKER_SIDE { LEFT, RIGHT};

    public Tracker leftTracker;
    public Tracker rightTracker;

    public PropController leftProp;
    public PropController rightProp;

    private MasterController masterController;
    // Start is called before the first frame update
    void Start()
    {
        masterController = gameObject.GetComponent<MasterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // Pair tracker with propcontroller. If RT condition is applied, both cirutl objects will be attached to the same
    // tracker. Left one here.
    public void setTrackers()
    {/*
        masterController = gameObject.GetComponent<MasterController>();
        if (masterController.currentCondition == MasterController.CONDITION.NM_RT ||
                masterController.currentCondition == MasterController.CONDITION.SM_RT)
        {
            leftProp.dTracker = leftTracker;
            rightProp.dTracker = leftTracker;
        }
        else
        {
            leftProp.dTracker = leftTracker;
            rightProp.dTracker = rightTracker;
        }*/
    }
}
