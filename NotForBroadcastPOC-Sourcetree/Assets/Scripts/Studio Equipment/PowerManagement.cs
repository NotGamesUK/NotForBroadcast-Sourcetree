using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManagement : MonoBehaviour {

    public PlugSocket myPlug;
    private bool lastPower;
    private bool lastOn;

    // Use this for initialization
    void Start()
    {
        lastPower = myPlug.hasPower;
        lastOn = myPlug.isOn;
        
        if (myPlug.isOn && myPlug.hasPower)
        {
            SendMessage("PowerOn");
        }
        else
        {
            SendMessage("PowerOff");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PowerChange()

    {
        //Debug.Log("Power Change Called");

        if (myPlug.hasPower != lastPower || myPlug.isOn != lastOn)
        {
            if (myPlug.hasPower && myPlug.isOn)
            {
                SendMessage("PowerOn");
            }
            else
            {
                SendMessage("PowerOff");
            }
        }
        lastPower = myPlug.hasPower;
        lastOn = myPlug.isOn;

    }

}
