using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraTracker :  MonoBehaviour
{
    
    Vector3 m_baseOffset;
    Quaternion m_baseOffset_Rot;
    public float m_sensitivity = 0.00017f; // Sixense units are in mm
    bool m_bInitialized;
    public int orientation;

    public float m_sensitivityX;
    public float m_sensitivityY;
    public float m_sensitivityZ;


    public SixenseInput.Controller Controller { get => controller; set => controller = value; }
    public SixenseHands SideController;

    private SixenseInput.Controller controller;
    private SixenseHands sideController;

    // Start is called before the first frame update
    void Start()
    {
       
 
    }
    private void Awake()
    {
        if (Controller == null)
        {
            Controller = SixenseInput.GetController(SideController);
        }
    }    // Update is called once per frame
    void Update()
    {
        if (Controller == null)
        {
            Controller = SixenseInput.GetController(SideController);
        }
        if (m_bInitialized && IsControllerActive())
        {
            Vector3 nPos = orientation * Controller.Position;
            Vector3 realPos = Vector3.zero;
            realPos = nPos;
            realPos = new Vector3(realPos.x * m_sensitivityX, realPos.y*m_sensitivityY, realPos.z*m_sensitivityZ);
            transform.position = realPos;
            transform.rotation = Controller.Rotation;
        }
 
    }


    /** returns true if a controller is enabled and not docked */
    bool IsControllerActive()
    {
        //Debug.Log("Controller > Isnull: " + controller != null + " ---> Enabled:" + controller.Enabled + "----> Docked: " + controller.Docked);
        return (controller != null && controller.Enabled && !controller.Docked);
    }
}
