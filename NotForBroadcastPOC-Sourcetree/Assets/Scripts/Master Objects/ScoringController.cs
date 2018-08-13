using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

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
    private ScoringData.ScoreColour currentFootageColour;

    //
    ////    Scoring System 02    ////
    //

    [Space(5)]
    [Header("Scoring System 02")]
    public VideoPlayer[] screen; // 0-3 are broadcast screens, 4-7 are Bleep Screens
    public BackWallLight myWallLightRed, myWallLightOrange, myWallLightGreen;
    public AudioSource mySFXPlayer;
    public VUBar myBleepLight;

    private ScoringData.ScoreColour[] screenAudioColour = new ScoringData.ScoreColour[8]; // 0-3 are broadcast screens, 4-7 are Bleep Screens
    private ScoringData.ScoreColour[] screenVideoColour= new ScoringData.ScoreColour[4]; // Based on whichever screen is currently showing on broadcast TV
    private long[] lastFrame = new long[8];
    private int[] listPosition = new int[8];
    private List<ScoringData>[] sequenceScoring = new List<ScoringData>[3];
    private List<ScoringData> currentSequence = new List<ScoringData>();
    private bool[] watchingChannel = new bool[4];
    private int currentSequenceNumber, currentBroadcastScreen, bleepWatchingCount;
    private BroadcastTV myBroadcastScreen;
    private ScoringData.ScoreColour broadcastScreenColour;


    // Use this for initialization
    void Start () {
        myMasterController = GetComponent<MasterController>();
        myVUMeter = FindObjectOfType<AudienceVUMeter>();
        myScoringMode = ScoringMode.SingleCam;
        audiencePercentage = startAudiencePercentage;
        thisAudienceChange = 0;
        lastAudienceChange = 0;

        // Scoring System 02

        myBroadcastScreen = FindObjectOfType<BroadcastTV>();
        for (int n=0; n<3; n++)
        {
            sequenceScoring[n] = new List<ScoringData>();
        }
        ////
        // TEMP TEST CODE FOR SCORING SYSTEM 02 - REMOVE WHEN BEING CALLED FROM MASTER CONTROLLER
        ////
        SetUpScoreTracker("01-01 Headlines", "01-02 Wildish", "01-03 Party Leaders");

    }


    // Update is called once per frame
    void Update () {

        // SCORING SYSTEM 02

        // Audio and Video Colour Tracking

        for (int n=0; n<8; n++)
        {
            if (screen[n].isPlaying)
            {
                long thisFrame = screen[n].frame;

                if (thisFrame > lastFrame[n])
                {
                    if (listPosition[n] < currentSequence.Count)
                    {
                        ScoringData thisData = currentSequence[listPosition[n]];
                        ScoringData.ScoreColour lastAudioColour = screenAudioColour[n];

                        while (thisData.scoreFrame < thisFrame)
                        {
                            // Is it an audio volour change?
                            if (thisData.editType == ScoringData.ScoreType.Audio)
                            {
                                int thisChannelNumber = n;
                                if (thisChannelNumber > 3) { thisChannelNumber -= 4; }
                                // Is it my channel?
                                if (thisData.channelNumber == thisChannelNumber)
                                {
                                    // Set new color
                                    screenAudioColour[n] = thisData.scoreColour;
                                    Debug.Log("NEW SCORING SYSTEM: Changed Screen " + n + " AUDIO Score to " + screenAudioColour[n] + " at frame " + thisFrame);
                                }
                            }
                            else // It's a video colour change
                            {
                                if (n<4) // Is it a broadcast screen?
                                {
                                    // Is it my channel?
                                    if (thisData.channelNumber == n)
                                    {
                                        // Set new color
                                        screenVideoColour[n] = thisData.scoreColour;
                                        Debug.Log("NEW SCORING SYSTEM: Changed Screen " + n + " VIDEO Score to " + screenAudioColour[n] + " at frame " + thisFrame);
                                    }

                                }

                            }

                            listPosition[n]++;
                            if (listPosition[n] >= currentSequence.Count) { break; }
                            thisData = currentSequence[listPosition[n]];

                            // Bleep Monitoring
                            if (n>=4)
                            {
                                int thisBleepChannel = n - 4;
                                ScoringData.ScoreColour currentColour = screenAudioColour[n];
                                if (lastAudioColour != currentColour)
                                {
                                    if (currentColour == ScoringData.ScoreColour.Red)
                                    {
                                        watchingChannel[thisBleepChannel] = true;
                                    }
                                    else if (watchingChannel[thisBleepChannel])
                                    {
                                        if (!mySFXPlayer.isPlaying)
                                        {
                                            mySFXPlayer.Play();

                                        }
                                        watchingChannel[thisBleepChannel] = false;
                                        myBleepLight.LightOn();
                                        bleepWatchingCount++;
                                        Debug.Log("NEW SCORING SYSTEM - BleepCount INCREASED TO - " + bleepWatchingCount);
                                        Invoke("BleepLightOff", myMasterController.broadcastScreenDelayTime);
                                    }
                                }

                            }

                        }
                    }
                    lastFrame[n] = thisFrame;

                }
            }
        }

        // Has Video Colour Changed?

        if (broadcastScreensLive) {

            // Has Screen Changed
            bool screenChanged = false;
            if (myBroadcastScreen.currentScreen-1 != currentBroadcastScreen)
            {
                currentBroadcastScreen = myBroadcastScreen.currentScreen-1;
                screenChanged = true;
            }

            // Set to red if no screen has been selected:
            ScoringData.ScoreColour thisColour = ScoringData.ScoreColour.Red;
            if (currentBroadcastScreen != -1)
            {
                thisColour = screenVideoColour[currentBroadcastScreen];
            }

            if (broadcastScreenColour != thisColour)
            {

                // TEMP - Set Back wall lights
                // Switch Current Light off
                switch (broadcastScreenColour)
                {
                    case ScoringData.ScoreColour.Red:
                        myWallLightRed.LightOff();
                        break;

                    case ScoringData.ScoreColour.Orange:
                        myWallLightOrange.LightOff();
                        break;

                    case ScoringData.ScoreColour.Green:
                        myWallLightGreen.LightOff();
                        break;


                }

                broadcastScreenColour = thisColour;

                // Switch New Light On
                switch (broadcastScreenColour)
                {
                    case ScoringData.ScoreColour.Red:
                        myWallLightRed.LightOn();
                        break;

                    case ScoringData.ScoreColour.Orange:
                        myWallLightOrange.LightOn();
                        break;

                    case ScoringData.ScoreColour.Green:
                        myWallLightGreen.LightOn();
                        break;


                }


                Debug.Log("New Scoring System - Broadcast Screen Colour changed to " + broadcastScreenColour);

                // Call Footage Colour Change Here!!
                FootageColourChange(broadcastScreenColour);


            }
            else if (screenChanged)
            {
                // Screen Changed but Colour Remained the same
                Debug.Log("New Scoring System - Broadcast Screen Changed but COLOUR is the SAME: " + broadcastScreenColour);

                // Call Footage Counter Reset Here!!
                FootageCounterReset(broadcastScreenColour);

            }

        





            // Monitor Audio Scoring and adjust weighting accordingly
            audioWeighting = 0;

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
                else if (screenAudioColour[n] == ScoringData.ScoreColour.Green)
                {
                    if (bleepOn)
                    {
                        incorrectlyBleepedCount++;
                        Debug.Log("Scoring Controller GREEN BLEEPED - Incrementing Incorrect Bleep Count to " + incorrectlyBleepedCount);
                    }

                }
                else if (screenAudioColour[n] == ScoringData.ScoreColour.Orange)
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
                else if (screenAudioColour[n] == ScoringData.ScoreColour.Red)
                {
                    if (!bleepOn)
                    {
                        Debug.Log("Scoring Controller RED NOT BLEEPED - Heavy Audio Weighting");
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

    void BleepLightOff()
    {
        bleepWatchingCount--;
        if (bleepWatchingCount <= 0)
        {
            bleepWatchingCount = 0;
            myBleepLight.LightOff();
        }
        Debug.Log("NEW SCORING SYSTEM - BleepCount DECREASED TO - " + bleepWatchingCount);

    }


    public void FootageCounterReset(ScoringData.ScoreColour thisColour)
    {
        Debug.Log("Resetting " + thisColour + " counter.");
        FootageColourChange(thisColour);
    }


    public void FootageColourChange(ScoringData.ScoreColour thisColour)
    {

        //Debug.Log("Scoring Controller: Screen Colour Changed.");

        switch (thisColour)
        {
            case ScoringData.ScoreColour.Red:
                footageWeighting = -1;
                footageCountdown = -1;
                break;

            case ScoringData.ScoreColour.Orange:
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

            case ScoringData.ScoreColour.Green:
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

    //
    ////    Scoring System 02    ////
    //

    public void SetUpScoreTracker(string seq1, string seq2, string seq3) {

        sequenceScoring[0].Clear();
        sequenceScoring[1].Clear();
        sequenceScoring[2].Clear();

        // Create all Scoring Lists for Level
        TurnTextFileIntoList(seq1, 0);
        TurnTextFileIntoList(seq2, 1);
        TurnTextFileIntoList(seq3, 2);
        // Reset All Variables
        ResetScoreTracking();

    }

    public void SetUpNextSequenceForTracking()
    {
        currentSequenceNumber++;
        ResetScoreTracking();
        Debug.Log("Scoring Controller Moving to Next Sequence");
    }

    void ResetScoreTracking()
    {
        for (int n = 0; n < 8; n++)
        {
            screenAudioColour[n] = ScoringData.ScoreColour.Null;
            lastFrame[n] = -1;
            listPosition[n] = 0;
            if (n<4)
            {
                screenVideoColour[n] = ScoringData.ScoreColour.Null;
            }
        }
        myWallLightGreen.LightOff();
        myWallLightOrange.LightOff();
        myWallLightRed.LightOff();
        bleepWatchingCount = 0;
        currentSequence = sequenceScoring[currentSequenceNumber];

        // FOR DEBUGGING - REMOVE THIS NEXT LINE
        PrintCurrentSequenceScoringListToDebugger();
        //

        currentBroadcastScreen = -1;
        broadcastScreenColour = ScoringData.ScoreColour.Null;
    }

    private void TurnTextFileIntoList(string thisFileName, int thisListNumber)
    {
        TextAsset thisScoringFile = (TextAsset)Resources.Load(thisFileName, typeof(TextAsset)); // "/NGVDs/"+thisFileName+".ngvd"
        StringReader myTextReader = new StringReader(thisScoringFile.text);
        if (myTextReader == null)
        {
            Debug.Log("File: "+thisFileName+ " not found or not readable.");
        }
        else
        {
            // Read each line from the file
            
            string thisLine = myTextReader.ReadLine();
            thisLine = myTextReader.ReadLine();
            thisLine = myTextReader.ReadLine();
            while (thisLine != "XXX")
            {
                Debug.Log(thisFileName+" --> " + thisLine);
                string thisTypeChar = thisLine.Substring(0, 1);
                ScoringData.ScoreType thisType = ScoringData.ScoreType.Audio;
                if (thisTypeChar=="V") { thisType = ScoringData.ScoreType.Video; }
                Debug.Log("Score Type: " + thisType);
                string thisChannelString = thisLine.Substring(1, 1);
                int thisChannel = int.Parse(thisChannelString);
                Debug.Log("Channel: " + thisChannel);
                string thisColourChar = thisLine.Substring(2, 1);
                ScoringData.ScoreColour thisColour = ScoringData.ScoreColour.Red;
                if (thisColourChar == "O") { thisColour = ScoringData.ScoreColour.Orange; }
                else if (thisColourChar == "G") { thisColour = ScoringData.ScoreColour.Green; }
                Debug.Log("Colour: " + thisColour);
                string thisFrameString = thisLine.Substring(3);
                float thisFrame = float.Parse(thisFrameString);
                Debug.Log("Frame: " + thisFrame);
                thisLine = myTextReader.ReadLine();
                sequenceScoring[thisListNumber].Add(new ScoringData(thisFrame, thisType, thisChannel, thisColour));
            }
        }

    }

    void PrintCurrentSequenceScoringListToDebugger()
    {
        int thisListLength = currentSequence.Count;
        Debug.Log("Sequence " + currentSequenceNumber + " - List Length: " + thisListLength);
    }
}
