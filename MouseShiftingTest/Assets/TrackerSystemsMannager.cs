using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerSystemsMannager : MonoBehaviour, IPunObservable
{
    public Tracker rightTracker;
    public Tracker leftTracker;

    // This defines the type of tracke we can use.
    // If new tracker wants to be used, it mustbe included below and implement all the logic around it
    public enum TYPE_TRACKER
    {
        VIVE,
        OPTITRACK,
        HYDRA_RAZER
    }

    public TYPE_TRACKER typeTracker;

    // It steores the selected set of trackes
    private TrackerSet selectedSetTrackers;

    // Array wit all set of trackers objects
    // By definition, each set must include 2 objects. The first one includes all the 
    // definitios for the LEFT tracker, the second about RIGHT
    public List<TrackerSet> setTrackers;
    private GameObject master;
    private TrackerMannager trackerMannager;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<PhotonView>().isMine)
        { 
            selectedSetTrackers = null;
            foreach(TrackerSet ts in setTrackers)
            {
                if(ts.typeTracker == typeTracker)
                {
                    ts.gameObject.SetActive(true);
                    selectedSetTrackers = ts;
                }
                else
                {
                    ts.gameObject.SetActive(false);
                }
            }

            leftTracker.trackerRep = selectedSetTrackers.gameObject.transform.GetChild(0)
                                    .transform.gameObject;

            rightTracker.trackerRep = selectedSetTrackers.gameObject.transform.GetChild(1)
                                        .transform.gameObject;
        }
        else
        {
            leftTracker.gameObject.SetActive(false);
            rightTracker.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();
    }
}
