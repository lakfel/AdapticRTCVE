using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericHand
{
    Vector3 giveRealPosition();
    void setDraw(bool canDraw);
    bool canDraw();
}
