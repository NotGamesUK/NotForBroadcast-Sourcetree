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

    public float broadcastScreenDelayTime=2;

    public LevelData[] levelData;

    [HideInInspector]
    public float masterLevelClock;

    private List<InterferenceLog> levelInterferenceLog = new List<InterferenceLog>();
    private VideoClip nextAdvert;
    private AudioClip nextAdvertAudio;
    private SequenceController mySequenceController;
    private BroadcastTV myBroadcastScreen;
    private GUIController myGUIController;
    private EDLController myEDLController;
    private VisionMixer myVisionMixer;
    private BackWallClock myClock;
    [HideInInspector]
    public int currentLevel;
    private int currentSequence;
    private LevelData myLevelData;
    public enum MasterState { Menu, StartLevel, WaitingForPlayer, PreparingAd, PlayingAd, Active, PostRoll, EndOfLevel, FailLevel, Paused }
    public MasterState myState;
    [HideInInspector]
    public bool preparingAd, overRunning;
    private float overrunTime, startPreRollTime;

    // Use this for initialization
    void Start () {
        mySequenceController = GetComponent<SequenceController>();
        myEDLController = GetComponent<EDLController>();
        myBroadcastScreen = FindObjectOfType<BroadcastTV>();
        myGUIController = FindObjectOfType<GUIController>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myClock = FindObjectOfType<BackWallClock>();
        myState = MasterState.Menu;
	}

    public void StartBroadcast(int thisLevel)
    {
        PrepareLevel(thisLevel);

    }
	
	// Update is called once per frame
	void Update () {

        // Monitor Pause Button and if pressed -
        // call PauseGame()


        switch (myState)
        {

            case MasterState.Menu:
                // If in Menu State
                // Wait for command from menu system to Start the Level
                // Allow currentLevel to be set by menu system
                break;

            case MasterState.StartLevel:
                // If in Opening Titles State
                // Fade "DAY ???" title
                // When done call PreLevel()

                PreLevel();

                break;

            case MasterState.WaitingForPlayer:

                // If in Waiting State
                // Check if Condition is met
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    PrepareAdvert(myLevelData.preRollSmaller, myLevelData.preRollAudio);
                }
                // Make relevant action (Send Fax, Phone Call, Display/Clear Tutorial text, etc)
                // Move to next condition
                // If at end of list of Pre-Conditions StartLevel
                break;

            case MasterState.PreparingAd:

                // Wait for Advert to finish preparing then play ad and start clock
                if (!preparingAd)
                {
                    Debug.Log("MASTER CONTROLLER AD PREPARED - PLAYING.");
                    // Play Ad, start clock
                    myBroadcastScreen.PlayAdvert();
                    myClock.StartClock();
                    if (currentSequence == 0)
                    {
                        StartLevel();
                    }
                    myState = MasterState.PlayingAd;
                    if (currentSequence > 2)
                    {
                        // Stop Scoring System
                        // Copy EDL into Array
                        // Save All EDLs and Interference Log
                        myState = MasterState.PostRoll;
                    }
                    startPreRollTime = -1;
                }
                break;

            case MasterState.PlayingAd:

                // If in AdBreak State
                // Do InGameManagement();
                InGameManagement();
                // Wait for Vision Mixer to announce end of post roll then
                if (!myVisionMixer.inPostRoll && startPreRollTime == -1)
                {
                    startPreRollTime = mySequenceController.PrepareSequence(myLevelData.sequenceNames[currentSequence]);
                    Debug.Log("Intending to start sequence when clock hits " + startPreRollTime +" seconds.");

                } else if (startPreRollTime>0)  
                {  

                    if (myClock.clockTime <= startPreRollTime) // When MasterClock is at top of Preroll Start Sequence
                    {
                        mySequenceController.StartSequence();
                        startPreRollTime = 0;
                    }
                }

                
                break;

            case MasterState.Active:

                // If in Active State
                InGameManagement();
                // If Overrunning
                if (overRunning)
                {
                    // Decrement overrunTime;
                    overrunTime -= Time.deltaTime;
                    // If it hits Zero there is no signal and the level is failed (NO ADVERT PLAYED)
                    if (overrunTime <= 0)
                    {
                        FailMidLevel("Didn't Play Advert before studio signal cut out.");
                    }
                }
                break;

            case MasterState.PostRoll:

                // If in PostLevel State
                // Wait for ENTER keypress or end of advert after PostRoll then:
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    LevelComplete();
                }

                // Wait Until OK is pressed on Scoreboard then activate "Upgrade/Repair Menu" (this might be handled from GUI Controller)
                break;


            case MasterState.EndOfLevel:

                // If in EndOfLevel State
                // Wait for gui to change state
                break;

            case MasterState.FailLevel:

                // If in FailLevel State
                // Bring up lights
                // Wait for gui to change state
                break;

            case MasterState.Paused:

                // If in Paused State
                // Wait for gui to send resume then call ResumeGame();
                break;


            default:

                Debug.LogError("MASTER CONTROLLER NOT IN VALID STATE.");

                break;
        }

    }

    void InGameManagement()
    {
        // Advance Master Clock
        // Monitor and trigger game events

    }

    public void ClockAtZero()
    {
        switch (myState)
        {
            case MasterState.Active:
                // Player Is Overrunning
                Debug.Log("MASTER CONTROLLER: CLOCK AT ZERO - PLAYER IS OVER-RUNNING");
                // Player is OverRunning ad point
                // Sound Overrun Alarm
                overRunning = true;
                overrunTime = mySequenceController.mySequence.runOut;

                break;

            case MasterState.PlayingAd:

                // Advert is Over
                ////Debug.Log("ClockAtZero Called.");
                Debug.Log("MASTER CONTROLLER: CLOCK AT ZERO ON ADVERT.");
                mySequenceController.GoLive();
                myState = MasterState.Active;
                break;

            case MasterState.PostRoll:
                Debug.Log("MASTER CONTROLLER: End of Last Advert.  Display End of Level Gui.");
                LevelComplete();
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
        myState = MasterState.StartLevel;
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
            myBroadcastScreen.PlayPreLoop(myLevelData.loopRollSmaller, myLevelData.loopRollAudio);
            // Enter Waiting State
            myState = MasterState.WaitingForPlayer;

        } else
        {
            PrepareAdvert(myLevelData.preRollSmaller, myLevelData.preRollAudio);
            myState = MasterState.PreparingAd;

        }



    }

    void PrepareAdvert(VideoClip thisAdvert, AudioClip thisAdvertAudio)
    {
        Debug.Log("MASTER CONT: PREPARING ADVERT - " + thisAdvert);
        // Set clock to Ad-Length second countdown minus broadcast delay - First Ad is always "Later tonight...."?
        float thisAdLength = (float)thisAdvert.length;
        myClock.SetTimeAndHold(thisAdLength - broadcastScreenDelayTime, true);

        // Prepare Ad (or countdown if at start) on Broadcast screen
        myBroadcastScreen.PrepareAdvert(thisAdvert, thisAdvertAudio);
        preparingAd = true;

        myState = MasterState.PreparingAd;

    }


    void StartLevel()
    {
        Debug.Log("MASTER CONT: INITIALISING EVENT MONITORING, INTERFERNECE LOGGING, AND MASTER CLOCK.");
        // Begin recording Interference (this is done from elsewhere as a consequence of setting mode to Active
        // Begin running Game Events
        // Start Master Level Clock
        masterLevelClock = 0;
        // Tell Sequence Controller to start first sequence
    }

    public void SequenceComplete(VideoClip thisAdvert, AudioClip thisAdvertAudio)
    {
        // Advance Sequence Count
        currentSequence++;


        //TESTING: REMOVE -------------------
        currentSequence = 4;
        // ----------------------------------


        // Is sequence count less than 4?
        if (currentSequence < 3)
        {
            Debug.Log("MASTER CONTROLLER SEQUENCE COMPLETE.  NEXT SEQUENCE:" + myLevelData.sequenceNames[currentSequence]);
            // Yes - 
            // Copy EDL into Array for storage before Sequence Controller wipes it
            // Put the Vision Mixer into ResetSystem Mode
            myVisionMixer.inPostRoll = true;

            // Enter Ad Break Mode
            PrepareAdvert(thisAdvert, thisAdvertAudio);
        }
        else
        {
            // No -
            Debug.Log("PLAYING FINAL AD");
            // Play final Ad
            myVisionMixer.inPostRoll = true;
            PrepareAdvert(thisAdvert, thisAdvertAudio);
            // Call LevelComplete()
            LevelComplete();
        }


    }

    void LevelComplete()
    {
        // Enter PostLevel State
        myState = MasterState.EndOfLevel;
        // Tell GUI to Display Scorecard, Update/Repair Sequence, and End Level GUI - Watch Broadcast, Replay, Continue.  
        myGUIController.GoToSuccess();
        

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

    public void FailMidLevel(string thisReason)
    {
        Debug.Log("LEVEL FAILED!  Reason: "+thisReason);
        // Called by Scoring System
        // Also Called by Sequence Controller when Ad not played by end of video

        // Stop all Video and Audio
        // Stop all recording of data

        StopAllAudioAndVideo();
        // Play tapeSlowdownSound
        // Fade Screen to black
        // Tell GUI to show Level Failed Menu
        myGUIController.GoToFailed(thisReason);
        myState = MasterState.FailLevel;
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
    }
}
