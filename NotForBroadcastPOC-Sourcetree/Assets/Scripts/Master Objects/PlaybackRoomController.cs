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

    [HideInInspector]
    public bool preparingAd;

    private MasterController myMasterController;
    private BroadcastTV myBroadcastSystem;
    private GUIController myGUIController;
    private GodOfTheRoom myRoomGod;
    private DataStorage myDataStore;
    private DataStorage.SequenceData mySequence;
    private int myFileListPosition, maxFileListPosition;
    private string currentFilename;

    private List<EditDecision>[] playbackEDL = new List<EditDecision>[3];

    private enum PlaybackMode { Menu, Pre, Advert, Playback, Post }
    private PlaybackMode myMode;
    private int currentLevel, currentSequence;
    private float adCountdown, sequenceClock, masterClock;
    public MasterController.LevelData myLevelData;


    // Use this for initialization
    void Start () {
        myMasterController = FindObjectOfType<MasterController>();
        myBroadcastSystem = FindObjectOfType<BroadcastTV>();
        myGUIController = GetComponent<GUIController>();
        myDataStore = FindObjectOfType<DataStorage>();
        myRoomGod = FindObjectOfType<GodOfTheRoom>();
        myMode = PlaybackMode.Menu;
        playbackEDL[0] = new List<EditDecision>();
        playbackEDL[1] = new List<EditDecision>();
        playbackEDL[2] = new List<EditDecision>();

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
                }
                


                break;

            case PlaybackMode.Advert:

                // Wait for Preroll secs then start screens and EDL playback

                break;



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
        myRoomGod.SwitchScreensTo2DSound();
        myRoomGod.MuteRoom();
        myRoomGod.TripSwitchPower(true);
        myRoomGod.PlugPower(2, true);
        adCountdown = (float)myLevelData.preRoll.length;
        preparingAd = true;
        myMode = PlaybackMode.Pre;
        myGUIController.StartPlayback();

    }

    void StartSequencePlayback()
    {
        mySequence = FindSequence(myLevelData.sequenceNames[currentSequence]);
        myBroadcastSystem.PlayAdvert();
        myBroadcastSystem.ResetScreens();

        myBroadcastSystem.PrepareScreens(mySequence.playbackVideo, mySequence.screenAudio, mySequence.AudioInterference);
        myBroadcastSystem.PrepareResistance(mySequence.resistanceVideo, mySequence.resistanceAudio);

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

}
