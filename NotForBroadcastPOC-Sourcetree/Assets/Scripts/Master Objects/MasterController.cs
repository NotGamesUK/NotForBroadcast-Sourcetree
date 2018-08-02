using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MasterController : MonoBehaviour {


    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public int dayNumber;
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

    public static MasterController uniqueMasterController;


    public float broadcastScreenDelayTime=2;

    public LevelData[] levelDataArray;

    [HideInInspector]
    public float masterLevelClock;

    private List<InterferenceLog> levelInterferenceLog = new List<InterferenceLog>();
    private VideoClip nextAdvert;
    private AudioClip nextAdvertAudio;
    private SequenceController mySequenceController;
    private BroadcastTV myBroadcastScreen;
    private GUIController myGUIController;
    private EDLController myEDLController;
    private ScoringController myScoringController;
    private PlaybackRoomController myPlaybackController;
    private VisionMixer myVisionMixer;
    private BackWallClock myClock;
    private GodOfTheRoom myRoomGod;
    [HideInInspector]
    public int currentLevel;
    private int currentSequence;
    private LevelData myLevelData;
    public enum MasterState { Menu, StartLevel, WaitingForPlayer, PreparingAd, PlayingAd, Active, PostRoll, EndOfLevel, FailLevel, Paused }
    public MasterState myState;
    [HideInInspector]
    public bool preparingAd, overRunning, inDevMode;
    private float overrunTime, startPreRollTime;
    private List<EditDecision>[] broadcastEDL = new List<EditDecision>[3];
    public List<String> savedFiles = new List<string>();
    private int myDayNumber;
    private string myLevelName;

    private int decisionCount;

    private void Awake()
    {
        if (uniqueMasterController == null)
        {
            DontDestroyOnLoad(gameObject);
            uniqueMasterController = this;
        }
        else if (uniqueMasterController != this)
        {
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start () {
        mySequenceController = GetComponent<SequenceController>();
        myEDLController = GetComponent<EDLController>();
        myBroadcastScreen = FindObjectOfType<BroadcastTV>();
        myGUIController = FindObjectOfType<GUIController>();
        myScoringController = GetComponent<ScoringController>();
        myPlaybackController = FindObjectOfType<PlaybackRoomController>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myClock = FindObjectOfType<BackWallClock>();
        myRoomGod = FindObjectOfType<GodOfTheRoom>();
        myState = MasterState.Menu;
        broadcastEDL[0] = new List<EditDecision>();
        broadcastEDL[1] = new List<EditDecision>();
        broadcastEDL[2] = new List<EditDecision>();
        LoadFileList();
    }

    public void StartBroadcast(int thisLevel)
    {
        PrepareLevel(thisLevel);
        myRoomGod.SwitchScreensTo3DSound();
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

    public void GetLevelInfoForPlayback(int thisLevel)
    {
        Debug.Log("Trying to Get Playback Info for Level " + thisLevel);
        myPlaybackController.myLevelData = levelDataArray[thisLevel-1];
    }

    void PrepareLevel(int thisLevel)
    {
        // Set Current Level to thisLevel;
        currentLevel = thisLevel;
        myLevelData = levelDataArray[currentLevel - 1];
        // Set Current Sequence to 0
        currentSequence = 0;
        // Display "DAY ???" and Title of Level - On black with suitable sound
        myDayNumber = myLevelData.dayNumber;
        myLevelName = myLevelData.levelName;
        Debug.Log("MC: Day " + myDayNumber+".  "+myLevelName+".");
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
        // Begin recording Interference (this is done from elsewhere as a consequence of setting mode to Active)
        // Begin running Game Events
        // Start Master Level Clock
        masterLevelClock = 0;
        // Tell Sequence Controller to start first sequence
        // StartVUBar
        myScoringController.InitialiseVU();
    }

    public void SequenceComplete(VideoClip thisAdvert, AudioClip thisAdvertAudio)
    {
        Debug.Log("Sequence Controller: Advert hit with " + myClock.clockTime + " seconds remaining.");
        // Has the player gone to AD too early?
        if (myClock.clockTime > 2)
        {
            FailMidLevel("Played Advert Too Early");
        }
        else
        {

            SortAndSaveEDL();
            // Advance Sequence Count
            currentSequence++;
            overRunning = false;

            //TESTING: REMOVE -------------------
            //currentSequence = 4;
            // ----------------------------------


            // Is sequence count less than 4?
            if (currentSequence < 3)
            {
                Debug.Log("MASTER CONTROLLER SEQUENCE COMPLETE.  NEXT SEQUENCE:" + myLevelData.sequenceNames[currentSequence]);
                // Yes - 

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


    }

    void SortAndSaveEDL()
    {
        // Copy EDL into Array for storage before Sequence Controller wipes it
        foreach (EditDecision thisEdit in myEDLController.EDL)
        {
            broadcastEDL[currentSequence].Add(thisEdit);
        }
        broadcastEDL[currentSequence].Sort();
        for (int n=0; n<=currentSequence; n++)
        {
            // DEBUG - List sorted EDLs            
            decisionCount = 1;
            Debug.Log("This is EDL[" + n + "]");
            foreach (EditDecision thisEdit in broadcastEDL[n])
            {
                print("Sorted Time: " + thisEdit.editTime + "  Decision " + decisionCount + ": " + thisEdit.editType);
                decisionCount++;
            }

        }

    }

    void LevelComplete()
    {
        // Enter PostLevel State
        myState = MasterState.EndOfLevel;
        // Tell GUI to Display Scorecard, Update/Repair Sequence, and End Level GUI - Watch Broadcast, Replay, Continue.  
        myGUIController.GoToSuccess();
        myClock.SetTimeAndHold(0, true);
        myScoringController.broadcastScreensLive = false;
        // LOAD AND SAVE TEST
        DateTime theTime = DateTime.Now;
        string date = theTime.ToString("ddXXMMXXyy");
        string time = theTime.ToString("HHCCmm");
        string saveFileName = ("Day " + myDayNumber + "CC " + myLevelName + " BR GradeCC B+ BR "+date + " - " + time);

        Debug.Log("Saving File with Name: "+saveFileName);
        SaveBroadcast(saveFileName);
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

    void SaveFileList()
    {
        BinaryFormatter myFormatter = new BinaryFormatter();
        FileStream thisOpenedFile = File.Open(Application.persistentDataPath + "/BroadcastArchive.dat", FileMode.Create);
        myFormatter.Serialize(thisOpenedFile, savedFiles);
        thisOpenedFile.Close();
        Debug.Log("SAVING FILE LIST");

    }


    void LoadFileList()
    {
        if (File.Exists(Application.persistentDataPath + "/BroadcastArchive.dat"))
        {
            BinaryFormatter myFormatter = new BinaryFormatter();
            FileStream thisOpenedFile = File.Open(Application.persistentDataPath + "/BroadcastArchive.dat", FileMode.Open);
            savedFiles.Clear();
            savedFiles = (List<String>)myFormatter.Deserialize(thisOpenedFile);
            thisOpenedFile.Close();
        }
        else
        {
            Debug.Log("NO FILE LIST EXISTS");
        }

    }

    void SaveBroadcast(string thisSaveName)
    {
        //// DEBUG - LIST ALL BROADCAST EDLS BEFORE SAVING
        //for (int n = 0; n < 3; n++)
        //{
        //    decisionCount = 1;
        //    Debug.Log("This is Broadcast EDL[" + n + "] TO SAVE");
        //    foreach (EditDecision thisEdit in broadcastEDL[n])
        //    {
        //        Debug.Log("BROADCAST EDL Time: " + thisEdit.editTime + "  Decision " + decisionCount + ": " + thisEdit.editType);
        //        decisionCount++;
        //    }
        //}

        BinaryFormatter myFormatter = new BinaryFormatter();
        FileStream thisOpenedFile = File.Open(Application.persistentDataPath + "/"+thisSaveName+".dat", FileMode.Create);

        BroadcastSaveData thisSaveData = new BroadcastSaveData();
        thisSaveData.levelNumber = currentLevel;
        for (int n = 0; n < 3; n++)
        {
            thisSaveData.savedEDL[n] = broadcastEDL[n];

            //////// DEBUG LOGGING:
            // DEBUG - List saved EDL            

            //Debug.Log("This is Save EDL[" + n + "]");
            //decisionCount = 1;
            //foreach (EditDecision thisEdit in thisSaveData.savedEDL[n])
            //{
            //    Debug.Log("SAVED Time: " + thisEdit.editTime + "  Decision " + decisionCount + ": " + thisEdit.editType);
            //    decisionCount++;
            //}
            ///////////////////////////////////

        }



        myFormatter.Serialize(thisOpenedFile, thisSaveData);
        thisOpenedFile.Close();
        Debug.Log("Adding File: "+thisSaveName+" to List of names.");
        savedFiles.Add(thisSaveName);
        Debug.Log("Calling SaveFileList.");
        SaveFileList();

    }

    void LoadBroadcast(string thisLoadName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + thisLoadName + ".dat"))
            {
            BinaryFormatter myFormatter = new BinaryFormatter();
            FileStream thisOpenedFile = File.Open(Application.persistentDataPath + "/" + thisLoadName + ".dat", FileMode.Open);
            BroadcastSaveData thisLoadedData = (BroadcastSaveData)myFormatter.Deserialize(thisOpenedFile);
            thisOpenedFile.Close();

            currentLevel=thisLoadedData.levelNumber;
            for (int n = 0; n <3; n++)
            {
                broadcastEDL[n]= thisLoadedData.savedEDL[n];

                ////////// DEBUG LOGGING:
                //// DEBUG - List sorted EDL            
                //int decisionCount = 1;
                //Debug.Log("This is Loaded EDL[" + n + "]");
                //foreach (EditDecision thisEdit in broadcastEDL[n])
                //{
                //    print("LOADED Time: " + thisEdit.editTime + "  Decision " + decisionCount + ": " + thisEdit.editType);
                //    decisionCount++;
                //}
                ///////////////////////////////////

            }

        }
    }
}

[Serializable]
class BroadcastSaveData
{
    public int levelNumber;
    public List<EditDecision>[] savedEDL = new List<EditDecision>[3];

}
