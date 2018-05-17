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
    [Tooltip("Tolerence in degrees for satellite tilt after which interference starts (Angle Range is 290-350 with ideal at 310).")]
    [Range(0, 40)]
    public float dishTolerance;
    
    [HideInInspector]
    public string myPowerStatus = "Green";

    public Text myPowerDisplay;
    public Text myTemperatureDisplay;
    public Text mySignalDisplay;
    public Material interferance;
    private MasterTripSwitch myPowerObject;
    private ValveBox myTemperatureObject;
    private SatelliteDish myDish;
    private PlayerFrequencyDisplayObject myPlayer;
    private SoundDesk myDesk;
    private Color interferanceAlpha = new Color(1f, 1f, 1f, 1f);



    // Use this for initialization
    void Start ()
    {
        myPowerObject = FindObjectOfType<MasterTripSwitch>();
        myTemperatureObject = FindObjectOfType<ValveBox>();
        myDish = FindObjectOfType<SatelliteDish>();
        myPlayer = FindObjectOfType<PlayerFrequencyDisplayObject>();
        myDesk = FindObjectOfType<SoundDesk>();
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

        // Display Broadcast Reading
        float thisTilt = myDish.transform.localEulerAngles.x;
        //Debug.Log("This Tilt: " + thisTilt);

        // Start Video and Audio Signal at 100%
        float videoStrength = 100f;

        // Subtract Video based on dish tilt
        float tiltMin = myDish.idealTilt - dishTolerance;
        float tiltMax = myDish.idealTilt + dishTolerance;
        float thisDifference = 0;
        if (thisTilt<tiltMin || thisTilt>tiltMax)
        {
            if (thisTilt < tiltMin)
            {
                thisDifference = tiltMin - thisTilt;
            } else
            {
                thisDifference = thisTilt - tiltMax;
            }
            thisDifference = Mathf.Round(thisDifference);
            //Debug.Log("This Difference: " + thisDifference + " out of possible " + (Mathf.Round (myDish.maxTilt - tiltMax)));
            videoStrength -= Mathf.Round((thisDifference / (Mathf.Round(myDish.maxTilt - tiltMax))) * 100);

            //Debug.Log("Video Strength: "+videoStrength+"%");
        }
        //Debug.Log("Video Strength Pre: " + videoStrength + "%");
        // Subtract Video and Audio based on variables in Signal Control Panel
        if (videoStrength>100-myPlayer.currentWhiteNoiseLevel)
        {
            videoStrength = 100 - myPlayer.currentWhiteNoiseLevel;
        }
        //Debug.Log("Video Strength Post: " + videoStrength + "%");
        // Adjust alpha of interference screen based on Video Signal level
        interferanceAlpha.a = 1 - (videoStrength/100);
        //Debug.Log("Colour: " + interferanceAlpha);
        interferance.color = interferanceAlpha;

        // Mix audio over broadcast and turn down other channels propotional to Audio Signal
        float audioInterferenceStrength = -(videoStrength * 0.8f);
        //Debug.Log("White Noise Volume: " + audioInterferenceStrength);
        myDesk.SetWhiteNoiseVolume(audioInterferenceStrength);

        // Display Video and Audio Signal fidelity on panel
        mySignalDisplay.text = videoStrength.ToString() + "%";
        mySignalDisplay.color = Color.green;
        if (videoStrength < 65) { mySignalDisplay.color = Color.yellow; }
        if (videoStrength < 35) { mySignalDisplay.color = Color.red; }

    }

}
