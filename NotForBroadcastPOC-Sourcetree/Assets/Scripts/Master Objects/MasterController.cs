using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MasterController : MonoBehaviour {


    [System.Serializable]
    public class LevelData
    {
        public int dayNumber;
        public string levelName;
        [Tooltip ("Leave this blank to jump straight to main pre-roll")]
        public VideoClip loopRoll;
        public VideoClip loopRollSmaller;
        public AudioClip loopRollAudio;
        public VideoClip preRoll;
        public VideoClip preRollSmaller;
        public AudioClip preRollAudio;
        public string[] sequenceNames = new string[3];
        public List<GameEvent> gameEvents;

        public string GetName()
        {
            return levelName;
        }

        public string GetSequenceName(int thisSequence)
        {
            return sequenceNames[thisSequence];
        }

    }

    public LevelData[] levelData;

    [HideInInspector]
    public float masterLevelClock;

    private List<InterferenceLog> levelInterferenceLog = new List<InterferenceLog>();

    private SequenceController mySequenceController;
    private BroadcastTV myBroadcastTV;
    private EDLController myEDLController;
    private int currentLevel;
    private int currentSequence;
    private LevelData myLevelData;

    public enum MasterState { Menu, OpeningTitles, Waiting, Active, PostLevel, Paused }
    public MasterState myState;

    // Use this for initialization
    void Start () {
        mySequenceController = GetComponent<SequenceController>();
        myEDLController = GetComponent<EDLController>();
        myBroadcastTV = FindObjectOfType<BroadcastTV>();
        myState = MasterState.Menu;
        Invoke("TEMPStartGame", 2);
	}

    void TEMPStartGame()
    {
        PrepareLevel(1);
    }
	
	// Update is called once per frame
	void Update () {

        switch (myState)
        {

            case MasterState.Menu:
                // If in Menu State
                // Wait for command from menu system to Start the Level
                // Allow currentLevel to be set by menu system
                break;

            case MasterState.OpeningTitles:
                // If in Opening Titles State
                // Fade "DAY ???" title
                // When done call PreLevel()
                PreLevel();
                break;

            case MasterState.Waiting:

                // If in Waiting State
                // Check if Condition is met
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    StartLevel();
                }
                // Make relevant action (Send Fax, Phone Call, Display/Clear Tutorial text, etc)
                // Move to next condition
                // If at end of list of Pre-Conditions StartLevel
                break;

            case MasterState.Active:

                // If in Active State
                // Advance Master Level Clock
                // Monitor and trigger game events
                // Monitor Pause Button and if pressed -
                // call PauseGame()
                break;

            case MasterState.PostLevel:

                // If in PostLevel State
                // Wait Until OK is pressed on Scoreboard then activate "Upgrade/Repair Menu" (this might be handled from GUI Controller
                break;

            case MasterState.Paused:

                // If in Paused State
                // Wait for menu system to send resume then call ResumeGame();
                break;

            default:

                Debug.LogError("MASTER CONTROLLER NOT IN VALID STATE.");

                break;
        }

    }

    void PrepareLevel(int thisLevel)
    {
        // Set Current Level to thisLevel;
        currentLevel = thisLevel;
        myLevelData = levelData[currentLevel - 1];
        // Set Current Sequence to 0
        currentSequence = 0;
        // Display "DAY ???" and Title of Level - On black with suitable sound
        int thisDay = myLevelData.dayNumber;
        string thisLevelName = myLevelData.levelName;
        Debug.Log("MC: Day " + thisDay+".  "+thisLevelName+".");
        // Set Mode to OpeningTitles
        myState = MasterState.OpeningTitles;
        // Mute Ambient Sound (But not "DAY ???" SFX).
        // Read RoomState info array and set objects to match
            // THIS INCLUDES: Power switches, FanLock, VM Link Switch, 
        // Tell Scoring System to Initialise

    }

    void PreLevel()
    {
        // Is there an indefinite loop clip?
        if (myLevelData.loopRoll)
        {
            // YES - Start Loop Clip
            myBroadcastTV.PlayPreLoop(myLevelData.loopRollSmaller, myLevelData.loopRollAudio);
            // Enter Waiting State
            myState = MasterState.Waiting;

        }

        // NO - Call StartLevel


    }

    void StartLevel()
    {
        // Begin recording Interference (this is done from elsewhere as a consequence of setting mode to Active
        // Begin running Game Events
        // Start Master Level Clock
        masterLevelClock = 0;
        // Tell Sequence Controller to start first sequence
        Debug.Log("Preparing Sequence: " + myLevelData.sequenceNames[currentSequence]);
        mySequenceController.PrepareSequence(myLevelData.sequenceNames[currentSequence], myLevelData.preRollSmaller, myLevelData.preRollAudio);
        // Set currentSequence to 1
        currentSequence = 1;
        // Enter Active State
        myState = MasterState.Active;
    }

    public void SequenceComplete(VideoClip thisAdvert, AudioClip thisAdvertAudio)
    {
        // Advance Sequence Count
        currentSequence++;
        // Is sequence count less than 4?
        if (currentSequence < 4)
        {
            Debug.Log("STARTING SEQUENCE " + myLevelData.sequenceNames[currentSequence - 1]);
            // Yes - 
            // Copy EDL into Array for storage before Sequence Controller wipes it
            // Pass Ad to Sequence Controller and begin next sequence
            mySequenceController.PrepareSequence(myLevelData.sequenceNames[currentSequence-1], thisAdvert, thisAdvertAudio);


        }
        else
        {
            // No -
            Debug.Log("BROADCAST COMPLETE");
            // Play final Ad
            // Call LevelComplete()

        }


    }

    void LevelComplete()
    {
        // Enter PostLevel State
        // Stop Scoring System
        // Copy EDL into Array
        // Save All EDLs and Interference Log
        // Tell GUI to Display Scorecard, Update/Repair Sequence, and End Level GUI - Watch Broadcast, Replay, Continue.  


    }

    void PauseGame()
    {
        // Pause all Audio and Video
        // Pause EDL Clock
        // Tell GUI to display and handle Pause menu
        // Enter paused State
    }

    void ResumeGame()
    {
        // Resume any paused Audio and Video
        // Resume EDL Clock
        // Enter Active State
    }

    public void FailMidLevel()
    {
        Debug.Log("LEVEL FAILED!!!!");
        // Called by Scoring System
        // Also Called by Sequence Controller when Ad not played by end of video

        // Stop all Video and Audio
        StopAllAudioAndVideo();
        // Play tapeSlowdownSound
        // Fade Screen to black
        // Tell GUI to show Level Failed Menu
        myState = MasterState.Menu;
    }

    void StopAllAudioAndVideo()
    {
        VideoPlayer[] theseVideoPlayers = FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer thisPlayer in theseVideoPlayers)
        {
            thisPlayer.Stop();
        }
        AudioSource[] theseAudioPlayers = FindObjectsOfType<AudioSource>();
        foreach (AudioSource thisPlayer in theseAudioPlayers)
        {
            if (thisPlayer.outputAudioMixerGroup!=null)
            {
                thisPlayer.Stop();

            }
        }
        NoSignal[] theseNoSignals = FindObjectsOfType<NoSignal>();
        foreach (NoSignal thisNoSignal in theseNoSignals)
        {
            thisNoSignal.GetComponent<MeshRenderer>().enabled = true;
        }

    }
}
