using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SequenceController : MonoBehaviour {

    [Tooltip("Number of seconds delay between broadcast screen and master screen")]
    public float broadcastScreenDelayTime;
    public string sequenceName;
    //[HideInInspector]
    public int preparedScreensCount, targetScreensCount;
    public VideoClip TEMPPreRollVideo;
    public AudioClip TEMPPreRollAudio;


    private DataStorage myDataStore;
    private MasterController myMasterController;
    private EDLController myEDLController;
    private VisionMixer myVisionMixer;
    private InterferenceSystem myInterferenceSystem;
    private BackWallClock myClock;
    private BroadcastTV myBroadcastScreen;
    private VHSControlPanel myVHSControlPanel;
    private bool waitingForScreens = false;
    private float thisPreSequenceVideoLength;
    private int foundPos;
    [HideInInspector]
    public bool preRollReady;
    private bool isPlayingAd;
    private bool overrunning;
    float overrunTime;
    private double thisSequenceVideoLength;
    private enum GameTypes { Headlines, Live }
    private GameTypes myGameType;
    



    // Use this for initialization
    void Start()
    {
        myDataStore = GetComponent<DataStorage>();
        myMasterController = GetComponent<MasterController>();
        myEDLController = GetComponent<EDLController>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myInterferenceSystem = FindObjectOfType<InterferenceSystem>();
        myClock = FindObjectOfType<BackWallClock>();
        myBroadcastScreen = FindObjectOfType<BroadcastTV>();
        myVHSControlPanel = FindObjectOfType<VHSControlPanel>();
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
                isPlayingAd = true;
                preRollReady = false;
                waitingForScreens = false;
                // Invoke StartSequence in Ad-length Seconds minus Run-In Seconds
                Invoke("StartSequence", thisPreSequenceVideoLength - myDataStore.sequenceData[foundPos].runIn - broadcastScreenDelayTime);
            }

        }

        if (overrunning)
        {
            // Player should have played Advert, we are broadcasting PostRoll.
            overrunTime -= Time.deltaTime;
            // Flash Screen Border Red (or some such)

            // Has Time run out?
            if (overrunTime<=0)
            {
                myMasterController.FailMidLevel();
                overrunning = false;
            }
        }
    }

    public void PrepareSequence(string thisSequenceName, VideoClip thisAdvert, AudioClip thisAdvertAudio)
    {
        foundPos = -1;
        int thisLength = myDataStore.sequenceData.Length;
        //print("Searching For Sequence: " + thisSequenceName);
        for (int n = 0; n < thisLength; n++)
        {
            if (thisSequenceName == myDataStore.sequenceData[n].SequenceName)
            {
                foundPos = n;
                //print("FOUND at Position " + foundPos);
            }
        }
        if (foundPos == -1)
        {
            Debug.LogError("Cannot Find Sequence with name: " + thisSequenceName);
            return; // BREAK OUT OF LOAD LOOP EARLY AS THERE IS NO SEQUENCE HERE
        }
        sequenceName = thisSequenceName;
        // Set prepared screens count to 0
        preparedScreensCount = 0;
        // Tell each VM screen and Broadcast Screens to prepare and increment targetcount AND Tell AudioInterference to Prepare (send Null as third parameter if no AudioInterference required at start)
        myVisionMixer.PrepareScreens(myDataStore.sequenceData[foundPos].screenVideo, myDataStore.sequenceData[foundPos].screenAudio);
        myBroadcastScreen.PrepareScreens(myDataStore.sequenceData[foundPos].screenVideo, myDataStore.sequenceData[foundPos].screenAudio, myDataStore.sequenceData[foundPos].AudioInterference);
        targetScreensCount = myDataStore.sequenceData[foundPos].screenVideo.Length*2;

        // Get Length of Video for Screen 01
        thisSequenceVideoLength= myDataStore.sequenceData[foundPos].screenVideo[0].length;
        //Debug.Log("Returned Length: " + thisSequenceVideoLength + " seconds.");

        // Set clock to Ad-Length second countdown minus broadcast delay - First Ad is always "Later tonight...."?
        thisPreSequenceVideoLength = (float)thisAdvert.length;
        myClock.SetTimeAndHold(thisPreSequenceVideoLength-broadcastScreenDelayTime, true);

        // Prepare Ad (or countdown if at start) on Broadcast screen
        myBroadcastScreen.PrepareAdvert(thisAdvert, thisAdvertAudio);
        preRollReady = false;

        // Tell Resistance Screen to prepare (and increment targetcount if required)
        myBroadcastScreen.PrepareResistance(myDataStore.sequenceData[foundPos].resistanceVideo, myDataStore.sequenceData[foundPos].resistanceAudio);

        

        // Load Level into Level Controller
        myInterferenceSystem.SpawnLevel(myDataStore.sequenceData[foundPos].interferenceLevel.transform);

        // Tell update to begin monitoring prepared screens count until target is reached - all ready
        waitingForScreens = true;

    }

    void StartSequence()
    {
        //Debug.Log("STARTING SCREENS");
        // if TARGET COUNT=PREPARED COUNT:
        // Send GO! to VM Screens
        myVisionMixer.PlayScreens();
        // Invoke GO! on StartBroadcastScreens with delay of broadcastScreenDelayTime;
        Invoke("StartBroadcastScreens", broadcastScreenDelayTime);
        // Set LevelController.LevelHasStarted
        myInterferenceSystem.StartLevel();
        // Send GO to Resistance screen
        myBroadcastScreen.PlayResistance();
        // Set clock to sequence length minus overspill
        overrunning = false;
        // IF NOT - INVOKE StartSequence AGAIN IN 0.1 SECONDS

        // Start EDL
        myEDLController.StartRecordingEDL();
        // Make VHS Control Panel Play Button Active again
        myVHSControlPanel.TapeComplete();


    }

    void StartBroadcastScreens()
    {
        //Debug.Log("SEQUENCE CONTROLLER: STARTING BROADCAST SCREENS");
        // Send GO to Broadcast Screens
        myBroadcastScreen.PlayScreens();

        // Set Mode to follow EDL
    }

    public void ClockAtZero() // CALLED BY WALL CLOCK WHEN TIMER REACHES ZERO

    // If playing an Advert Tell Broadcast System to End Advert and Start Sequence after broadcastscreen delay
    {
        ////Debug.Log("ClockAtZero Called.");
        if (isPlayingAd)
        {
            Debug.Log("CLOCK AT ZERO ON ADVERT.");
            Invoke("SwitchBroadcastSystemToLive", broadcastScreenDelayTime);
            myClock.SetTimeAndHold((float)thisSequenceVideoLength - myDataStore.sequenceData[foundPos].runIn - myDataStore.sequenceData[foundPos].runOut +broadcastScreenDelayTime, false);
        } else {
            if (!overrunning)
            {
                Debug.Log("PLAYER IS OVER-RUNNING");
                // Player is OverRunning ad point
                // Sound Overrun Alarm
                overrunning = true;
                overrunTime = myDataStore.sequenceData[foundPos].runOut;
            } 
        }

    }

    void SwitchBroadcastSystemToLive ()
    {
        //Debug.Log("SEQUENCE CONTROLLER: Switching Broadcast System to Live Video.");
        myBroadcastScreen.EndAdvertAndStartLiveBroadcast();
        isPlayingAd = false;
    }

    void SequenceOverrun()
    {
        // THIS IS CALLED WHEN SEQUENCE HAS CUT TO BREAK AND AD HAS NOT BEEN PLAYED
        // Sound Alarms (fringe screen in red? flash lights? siren?)
        // If Player does not play in the AD after X Seconds the level is failed
    }

    public void EndSequenceAndPlayAdvert(string thisAdvertName)
    {
        // THIS IS CALLED BY VHS CONTROL PANEL WHEN PLAY BUTTON IS PRESSED
        // Log Advert Choice to EDL for Storage
        myEDLController.AddAdvert(thisAdvertName);
        // Tell MasterController that the sequence has ended and also which Advert has been selected to play.
        VHSTape[] theseVHSTapes = FindObjectsOfType<VHSTape>();
        VideoClip thisAdvertVideo = null;
        AudioClip thisAdvertAudio = null;
        overrunning = false;
        foreach (VHSTape thisVHS in theseVHSTapes)
        {
            if (thisVHS.myTitle == thisAdvertName)
            {
                thisAdvertVideo = thisVHS.myVideoSmaller;
                thisAdvertAudio = thisVHS.myAudio;
            }
        }
        if (thisAdvertVideo)
        {
            myMasterController.SequenceComplete(thisAdvertVideo, thisAdvertAudio);

        } else
        {
            Debug.Log("CANNOT FIND VIDEO CLIP FOR VHSTape: " + thisAdvertName);
        }
    }
}
