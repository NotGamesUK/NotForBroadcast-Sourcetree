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
    [HideInInspector]
    public float videoStrength;

    public Text myPowerTextDisplay;
    public Text myTemperatureTextDisplay;
    public Text mySignalTextDisplay;

    public Gauge myPowerGauge;
    public Gauge myTemperatureGauge;
    public Gauge mySignalGauge;

    private MasterTripSwitch myPowerObject;
    private ValveBox myTemperatureObject;
    private SatelliteDish myDish;
    private PlayerFrequencyDisplayObject myPlayer;
    private SoundDesk myDesk;
    private BroadcastTV myBroadcastTV;



    // Use this for initialization
    void Start ()
    {
        myPowerObject = FindObjectOfType<MasterTripSwitch>();
        myTemperatureObject = FindObjectOfType<ValveBox>();
        myDish = FindObjectOfType<SatelliteDish>();
        myPlayer = FindObjectOfType<PlayerFrequencyDisplayObject>();
        myDesk = FindObjectOfType<SoundDesk>();
        myBroadcastTV = FindObjectOfType<BroadcastTV>();
    }

    // Update is called once per frame
    void Update()
    {
        // Display Power Reading
        float thisPower = myPowerObject.currentPower;
        myPowerTextDisplay.text = (Mathf.Round(thisPower) + "/" + maxPower);
        float thisPowerPercentage = Mathf.Round((thisPower / maxPower) * 100);
        //Debug.Log("SENDING " + thisPowerPercentage + "% TO POWER GAUGE.");
        myPowerGauge.SetNeedle(thisPowerPercentage);

        myPowerTextDisplay.color = Color.white;
        myPowerStatus = "None";
        if (myPowerObject.isOn)
        {
            myPowerTextDisplay.color = Color.green;
            if (thisPower > 0)
            {
                myPowerStatus = "Green";
            }
        }
        if (thisPower / maxPower >= orangePowerPercent / 100)
        {
            // Turn Text Orange
            myPowerTextDisplay.color = Color.yellow;
            myPowerStatus = "Orange";
        }
        if (thisPower > maxPower)
        {
            // Turn Text Red
            myPowerTextDisplay.color = Color.red;
            myPowerStatus = "Red";

        }

        // Display Temperature Readings
        float thisTemperature = myTemperatureObject.currentTemperature;
        myTemperatureTextDisplay.text = (Mathf.Round(thisTemperature) / maxTemperature * 100+"%");
        float thisTemperaturePercentage = Mathf.Round(thisTemperature) / maxTemperature * 100;
        //Debug.Log("SENDING " + thisTemperaturePercentage + "% TO TEMPERATURE GAUGE.");
        myTemperatureGauge.SetNeedle(thisTemperaturePercentage);


        float orangeTemperature = maxTemperature * (orangeTemperaturePercent / 100);
        myTemperatureTextDisplay.color = Color.green;
        if (thisTemperature>orangeTemperature)
        {
            myTemperatureTextDisplay.color = Color.yellow;
        }
        if (thisTemperature>maxTemperature || myTemperatureObject.isOverheated)
        {
            myTemperatureTextDisplay.color = Color.red;
        }

        // Display Broadcast Reading
        float thisTilt = myDish.transform.localEulerAngles.x;
        //Debug.Log("This Tilt: " + thisTilt);

        // Start Video and Audio Signal at 100%
        videoStrength = 100f;

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
        //myBroadcastTV.SetWhiteNoiseVideoLevel(videoStrength);


        // Mix audio over broadcast and turn down other channels propotional to Audio Signal
        float audioInterferenceStrength = -(videoStrength * 0.8f);
        //Debug.Log("White Noise Volume: " + audioInterferenceStrength);
        //myDesk.SetWhiteNoiseAudioLevel(audioInterferenceStrength);

        // Display Video and Audio Signal fidelity on panel
        mySignalTextDisplay.text = Mathf.Round(videoStrength).ToString() + "%";

        //Debug.Log("SENDING " + Mathf.Round(videoStrength) + "% TO SIGNAL GAUGE.");
        mySignalGauge.SetNeedle(Mathf.Round(videoStrength));

        mySignalTextDisplay.color = Color.green;
        if (videoStrength < 65) {
            mySignalTextDisplay.color = Color.yellow;
        }
        if (videoStrength < 35) {
            mySignalTextDisplay.color = Color.red;
        }

    }

}
