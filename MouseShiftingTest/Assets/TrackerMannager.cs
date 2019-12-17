using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMannager : MonoBehaviour
{
    /**
     * These are the trackers
     */
    public Tracker leftTracker;
    public Tracker rightTracker;


    /**
     * These are the trackers references.
     * Basically to use them as references when RT or OO condition works
     */
    public Tracker fLeftTracker;
    public Tracker fRightTracker;

    
    private MasterController masterController;

    public TrackerSystemsMannager trackerSystemsMannager;

    // Start is called before the first frame update
    void Start()
    {
        masterController = gameObject.GetComponent<MasterController>();
        if (!GetComponent<PhotonView>().isMine)
        {
            this.enabled = false;
            trackerSystemsMannager.gameObject.SetActive(false);
        }
       
            
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if(leftTracker == null)
            {
                if (trackerSystemsMannager.leftTracker != null)
                    leftTracker = trackerSystemsMannager.leftTracker;
                else
                    Debug.LogError("No tracker found");
            }
            if (rightTracker == null)
            {
                if (trackerSystemsMannager.rightTracker != null)
                    leftTracker = trackerSystemsMannager.rightTracker;
                else
                    Debug.LogError("No tracker found");
            }
        }
    }
    // Pair tracker with propcontroller. If RT condition is applied, both cirutl objects will be attached to the same
    // tracker. Left one here.
    public void setTrackers()
    {
        //if (GetComponent<PhotonView>().isMine)
        //{
            if (masterController.condition == MasterController.CONDITION.NM_RT ||
                masterController.condition == MasterController.CONDITION.SM_RT)
            {
                fLeftTracker = leftTracker;
                fRightTracker = leftTracker;
            }
            else
            {
                fLeftTracker = leftTracker;
                fRightTracker = rightTracker;
            }
       // }
    }
}
