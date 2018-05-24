using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterTripSwitch : MonoBehaviour {

    private float maximumPower;
    public bool isOn= false;
    private bool lastOn=true;

    private PlugBoard myPlugBoard;
    private Switch mySwitch;
    private ValveBox myValveBox;
    private MasterGauges myGauges;
    //[HideInInspector]
    public float currentPower;

    // Use this for initialization
    void Start()
    {
        myValveBox = FindObjectOfType<ValveBox>();
        myPlugBoard = FindObjectOfType<PlugBoard>();
        myGauges = FindObjectOfType<MasterGauges>();
        mySwitch = GetComponentInChildren<Switch>();
        maximumPower = myGauges.maxPower;
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
        currentPower = myPlugBoard.currentPower;

        if (isOn && (myValveBox.isOverheated))
        {
            TripPower();
        }
        lastOn = isOn;
    }

    private void TripPower()
    {
        myPlugBoard.PowerOff();
        mySwitch.SwitchOff();
    }

    private void RestorePower()
    {
        myPlugBoard.PowerOn();
    }

    void SwitchOn()
    {
        // Called by Message when switch changes
        Debug.Log("TripSwitch On");
    }

    void SwitchOff()
    {
        // Called by Message when switch changes
        Debug.Log("TripSwitchOff");
    }

}
