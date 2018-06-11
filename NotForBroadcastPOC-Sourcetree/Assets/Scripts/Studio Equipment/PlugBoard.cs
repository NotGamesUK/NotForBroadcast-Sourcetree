using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugBoard : MonoBehaviour {

    [HideInInspector]
    public float currentPower;
    private PlugSocket[] sockets;
    private bool hasPower;



    // Use this for initialization
    void Start()
    {
        sockets = GetComponentsInChildren<PlugSocket>();
    }

    // Update is called once per frame
    void Update()
    {

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
