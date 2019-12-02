using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpecs : MonoBehaviour, IPunObservable
{
    
    public bool useAdaptic;
    public PropMannager.PRESET_TYPE type;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
