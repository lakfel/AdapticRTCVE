using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{

  
    public GameObject voObject;

    public bool[] anglesUsed = new bool[] { false, false, false, false, false };
    public int angleNumber;

    // This must be replaced.
    private Logic logic;

    private GameObject master;
    private PersistanceManager persistanceManager;
    private MasterController masterController;

    // Describes the range of possibly rotations
    //TODO Check this. Last time error ocurred in one cylinder orientation.



    

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

 
}
