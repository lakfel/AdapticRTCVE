using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Leap;

// Position spawn is configured and assured by PUN. Orientation is not. This class tries to set this up.
public class HTCCompensate : MonoBehaviour
{

    //TODO Maybe the height can be configured here. 

    // Flag to selfconfigure the rotation
    public bool selfConfigured;

  

    void Start()
    {
        selfConfigured = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (!selfConfigured)
        {
            GameObject camera = GameObject.Find("Camera");
            if (camera != null)
            {
                float cameraYDegree = camera.transform.eulerAngles.y;
                float myYDegree = transform.eulerAngles.y;
                float diff = cameraYDegree - myYDegree;
                transform.Rotate(new Vector3(0f, -diff, 0f));
                selfConfigured = true;
            }
            else
            {
                selfConfigured = true;
                Debug.Log("JFGA -- No camera found");
            }
        }
    }
}
