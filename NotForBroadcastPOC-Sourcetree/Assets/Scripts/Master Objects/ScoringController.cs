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
    private ScoringPlane.ScoreColour currentFootageColour;

    //
    ////    Scoring System 02    ////
    //

    [Space(5)]
    [Header("Scoring System 02")]
    public VideoPlayer[] broadcastScreen;
    public VideoPlayer[] bleepScreen;
    public BackWallLight myWallLightRed, myWallLightOrange, myWallLightGreen;

    private enum ScoreColour { Red, Orange, Green, Null }
    private ScoreColour[] audioScore = new ScoreColour[4];
    private ScoreColour[] bleepScore = new ScoreColour[4];
    private ScoreColour videoScore;
    private int lastBroadcastFrame, lastBleepFrame;
    private List<ScoringData>[] sequenceScoring = new List<ScoringData>[3];
    private bool isTracking;



    // Use this for initialization
    void Start () {
        myMasterController = GetComponent<MasterController>();
        myVUMeter = FindObjectOfType<AudienceVUMeter>();
        myScoringMode = ScoringMode.SingleCam;
        audiencePercentage = startAudiencePercentage;
        thisAudienceChange = 0;
        lastAudienceChange = 0;

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

    //
    ////    Scoring System 02    ////
    //

    public void SetUpScoreTracker(string seq1, string seq2, string seq3) {

        // Reset All Variables
        videoScore = ScoreColour.Null;
        for (int n=0; n<4; n++)
        {
            audioScore[n] = ScoreColour.Null;
            bleepScore[n] = ScoreColour.Null;
            if (n< 3) { sequenceScoring[n].Clear(); }
        }
        myWallLightGreen.LightOff();
        myWallLightOrange.LightOff();
        myWallLightRed.LightOff();
        lastBroadcastFrame = 0;
        lastBleepFrame = 0;

        // Create all Scoring Lists for Level
        TurnTextFileIntoList(seq1, 0);
        TurnTextFileIntoList(seq2, 1);
        TurnTextFileIntoList(seq3, 2);

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
}
