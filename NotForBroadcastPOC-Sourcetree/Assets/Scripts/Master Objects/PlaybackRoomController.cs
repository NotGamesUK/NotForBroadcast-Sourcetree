using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class PlaybackRoomController : MonoBehaviour {

    public Text myFilenameDisplay;
    public AudioSource mySFXPlayer;
    public AudioClip myLeftRightSFX;
    public Animator myCameraAnimator;

    [HideInInspector]
    public bool preparingAd;

    private MasterController myMasterController;
    private BroadcastTV myBroadcastSystem;
    private SoundDesk myMixingDesk;
    private GUIController myGUIController;
    private GodOfTheRoom myRoomGod;
    private DataStorage myDataStore;
    private DataStorage.SequenceData mySequence;
    private int myFileListPosition, maxFileListPosition;
    private string currentFilename, nextSequenceName;

    private List<EditDecision>[] playbackEDL = new List<EditDecision>[3];
    private List<EditDecision> currentPlaybackEDL = new List<EditDecision>();

    private enum PlaybackMode { Menu, Pre, Advert, Playback, Post }
    private PlaybackMode myMode;
    private int currentLevel, currentSequence, currentEDLPosition, maxEDLPosition;
    private float adCountdown, nextAdCountdown, playPreRollTime, sequenceClock, masterClock;
    private bool sequenceStarted, goneLive, zoomingOut;
    public MasterController.LevelData myLevelData;


    // Use this for initialization
    void Start () {
        myMasterController = FindObjectOfType<MasterController>();
        myBroadcastSystem = FindObjectOfType<BroadcastTV>();
        myMixingDesk = FindObjectOfType<SoundDesk>();
        myGUIController = GetComponent<GUIController>();
        myDataStore = FindObjectOfType<DataStorage>();
        myRoomGod = FindObjectOfType<GodOfTheRoom>();
        myMode = PlaybackMode.Menu;
        playbackEDL[0] = new List<EditDecision>();
        playbackEDL[1] = new List<EditDecision>();
        playbackEDL[2] = new List<EditDecision>();
        currentPlaybackEDL = new List<EditDecision>();

    }

    // Update is called once per frame
    void Update () {
		switch (myMode) {
        

            case PlaybackMode.Menu:

                // Do Nothing??

                break;

            case PlaybackMode.Pre:
                if (!preparingAd)
                {
                    StartSequencePlayback();
                    myMode = PlaybackMode.Advert;
                    currentPlaybackEDL = playbackEDL[currentSequence];
                    maxEDLPosition = currentPlaybackEDL.Count;
                    Debug.Log("MaxEDLPosition=" + maxEDLPosition);
                }
                


                break;

            case PlaybackMode.Advert:

                // Adjust Clocks
                masterClock += Time.deltaTime;
                adCountdown -= Time.deltaTime;

                // Wait for Preroll secs then start screens and EDL playback
                if (adCountdown<=playPreRollTime && !sequenceStarted)
                {
                    sequenceStarted = true;
                    sequenceClock = 2f;
                    currentEDLPosition = 0;
                    myMode = PlaybackMode.Playback;
                    Debug.Log("Playback Controller: Starting Broadcast Screens.");
                    myBroadcastSystem.PlayScreens();
                }
                
                break;

            case PlaybackMode.Playback:

                // Adjust Clocks
                sequenceClock += Time.deltaTime;
                masterClock += Time.deltaTime;
                adCountdown -= Time.deltaTime;

                // Monitor for end of Ad then GoLive

                if (adCountdown <= 0 && !goneLive)
                {
                    goneLive = true;
                    adCountdown = 0;
                    Debug.Log("Playback Controller: Going Live.");
                    myBroadcastSystem.EndAdvertAndStartLiveBroadcast();

                }

                FollowEDL();

                break;

            case PlaybackMode.Post:

                adCountdown -= Time.deltaTime;
                if (adCountdown<10 && !zoomingOut)
                {
                    myCameraAnimator.SetTrigger("EndPlayback");
                    zoomingOut = true;
                }
                if (adCountdown < 0)
                {
                    myMode = PlaybackMode.Menu;
                    myGUIController.GoToPlayback();
                    myBroadcastSystem.myNoSignal.enabled = true;
                }


                break;


        }
    }

    void FollowEDL()
    {
        if (currentEDLPosition < maxEDLPosition)
        {
            while (currentPlaybackEDL[currentEDLPosition].editTime <= sequenceClock && currentEDLPosition < maxEDLPosition)
            {
                EditDecision thisEdit = currentPlaybackEDL[currentEDLPosition];
                Debug.Log("POSITION - " + currentEDLPosition + " - REACHED EDIT DECISION: " + thisEdit.editType + " at " + thisEdit.editTime);
                switch (thisEdit.editType)
                {
                    case EditDecision.EditDecisionType.SwitchScreen:

                        myBroadcastSystem.ScreenChange(thisEdit.channelNumber);
                        break;

                    case EditDecision.EditDecisionType.PlayAd:
                        if (currentSequence <= 2)
                        {
                            PrepareNextSequence(thisEdit.loadName);
                        }
                        break;

                    case EditDecision.EditDecisionType.BleepOn:
                        myMixingDesk.BleepOn();
                        break;

                    case EditDecision.EditDecisionType.BleepOff:
                        myMixingDesk.BleepOff();
                        break;


                    case EditDecision.EditDecisionType.MuteChannel:
                        myMixingDesk.MuteChannel(thisEdit.channelNumber);
                        break;

                    case EditDecision.EditDecisionType.UnMuteChannel:
                        myMixingDesk.UnmuteChannel(thisEdit.channelNumber);
                        break;

                }

                currentEDLPosition++;
                if (currentEDLPosition == maxEDLPosition) { break; }

            }
        }

    }

    public void PrepareList()
    {
        myFileListPosition = 0;
        maxFileListPosition = myMasterController.savedFiles.Count-1;
        if (maxFileListPosition != -1)
        {
            Debug.Log("At Position 0 of " + maxFileListPosition);
            for (int n = 0; n <= maxFileListPosition; n++)
            {
                Debug.Log("FileName " + n + ": " + myMasterController.savedFiles[n]);
            }
            myFileListPosition = maxFileListPosition;
            ParseFilenameAndDisplay(myMasterController.savedFiles[myFileListPosition]);
        }
        else
        {
            ParseFilenameAndDisplay("No Finished\nBroadcasts\nReady To View");
        }
    }

    void ParseFilenameAndDisplay(string thisFileName)
    {
        string thisDisplayFilename = thisFileName.Replace(" BR ", "\n");
        thisDisplayFilename = thisDisplayFilename.Replace("XX", "/");
        thisDisplayFilename = thisDisplayFilename.Replace("XX", "/");
        thisDisplayFilename = thisDisplayFilename.Replace("CC", ":");
        myFilenameDisplay.text = thisDisplayFilename;

    }

    public void MoveUpFileList()
    {
        if (myFileListPosition < maxFileListPosition)
        {
            myFileListPosition++;
            ParseFilenameAndDisplay(myMasterController.savedFiles[myFileListPosition]);
            mySFXPlayer.clip = myLeftRightSFX;
            mySFXPlayer.Play();
        }
    }

    public void MoveDownFileList()
    {
        if (myFileListPosition > 0)
        {
            myFileListPosition--;
            ParseFilenameAndDisplay(myMasterController.savedFiles[myFileListPosition]);
            mySFXPlayer.clip = myLeftRightSFX;
            mySFXPlayer.Play();

        }
    }

    public void PreparePlayback()
    {
        LoadBroadcast(myMasterController.savedFiles[myFileListPosition]);
        myMasterController.GetLevelInfoForPlayback(currentLevel);
        currentSequence = 0;
        Debug.Log("Playback Controller: Found Level Data for " + myLevelData.levelName);
        VideoClip thisPreRoll = myLevelData.preRoll;
        AudioClip thisAudio = myLevelData.preRollAudio;
        Debug.Log("Playback Controller: Pre-Roll Video: " + thisPreRoll);
        Debug.Log("Playback Controller: Pre-Roll Audio: " + thisAudio);
        myBroadcastSystem.PrepareAdvert(thisPreRoll,thisAudio);

        // Set Up Room for Playback
        myRoomGod.MuteRoom(); // Make all switches etc silent
        myMixingDesk.ResetMixingDesk();
        myRoomGod.SwitchScreensTo2DSound();
        myRoomGod.TripSwitchPower(true);
        myRoomGod.SetAllPlugs("00110000"); // Turn on Spreakers and Mixing Desk
        myRoomGod.SetMixingDeskChannelSelect(2); // Set to Broadcast Only on Mixing Desk
        myRoomGod.SetBroadcastVolumeSlider(1f); // Turn Volume to full


        myRoomGod.UnMuteRoom(); // Make all switches etc audible




        adCountdown = (float)myLevelData.preRoll.length;
        preparingAd = true;
        sequenceStarted = false;
        goneLive = false;
        myMode = PlaybackMode.Pre;
        myGUIController.StartPlayback();
        myCameraAnimator.SetTrigger("StartPlayback");

    }

    void StartSequencePlayback()
    {
        mySequence = FindSequence(myLevelData.sequenceNames[currentSequence]);
        myBroadcastSystem.PlayAdvert();
        myBroadcastSystem.ResetScreens();

        myBroadcastSystem.PrepareScreens(mySequence.playbackVideo, mySequence.screenAudio, mySequence.AudioInterference);
        myBroadcastSystem.PrepareResistance(mySequence.resistanceVideo, mySequence.resistanceAudio);
        playPreRollTime = mySequence.runIn;
        Debug.Log("PlayBack Controller: Starting broadcast screen sequence in " + (adCountdown - playPreRollTime) + " seconds.");
        myMode = PlaybackMode.Advert;
    }

    DataStorage.SequenceData FindSequence(string thisSequenceName)
    {
        int thisLength = myDataStore.sequenceData.Length;
        int foundPos = -1;
        print("Searching For Sequence: " + thisSequenceName);
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
            return null; // BREAK OUT OF LOAD LOOP EARLY AS THERE IS NO SEQUENCE HERE
        }
        // Make Local Copy of Sequence Data
        return myDataStore.sequenceData[foundPos];

    }

    void LoadBroadcast(string thisLoadName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + thisLoadName + ".dat"))
        {
            BinaryFormatter myFormatter = new BinaryFormatter();
            FileStream thisOpenedFile = File.Open(Application.persistentDataPath + "/" + thisLoadName + ".dat", FileMode.Open);
            BroadcastSaveData thisLoadedData = (BroadcastSaveData)myFormatter.Deserialize(thisOpenedFile);
            thisOpenedFile.Close();

            currentLevel = thisLoadedData.levelNumber;
            for (int n = 0; n < 3; n++)
            {
                playbackEDL[n] = thisLoadedData.savedEDL[n];

                //////// DEBUG LOGGING:
                // DEBUG - List sorted EDL            
                int decisionCount = 1;
                Debug.Log("This is Loaded EDL[" + n + "]");
                foreach (EditDecision thisEdit in playbackEDL[n])
                {
                    print("LOADED Time: " + thisEdit.editTime + "  Decision " + decisionCount + ": " + thisEdit.editType);
                    decisionCount++;
                }
                /////////////////////////////////

            }

        }
        else
        { Debug.Log("FILE NOT FOUND: " + thisLoadName); }
    }

    void PrepareNextSequence(string thisAdvertName)
    {
        // Read Advert Name
        Debug.Log("Looking for Advert: " + thisAdvertName);

        // Find in Tapes
        VHSTape[] theseVHSTapes = FindObjectsOfType<VHSTape>();
        VideoClip thisAdvertVideo = null;
        AudioClip thisAdvertAudio = null;
        foreach (VHSTape thisVHS in theseVHSTapes)
        {
            if (thisVHS.myTitle == thisAdvertName)
            {
                // Find Large Video and Audio Files
                thisAdvertVideo = thisVHS.myVideo;
                thisAdvertAudio = thisVHS.myAudio;
                nextAdCountdown = (float)thisAdvertVideo.length;
            }
        }
        if (thisAdvertVideo)
        {
            // Prepare ad screen
            myBroadcastSystem.PrepareAdvert(thisAdvertVideo, thisAdvertAudio);
            // Look at first Edit Decision in next EDL for next sequence name and set to string nextSequenceName
            if (currentSequence < 2)
            {
                nextSequenceName = playbackEDL[currentSequence + 1][0].loadName;
                Debug.Log("Next Sequence Name: " + nextSequenceName);
                // Invoke StartNextSequence in 2 seconds
                Invoke("StartNextSequence", 2);
            }
            else
            {
                Debug.Log("Preparing Last Advert");
                Invoke("StartLastAdvert", 2);
            }
        }
        else
        {
            Debug.Log("CANNOT FIND VIDEO CLIP FOR VHSTape: " + thisAdvertName);
        }

    }


    void StartNextSequence()
    {
        // Start Advert Screen
        myBroadcastSystem.PlayAdvert();
        // Stop Broadcast Screens
        myBroadcastSystem.StopScreens();
        // Reset Broadcast Screens
        myBroadcastSystem.ResetScreens();
        // Load next sequence
        mySequence = FindSequence(nextSequenceName);
        // Calculate Next startPreRollTime;
        playPreRollTime = mySequence.runIn;
        // Set ad countdown clock to Ad length
        adCountdown = nextAdCountdown;
        // Switch to Advert Mode
        myMode = PlaybackMode.Advert;
        // Tell Broadcast Screens to Prepare nextSequence (use FindSequence(string thisSequenceName))
        myBroadcastSystem.PrepareScreens(mySequence.playbackVideo, mySequence.screenAudio, mySequence.AudioInterference);
        myBroadcastSystem.PrepareResistance(mySequence.resistanceVideo, mySequence.resistanceAudio);
        sequenceStarted = false;
        goneLive = false;
        // Reset Mixing Desk??

        // Increment currentSequence
        currentSequence++;
        currentPlaybackEDL = playbackEDL[currentSequence];
        maxEDLPosition = currentPlaybackEDL.Count;
        Debug.Log("MaxEDLPosition=" + maxEDLPosition);


    }

    void StartLastAdvert()
    {
        Debug.Log("Playing Last Advert");

        myMode = PlaybackMode.Post;
        adCountdown = nextAdCountdown;
        myBroadcastSystem.PlayAdvert();
        myBroadcastSystem.StopScreens();
        myBroadcastSystem.ResetScreens();
        zoomingOut = false;
    }
}
