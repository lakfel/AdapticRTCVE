using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{

    public enum SIDE { LEFT, RIGHT };
    public SIDE currentSide;

    public GameObject voObject;

    private readonly Quaternion[] flatRots = {
                                        new Quaternion(0f, 0.0f, 0.7f, 0.7f),
                                        new Quaternion(0.5f, 0.5f , 0.5f , 0.5f),
                                        new Quaternion(0.3f, 0.3f , 0.7f , 0.7f),
                                        new Quaternion(-0.3f, -0.3f , 0.7f , 0.7f),
                                        new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f)
                                    };

    private readonly Quaternion[] cyllRots = {
                                        new Quaternion(0.0f, -0.7f, 0f, 0.7f),
                                        new Quaternion(0.0f, -0.4f, 0.0f, 0.9f),
                                        new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                                        new Quaternion(0.0f, -0.9f, 0.0f, 0.4f),
                                        new Quaternion(0.0f, -1f, 0f, 0.3f)
                                    };


    public bool[] anglesUsed = new bool[] { false, false, false, false, false };
    public int angleNumber;

    // This must be replaced.
    private Logic logic;

    private GameObject master;
    private PersistanceManager persistanceManager;
    private MasterController masterController;

    // Describes the range of possibly rotations
    //TODO Check this. Last time error ocurred in one cylinder orientation.


    public Tracker dTracker;
    

    private Quaternion[] rangeRotation;
    public Quaternion[] RangeRotation { get => rangeRotation; set => rangeRotation = value; }


    /**
     * Position where should be in Virtual world
     */
    public GameObject positionReference;

    /**
     * The distance from the table so we consider the object is still in the participant hand
     */
    private float deltaY;

    /**
     * Guide object 
     */
    public GameObject dockProp;

    /**
     * Reference to PropManager, in charge to control te adaptic 
     */
    public PropMannager propManager;
    public int trackerObject;


    //public Collider handCollider;

    private bool isOverlapped;

    // Start is called before the first frame update
    void Start()
    {
       
            isOverlapped = false;
            deltaY = 0.5f;
            angleNumber = 0;
        
    }

    public void objectGreen(bool isGreen)
    {
        if(voObject != null)
        {
            PropSpecs prop = voObject.GetComponent<PropSpecs>();
            prop.objectGreen(isGreen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        isOverlapped = true;
        
    }
  
  


    private void OnTriggerExit(Collider other)
    {
        isOverlapped = false;
       

    }

    // Update is called once per frame
    void Update()
    {
       
          //if (Physics.OverlapBox(handCollider.transform.position, new Vector3(0.1f, 0.1f, 0.1f)).Length == 1)
    }

    public string preSetShape()
    {
        string presetResult = "";
        if(gameObject.transform.childCount > 0)
        {
            //Debug.Log("PropController ---- Found children");
            GameObject prop = gameObject.transform.GetChild(0).gameObject;
            if (propManager != null)
            {
                //Debug.Log("PropController ---- Asking propmanager.");
                PropSpecs specs = prop.GetComponent<PropSpecs>();
               // propManager.adapticCommand(specs.type);
                //presetResult = propManager.readData();
            }
        }
        return presetResult;
    }
    /*
    public void movePropDock(bool toPointZ)
    {
        if (toPointZ)
        {
            GameObject pointZ = GameObject.Find("HandStartPoint");
            if(pointZ != null)
            {
                dockProp.transform.parent = pointZ.transform;
                dockProp.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
                randomizeRotation(false);
            }
        }
        else
        {
            dockProp.transform.parent = virtualObject.transform;
            dockProp.transform.localPosition = Vector3.zero;
            randomizeRotation(true);
        }
    }

    public void relocatePropDock()
    {
        dockProp.transform.parent = transform.GetChild(0).transform;
        dockProp.transform.localPosition = Vector3.zero;
    }*/
    /*
    public void randomizeRotation (bool neutral)
    {
        
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        int newAnglePosition = 0;
        if(!neutral)
        {
            if(angleNumber == 5)
            {
                for (int i = 0; i < 5; i++)
                    anglesUsed[i] = false;
                angleNumber =0;
            }
            angleNumber++;
            while (anglesUsed[newAnglePosition = Random.Range(0, 5)]) ;
            anglesUsed[newAnglePosition] = true;
        }

        PropSpecs propSpecs = child.GetComponent<PropSpecs>();
        if(propSpecs.type == PropMannager.PRESET_TYPE.CYLINDER)
            dockProp.transform.rotation = cyllRots[neutral ? 0: newAnglePosition];
        else
            dockProp.transform.rotation = flatRots[neutral ? 0 : newAnglePosition];

    } */

    public void activeChildren(bool activate)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(activate);
    }

    public float distanceToDock()
    {
        float distance = 0f;
        distance = Vector3.Distance(transform.position, dockProp.transform.position);
        return distance;
    }
  
    public float distanceToInitialPoint()
    {
        float distance = 0f;
        //distance = Vector3.Distance(transform.position, virtualObject.transform.position);
        return distance;
    }
     

}
