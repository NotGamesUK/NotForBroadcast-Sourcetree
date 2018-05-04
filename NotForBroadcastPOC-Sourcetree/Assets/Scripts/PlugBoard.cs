using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugBoard : MonoBehaviour {

    //public float maximumPower;
    [HideInInspector]
    public float currentPower;
    private PlugSocket[] sockets;
    private bool hasPower;

    //private bool lastPower;     // REMOVE AFTER TRIP SWITCH TAKES CONTROL


    // Use this for initialization
    void Start()
    {
        sockets = GetComponentsInChildren<PlugSocket>();
        //lastPower = hasPower;
    }

    // Update is called once per frame
    void Update()
    {
        // TESTING CODE - REMOVE WHEN TRIP SWITCH SENDING POWER ON AND OFF

        //if (lastPower != hasPower)
        //{
        //    if (hasPower)
        //    {
        //        PowerOn();
        //    } else
        //    {
        //        PowerOff();
        //    }
        //}
        //lastPower = hasPower;

        // ----------------------------------------------------------------


        currentPower = 0;
        foreach (PlugSocket thisSocket in sockets)
        {
            if (thisSocket.isOn && thisSocket.hasPower)
            {
                currentPower += thisSocket.myPower;
            }
        }
    }

    public void PowerOn()
    {
        foreach (PlugSocket thisSocket in sockets)
        {
            thisSocket.PowerOn();
        }
        hasPower = true;
    }

    public void PowerOff()
    {
        foreach (PlugSocket thisSocket in sockets)
        {
            thisSocket.PowerOff();
        }
        hasPower = false;
    }
}
