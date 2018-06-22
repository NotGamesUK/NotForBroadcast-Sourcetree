using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour {

    [Tooltip ("The rotation from 12 O-Clock to Maximum (same in minus takes gauge to minimum")]
    public float myMaxRotation;
    [Tooltip("Set to false if the alarm triggers at the BOTTOM of the gauge")]
    public bool alarmsAtTop=true;
    [Range (0,100)]
    public float myAlarmPercentage, myOrangePercentage, myRedPercentage, myFlashPercentage;
    [Tooltip ("This is the flash rate in seconds when the LED starts flashing.  It will eventually (FINAL VERSION) increase in speed after alarm % is reached.")]
    public float myLEDFlashRate;
    public enum LEDColours { Red, Orange, Green }
    [HideInInspector]
    public LEDColours myLEDColour=LEDColours.Green;
    public AudioClip myAlarmSound;


    private GaugeNeedle myNeedle;
    public LED myLED;
    private AudioSource myAlarm;
    private float fullRotation;
    private Quaternion startRotation;
    private bool alarmSounding, LEDFlashing;

    // TEMPORARY FOR TESTING - REMOVE:
    [Range (0, 100)]
    public float currentReading;
    private float lastReading;


    // Use this for initialization
    void Start () {
        myNeedle = GetComponentInChildren<GaugeNeedle>();
        myAlarm = GetComponent<AudioSource>();
        fullRotation = myMaxRotation * 2;
        startRotation = myNeedle.transform.rotation;

        myLEDColour = LEDColours.Green;

        // Reverse Percentages for downwards Gauge
        if (!alarmsAtTop)
        {
            myAlarmPercentage = 100 - myAlarmPercentage;
            myOrangePercentage = 100 - myOrangePercentage;
            myRedPercentage = 100 - myRedPercentage;
            myFlashPercentage = 100 - myFlashPercentage;
        }

        myAlarm.clip = myAlarmSound;
        myAlarm.loop = true;

        // TEMPORARY:
        lastReading = currentReading;
    }
	
	// Update is called once per frame
	void Update () {
        if (lastReading!=currentReading) {
            //Debug.Log("Changing Needle to: " + currentReading);
            SetNeedle(currentReading);
            lastReading = currentReading;
        }
	}

    public void SetNeedle (float thisPercentage)
    {
        float newRotation = ((thisPercentage/100) * fullRotation)-myMaxRotation;
        //Debug.Log("New Rotation: " + newRotation + " degrees.");
        myNeedle.transform.rotation = startRotation;
        myNeedle.transform.Rotate(Vector3.down * newRotation);
        float thisComparePercentage = thisPercentage;
        if (!alarmsAtTop) // REVERSE PERCENTAGE FOR ALARMS ETC AT BOTTOM END
        {
            thisComparePercentage = 100 - thisComparePercentage;
        }
        // Set LED
        LEDColours lastLEDcolour = myLEDColour;
        myLEDColour = LEDColours.Green;
        if (thisComparePercentage >= myOrangePercentage)
        {
            myLEDColour = LEDColours.Orange;
        }
        if (thisComparePercentage >= myRedPercentage)
        {
            myLEDColour = LEDColours.Red;
        }
        if (lastLEDcolour != myLEDColour)
        {
            switch (myLEDColour)
            {
                case LEDColours.Green:
                    myLED.GoGreen();
                    break;

                case LEDColours.Orange:
                    myLED.GoOrange();
                    break;

                case LEDColours.Red:
                    myLED.GoRed();
                    break;

            }
        }
        // LED Flash Control

        if (LEDFlashing && thisComparePercentage < myFlashPercentage)
        {
            myLED.FlashOff();
            LEDFlashing = false;

        } else if (!LEDFlashing && thisComparePercentage >= myFlashPercentage)
        {
            myLED.FlashOn(myLEDFlashRate);
            LEDFlashing = true;
        }

        // Sound Alarms

        if (alarmSounding && thisComparePercentage < myAlarmPercentage)
        {
            myAlarm.Stop();
            alarmSounding = false;

        }
        else if (!alarmSounding && thisComparePercentage >= myAlarmPercentage)
        {
            myAlarm.Play();
            alarmSounding = true;
        }

    }

}
