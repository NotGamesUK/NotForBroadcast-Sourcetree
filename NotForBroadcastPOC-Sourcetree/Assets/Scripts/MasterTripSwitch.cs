using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterTripSwitch : MonoBehaviour {

    public float maximumPower;
    public bool isOn=false;
    private bool lastOn=true;

    public PlugSocket[] sockets;
    public Switch mySwitch;
    private ValveBox valveBox;
    public float currentPower;
    private 

    // Use this for initialization
    void Start()
    {
        valveBox = FindObjectOfType<ValveBox>();
    }

    // Update is called once per frame
    void Update()
    {
        isOn = mySwitch.isOn;
        if (lastOn != isOn)
        {
            if (isOn)
            {
                RestorePower();
            }
            else
            {
                TripPower();
            }
        }

        // Calculate Current Power
        currentPower = 0;
        foreach (PlugSocket thisSocket in sockets)
        {
            if (thisSocket.hasPower && thisSocket.isOn)
            {
                currentPower += thisSocket.myPower;
            }
        }

        if (isOn && (currentPower > maximumPower || valveBox.isOverheated))
        {
            TripPower();
        }
        lastOn = isOn;
    }

    private void TripPower()
    {

        foreach (PlugSocket thisSocket in sockets)
        {
            thisSocket.hasPower = false;
        }
        mySwitch.SwitchOff();
    }

    private void RestorePower()
    {

        foreach (PlugSocket thisSocket in sockets)
        {
            thisSocket.hasPower = true;
        }
    }
}
