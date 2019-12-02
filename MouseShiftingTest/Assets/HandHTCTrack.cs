using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHTCTrack : MonoBehaviour, GenericHand 
{

    public GameObject tracker;
    public Quaternion initialRotation;
    public bool configured;
    private TargetedController targeted;
    public GameObject handRepresentation;

    public Vector3 giveRealPosition()
    {
        return transform.position;
    }

    public void setDraw(bool canDraw)
    {
        handRepresentation.SetActive(canDraw);
    }

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = tracker.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!configured)
            initialRotation = tracker.transform.rotation;
        transform.position = tracker.transform.position;
        transform.rotation = tracker.transform.rotation * Quaternion.Inverse(initialRotation);
    }
}
