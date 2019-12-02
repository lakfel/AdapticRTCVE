using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITracker : MonoBehaviour
{


    public abstract void attach();

    public abstract void detach();

    private PropController positionReference;
    public PropController PositionReference { get => positionReference; set => positionReference = value; }

}
