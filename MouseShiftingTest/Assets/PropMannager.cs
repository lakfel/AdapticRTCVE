using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using System.Text;
using System;
//TODO Name and hierarchy is not good here.
// This manages the comunication with the Adaptci.
public class PropMannager : MonoBehaviour
{

    public enum PRESET_TYPE
    {
        NONE,
        FLAT,
        CYLINDER,
        BOOK
    };

    private MasterController masterController;

    public static string serialName = @"\\.\COM3";

    public SerialPort mySPort = new SerialPort(serialName, 115200);

    public void openPort()
    {
        mySPort.Open();
        mySPort.ReadTimeout = 10;
       // if(masterController.isDemo)
         //   mySPort.Write("<-99,4>");
    }
    // Start is called before the first frame update
    void Start()
    {

        masterController = GetComponent<MasterController>();
        if (!GetComponent<PhotonView>().isMine )
        {
            this.enabled = false;
        }
        if(masterController != null 
                && masterController.condition == MasterController.CONDITION.SM_RT)
            try
            {
                openPort();
            }
            catch(Exception e)
            {
                mySPort = null;
                Debug.Log("ERROR OPENNING PORT " + e.Message);
            }
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (masterController.condition == MasterController.CONDITION.SM_RT)
            try
            {
                if (mySPort.CDHolding)
                { }
            }
            catch (Exception e)
            {
                Debug.Log("Port closed, re opening");
                mySPort = new SerialPort(serialName, 115200);
                openPort(); //REMOVE THIS
            }
    }

    public void adapticCommand(PRESET_TYPE type)
    {
        
        //Debug.Log("PropManager ---- Adaptic  " + type.ToString());
        if (type == PRESET_TYPE.FLAT)
        {
            mySPort.Write("<1>");
        }
        else if (type == PRESET_TYPE.CYLINDER)
        {
            mySPort.Write("<2>");
        }
        else if (type == PRESET_TYPE.BOOK)
        {
            mySPort.Write("<1>");
        }
        mySPort.DiscardOutBuffer();
    }

    public string readData()
    {
        string data = "";
        /*//for(int i =0; i<10000 && !string.Equals(data,"OK"); i++)
            try
            {
                data = mySPort.ReadLine();
            }
            catch(Exception e)
            {

            }*/

        return data;
    }
}
