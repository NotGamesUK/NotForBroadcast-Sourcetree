using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugSocket : MonoBehaviour {

    public PowerManagement myDevice;
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

        if (isOn != mySwitch.isOn)
        {
            if (isOn)
            {
                SwitchOff();
            }
            else
            {
                SwitchOn();
            }
        }
        isOn = mySwitch.isOn;

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
	}

    public void PowerOn ()
    {
        //Debug.Log("Power On");
        mySwitch.hasPower = true;
        hasPower = true;
        if (isOn)
        {
        myDevice.PowerChange();
        }
    }

    public void PowerOff ()
    {
        //Debug.Log("Power Off");

        mySwitch.hasPower = false;
        hasPower = false;
        if (isOn)
        {
            myDevice.PowerChange();
        }

    }

    void SwitchOn()
    {
        //Debug.Log("Switch On");
        isOn = true;
        if (hasPower)
        {
            myDevice.PowerChange();
        }
    }
    

    void SwitchOff()
    {
        //Debug.Log("Switch Off");
        isOn = false;
        if (hasPower)
        {
            myDevice.PowerChange();
        }
    }
}
