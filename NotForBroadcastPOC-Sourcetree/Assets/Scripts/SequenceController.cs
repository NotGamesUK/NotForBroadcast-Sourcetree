﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SequenceController : MonoBehaviour {

    [Tooltip("Number of seconds delay between broadcast screen and master screen")]
    public float broadcastScreenDelayTime;
    public string searchForThis;
    //[HideInInspector]
    public int preparedScreensCount, targetScreensCount;


    private DataStorage myDataStore;
    private VisionMixer myVisionMixer;
    private InterferenceSystem myInterferenceSystem;
    private BackWallClock myClock;
    private BroadcastTV myBroadcastScreen;
    private bool waitingForScreens = false;
    private float thisPreSequenceVideoLength;
    private int foundPos;
    [HideInInspector]
    public bool preRollReady;



    // Use this for initialization
    void Start()
    {
        myDataStore = GetComponent<DataStorage>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myInterferenceSystem = FindObjectOfType<InterferenceSystem>();
        myClock = FindObjectOfType<BackWallClock>();
        myBroadcastScreen = FindObjectOfType<BroadcastTV>();
        Invoke("TEMPTestDeleteMe", 3);
    }

    void TEMPTestDeleteMe()
    {
        PrepareSequence(searchForThis);
    }

    // Update is called once per frame
    void Update()
    {
        // If waiting for screens...
        if (waitingForScreens)
        {
            // If Advert Screen is Prepared (CHECK preRollReady - is set by Broadcast system): START CLOCK. START AD SCREEN.
            if (preRollReady)
            {
                myBroadcastScreen.PlayAdvert();
                myClock.StartClock();
                preRollReady = false;
                waitingForScreens = false;
                // Invoke StartSequence in Ad-length Seconds minus Run-In Seconds
                Invoke("StartSequence", thisPreSequenceVideoLength - myDataStore.sequenceData[foundPos].runIn);
            }

        }
    }

    public void PrepareSequence(string thisSequenceName)
    {
        foundPos = -1;
        int thisLength = myDataStore.sequenceData.Length;
        print("Searching For Sequence: " + thisSequenceName);
        for (int n = 0; n < thisLength; n++)
        {
            if (thisSequenceName == myDataStore.sequenceData[n].SequenceName)
            {
                foundPos = n;
                print("FOUND at Position " + foundPos);
            }
        }
        if (foundPos == -1)
        {
            Debug.LogError("Cannot Find Sequence with name: " + thisSequenceName);
            return; // BREAK OUT OF LOAD LOOP EARLY AS THERE IS NO SEQUENCE HERE
        }
        
        // Set prepared screens count to 0
        preparedScreensCount = 0;
        // Tell each VM screen and Broadcast Screens to prepare and increment targetcount
        myVisionMixer.PrepareScreens(myDataStore.sequenceData[foundPos].screenVideo, myDataStore.sequenceData[foundPos].screenAudio);
        myBroadcastScreen.PrepareScreens(myDataStore.sequenceData[foundPos].screenVideo, myDataStore.sequenceData[foundPos].screenAudio);
        targetScreensCount = myDataStore.sequenceData[foundPos].screenVideo.Length*2;

        // Get Length of Video for Screen 01
        double thisSequenceVideoLength= myDataStore.sequenceData[foundPos].screenVideo[0].length;
        Debug.Log("Returned Length: " + thisSequenceVideoLength + " seconds.");

        // Set clock to Ad-Length second countdown minus broadcast delay - First Ad is always "Later tonight...."?
        thisPreSequenceVideoLength = (float)myDataStore.sequenceData[foundPos].preSequenceBroadcastVideo.length;
        myClock.SetTimeAndHold(thisPreSequenceVideoLength, true);

        // Prepare Ad (or countdown if at start) on Broadcast screen
        myBroadcastScreen.PrepareAdvert(myDataStore.sequenceData[foundPos].preSequenceBroadcastSmallerVideo, myDataStore.sequenceData[foundPos].preSequenceBroadcastAudio);
        preRollReady = false;

        // Check VM settings. Tell control room screen to prepare selected screen.  Lock VM buttons.  If no screen selected set to Screen 01 as default.

        // Tell Resistance Screen to prepare (and increment targetcount if required)
        myBroadcastScreen.PrepareResistance(myDataStore.sequenceData[foundPos].resistanceVideo, myDataStore.sequenceData[foundPos].resistanceAudio);

        // Load Level into Level Controller

        // Tell update to begin monitoring prepared screens count until target is reached - all ready
        waitingForScreens = true;

        // Clear EDL
    }

    void StartSequence()
    {
        Debug.Log("STARTING SCREENS");
        // if TARGET COUNT=PREPARED COUNT:
        // Send GO! to VM Screens
        myVisionMixer.PlayScreens();
        // Send GO to Control Room Screen
        // Invoke GO! on StartBroadcastScreens with delay of broadcastScreenDelayTime;
        Invoke("StartBroadcastScreens", broadcastScreenDelayTime);
        // Set LevelController.LevelHasStarted
        // Send GO to Resistance screen
        // Set clock to sequence length minus overspill

        // IF NOT - INVOKE StartSequence AGAIN IN 0.1 SECONDS
    }

    void StartBroadcastScreens()
    {
        Debug.Log("STARTING BROADCAST SCREENS");
        // Send GO to Broadcast Screens
        // Set Mode to follow EDL
    }

    void SequenceOverrun()
    {
        // THIS IS CALLED WHEN SEQUENCE HAS CUT TO BREAK AND AD HAS NOT BEEN PLAYED
        // Sound Alarms (fringe screen in red? flash lights? siren?)
        // If Player does not play in the AD after X Seconds the level is failed
    }

    public void AdvertPlayed(VideoClip thisAdvert)
    {
        // THIS IS CALLED BY VHS CONTROL PANEL WHEN PLAY BUTTON IS PRESSED
        // Tell MasterController that the sequence has ended and also which Advert has been selected to play.
    }
}
