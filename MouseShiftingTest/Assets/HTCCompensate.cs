using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using HTC.UnityPlugin.Vive;
using Leap;
using System;
// Position spawn is configured and assured by PUN. Orientation is not. This class tries to set this up.
public class HTCCompensate : MonoBehaviour
{

    //TODO Maybe the height can be configured here. 

    // Flag to selfconfigure the rotation
    public bool selfConfigured;
    public bool handConfigured;
    public Transform desiredPosition;

    public GameObject camera;
    public GameObject hand;

    public LogicGame logicGame;

    void Start()
    {
        selfConfigured = false;
        if (!GetComponent<PhotonView>().isMine)
        {
            this.enabled = false;
        }

        logicGame = GameObject.Find("LevelMannager").GetComponent<LogicGame>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if (!selfConfigured)
                if (camera != null)
                {

                    int idPlayer = 1 + Int32.Parse(gameObject.name.ToCharArray()[gameObject.name.Length - 1] + "");
                    desiredPosition = GameObject.Find("Room/SpawnPositions/User" + idPlayer).transform;

                    transform.rotation = desiredPosition.rotation;

                    float cameraYDegree = camera.transform.eulerAngles.y;
                    float myYDegree = transform.eulerAngles.y;
                    float diff = cameraYDegree - myYDegree;
                    transform.Rotate(new Vector3(0f, -diff, 0f));


                    Transform cam = camera.transform;
                    Vector3 posDiff = new Vector3(cam.position.x - transform.position.x,
                                        cam.position.y - transform.position.y,
                                        cam.position.z - transform.position.z);

                    transform.position = new Vector3(desiredPosition.position.x - posDiff.x,
                                                    desiredPosition.position.y - posDiff.y,
                                                    desiredPosition.position.z - posDiff.z);




                    selfConfigured = true;
                }
                else
                {
                    selfConfigured = true;
                    Debug.Log("JFGA -- No camera found");
                }
            if (!handConfigured)
                if (hand != null)
                {
                    Transform cam = hand.transform;
                    Vector3 posDiff = new Vector3(cam.position.x - transform.position.x,
                                        cam.position.y - transform.position.y,
                                        cam.position.z - transform.position.z);

                    transform.position = new Vector3(desiredPosition.position.x - posDiff.x,
                                                    desiredPosition.position.y - posDiff.y,
                                                    desiredPosition.position.z - posDiff.z);




                    handConfigured = true;
                }
                else
                {
                    selfConfigured = true;
                    Debug.Log("JFGA -- No hand found");
                }
                  
                if (logicGame != null)
                {
                    if(!logicGame.started)
                    {
                        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)
                                   || ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Trigger))
                            {
                            selfConfigured = false;
                            handConfigured = false;
                        }
                    }
                }

        }
    }
}
