﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringController : MonoBehaviour {

    public float startAudiencePercentage, minimumAudiencePercentage, maxGreenSecsMultiCam, maxOrangeSecsMultiCam, maxOrangeSecsSingleCam;
    [Range (0, 5f)]
    public float footageWeight, audioWeight, interferenceWeight, resistanceWeight;
    [Range(0.001f, 1.5f)]
    public float speedOfChange;
    public ScoringPlane[] myAudioScorer;
    public VUBar myUpArrow, myDownArrow;
    private MasterController myMasterController;
    private AudienceVUMeter myVUMeter;
    public enum ScoringMode { SingleCam, MultiCam }
    [HideInInspector]
    public ScoringMode myScoringMode;
    [HideInInspector]
    public bool broadcastScreensLive, bleepOn;
    [HideInInspector]
    public int maxAudioChannel;
    public bool[] channelMuted;
    private int muteCount, incorrectlyBleepedCount;
    private bool orangeAudioWeighted;

    [HideInInspector]
    public float audiencePercentage, footageCountdown, footageWeighting, audioWeighting, interferenceWeighting, thisAudienceChange, lastAudienceChange;
    private ScoringPlane.ScoreColour currentFootageColour;




    // Use this for initialization
    void Start () {
        myMasterController = GetComponent<MasterController>();
        myVUMeter = FindObjectOfType<AudienceVUMeter>();
        myScoringMode = ScoringMode.SingleCam;
        audiencePercentage = startAudiencePercentage;
        thisAudienceChange = 0;
        lastAudienceChange = 0;
    }


    // Update is called once per frame
    void Update () {

        // Monitor Audio Scoring and adjust weighting accordingly
        audioWeighting = 0;

        if (broadcastScreensLive)
        {
            //Debug.Log("Scoring Controller - Currently Scoring Audio");
            muteCount = 0;
            incorrectlyBleepedCount = 0;
            orangeAudioWeighted = false;
            for (int n=0; n<maxAudioChannel; n++)
            {
                if (channelMuted[n])
                {
                    muteCount++;
                }
                else if (myAudioScorer[n].currentColour == ScoringPlane.ScoreColour.Green)
                {
                    if (bleepOn)
                    {
                        incorrectlyBleepedCount++;
                        Debug.Log("Scoring Controller GREEN BLEEPED - Incrementing Incorrect Bleep Count to " + incorrectlyBleepedCount);
                    }

                }
                else if (myAudioScorer[n].currentColour == ScoringPlane.ScoreColour.Orange)
                {
                    if (orangeAudioWeighted == false)
                    {
                        orangeAudioWeighted = true;
                        //Debug.Log("Scoring Controller: Making MODERATE Orange audio out weighting.");
                        // MAKE ORANGE AUDIO BROADCAST WEIGHT ADJUSTMENT

                    }
                    if (bleepOn)
                    {
                        incorrectlyBleepedCount++;
                        Debug.Log("Scoring Controller ORANGE BLEEPED - Incrementing Incorrect Bleep Count to " + incorrectlyBleepedCount);
                    }

                }
                else if (myAudioScorer[n].currentColour == ScoringPlane.ScoreColour.Red)
                {
                    if (!bleepOn)
                    {
                        //Debug.Log("Scoring Controller RED NOT BLEEPED - Heavy Audio Weighting");
                    }

                }
                if (muteCount==maxAudioChannel)
                {
                    Debug.Log("All Viable Channels Muted - Weighting Audio Score.");
                }
                else if (incorrectlyBleepedCount == maxAudioChannel)
                {
                    Debug.Log("Incorrectly Bleeped!  Weighting Audio Score.");
                }



            }



            // Apply Audio Weighting Here


            // Adjust Footage Fail Countdown

            if (footageCountdown > 0)
            {
                footageCountdown -= Time.deltaTime;
                if (footageCountdown <= 0)
                {
                    footageWeighting = -1;
                    footageCountdown = -1;
                }
            }

            // Make Adjustment based on weighting
            lastAudienceChange = thisAudienceChange;
            thisAudienceChange = footageWeighting * Time.deltaTime * footageWeight * speedOfChange;
            audiencePercentage += thisAudienceChange;
            if (thisAudienceChange > 0 && lastAudienceChange<=0)
            {
                // Turn on UpArrow
                myUpArrow.LightOn();
                myDownArrow.LightOff();

            } else if (thisAudienceChange<0 && lastAudienceChange >= 0)
            {
                // Turn on DownArrow
                myDownArrow.LightOn();
                myUpArrow.LightOff();


            } else if (thisAudienceChange == 0)
            {
                // Turn Arrows Off
                myUpArrow.LightOff();
                myDownArrow.LightOff();


            }
            if (audiencePercentage < 0) { audiencePercentage = 0; }
            if (audiencePercentage > 100) { audiencePercentage = 100; }
            myVUMeter.SetToPercentage(audiencePercentage);
            if (audiencePercentage < minimumAudiencePercentage)
            {
                string failString = "Audience Fell Below " + minimumAudiencePercentage.ToString() + "%";
                myMasterController.FailMidLevel(failString);
            }
        }

    }

    public void InitialiseVU()
    {
        audiencePercentage = startAudiencePercentage;
        myVUMeter.SetToPercentage(audiencePercentage);
    }

    public void FootageCounterReset(ScoringPlane.ScoreColour thisColour)
    {
        Debug.Log("Resetting " + thisColour + " counter.");
        FootageColourChange(thisColour);
    }


    public void FootageColourChange(ScoringPlane.ScoreColour thisColour)
    {

        //Debug.Log("Scoring Controller: Screen Colour Changed.");

        switch (thisColour)
        {
            case ScoringPlane.ScoreColour.Red:
                footageWeighting = -1;
                footageCountdown = -1;
                break;

            case ScoringPlane.ScoreColour.Orange:
                footageWeighting = 0;
                switch (myScoringMode)
                {
                    case ScoringMode.SingleCam:
                        footageCountdown = maxOrangeSecsSingleCam;
                        break;

                    case ScoringMode.MultiCam:
                        footageCountdown = maxOrangeSecsMultiCam;
                        break;
                }
                break;

            case ScoringPlane.ScoreColour.Green:
                footageWeighting = 1;
                footageCountdown = -1;
                if (myScoringMode == ScoringMode.MultiCam)
                {
                    footageCountdown = maxGreenSecsMultiCam;
                }
                break;

        }
        currentFootageColour = thisColour;
    }

    public void AudioColourChange(ScoringPlane.ScoreColour thisColour, int thisAudioChannel)
    {

    }


}
