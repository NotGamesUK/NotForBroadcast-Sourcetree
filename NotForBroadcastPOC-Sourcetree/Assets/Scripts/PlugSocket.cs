using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugSocket : MonoBehaviour {

    public float myPower;
    public bool hasPower = true;
    public bool isOn;


    private Switch mySwitch;
    private bool lastPower;

    // Use this for initialization
    void Start () {
        mySwitch = GetComponentInChildren<Switch>();
        isOn = mySwitch.isOn;
        lastPower = hasPower;
	}
	
	// Update is called once per frame
	void Update () {
        isOn = mySwitch.isOn;
        if (lastPower != hasPower)
        {
            if (hasPower)
            {
                PowerOn();
            } else
            {
                PowerOff();
            }
        }
        lastPower = hasPower;
	}

    public void PowerOn ()
    {
        mySwitch.hasPower = true;
    }

    public void PowerOff ()
    {
        mySwitch.hasPower = false;
    }
}
