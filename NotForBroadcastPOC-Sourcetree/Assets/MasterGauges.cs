using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterGauges : MonoBehaviour {

    [Tooltip("Above this value valve heat will increase rapidly.")]
    public float maxPower;
    [Tooltip("Above this % valves will start to heat up.")]
    [Range(0,100)]
    public float orangePowerPercent;
    [Tooltip("Starting Heat.")]
    public float roomTemperature;
    [Tooltip("Above this value Trip Switch will blow.")]
    public float maxTemperature;
    [Tooltip("Above this % alarms will sound and lights turn on - 5 seconds (max) from TripSwitch")]
    [Range(0,100)]
    public float orangeTemperaturePercent;
    [HideInInspector]
    public string myPowerStatus = "Green";

    public Text myPowerDisplay;
    public Text myTemperatureDisplay;
    public Text mySignalDisplay;
    private MasterTripSwitch myPowerObject;
    private ValveBox myTemperatureObject;
    

	// Use this for initialization
	void Start ()
    {
        myPowerObject = FindObjectOfType<MasterTripSwitch>();
        myTemperatureObject = FindObjectOfType<ValveBox>();
    }

    // Update is called once per frame
    void Update()
    {
        // Display Power Reading
        float thisPower = myPowerObject.currentPower;
        myPowerDisplay.text = (Mathf.Round(thisPower) + "/" + maxPower);
        myPowerDisplay.color = Color.white;
        myPowerStatus = "None";
        if (myPowerObject.isOn)
        {
            myPowerDisplay.color = Color.green;
            if (thisPower > 0)
            {
                myPowerStatus = "Green";
            }
        }
        if (thisPower / maxPower >= orangePowerPercent / 100)
        {
            // Turn Text Orange
            myPowerDisplay.color = Color.yellow;
            myPowerStatus = "Orange";
        }
        if (thisPower > maxPower)
        {
            // Turn Text Red
            myPowerDisplay.color = Color.red;
            myPowerStatus = "Red";

        }

        // Display Temperature Readings
        float thisTemperature = myTemperatureObject.currentTemperature;
        myTemperatureDisplay.text = (Mathf.Round(thisTemperature) / maxTemperature * 100+"%");
        float orangeTemperature = maxTemperature * (orangeTemperaturePercent / 100);
        myTemperatureDisplay.color = Color.green;
        if (thisTemperature>orangeTemperature)
        {
            myTemperatureDisplay.color = Color.yellow;
        }
        if (thisTemperature>maxTemperature || myTemperatureObject.isOverheated)
        {
            myTemperatureDisplay.color = Color.red;
        }
    }

}
