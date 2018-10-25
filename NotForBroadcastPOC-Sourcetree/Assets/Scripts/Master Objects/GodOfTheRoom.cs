using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.UI;

public class GodOfTheRoom : MonoBehaviour
{

    public AudioMixer unityMixingDesk;
    public AudioSource[] switchableScreenAudioSources;
    public Switch[] plugSwitch;
    public Switch tripSwitch;
    public Slider controlRoomVolumeSlider;
    public Slider broadcastVolumeSlider;
    public Slider channelSelectSlider;
    public Light roomLight;
    public Sun mySky;
    public Television[] allScreensExceptBroadcast;
    public RollerBlind[] myBlinds;
    public VHSPlayer[] myVHSPlayers;

    [Space(5)]
    [Header("Objects that get Activated/Deactivated:")]
    [Space(2)]
    public GameObject myLensFlareLight;
    public GameObject myVisionMixerLinkSwitch;
    public GameObject myVisionMixerUpgradePanel;
    public GameObject myIntercom;
    public GameObject myFaxMachineGameObject;
    public GameObject myTowerControlPlug;
    public GameObject myTowerControlLever;
    public GameObject myMuteCover;
    public GroovedSlider myFrequencyControlSlider;

    [Space(5)]
    [Header("Materials that get Swapped (with their respective MeshRenderers)")]
    [Space(2)]
    public Material noTapeMixingDesk;
    public Material tapedMixingDesk;
    public MeshRenderer mixingDeskFacePlateMesh;

    public float previousRoomVolume;
    private ScoringController myScoringController;
    private VHSTapeController myVHSTapeRack;
    private VHSControlPanel myVHSPlayerSelectionPanel;
    private BroadcastTV myBroadcastSystem;
    //private SoundDesk myMixingDesk;
    private RotatingFan myFan;
    private SatelliteDish myTower;
    private VisionMixer myVisionMixer;
    private ValveBox myValveBox;
    private MasterGauges myGauges;
    private VHSPlayer[] allVHSPlayers;
    private FaxMachine myFaxMachine;
    private InterferenceSystem myInterferenceSystem;
    private MasterController myMasterController;
    private SequenceController mySequenceController;
    private GUIController myGUIController;
    private EventController myEventController;
    private BackWallClock myBackWallClock;
    private VHSTape[] allTapes;
    private CheckpointData[] myCheckpoints = new CheckpointData[3];
    private VideoClip advertAfterCheckpointVideo;
    private AudioClip advertAfterCheckpointAudio;

    private bool fadingSound, fadingLights, roomMuted;
    private float soundFadeIncrement, currentSoundFadeVolume, lightFadeIncrement, currentLightFadeIntensity, targetLightFadeIntensity, defaultRoomLightIntensity;

    // Use this for initialization
    void Start()
    {
        myScoringController = FindObjectOfType<ScoringController>();
        myVHSTapeRack = FindObjectOfType<VHSTapeController>();
        myVHSPlayerSelectionPanel = FindObjectOfType<VHSControlPanel>();
        myBroadcastSystem = FindObjectOfType<BroadcastTV>();
        //myMixingDesk = FindObjectOfType<SoundDesk>();
        myFan = FindObjectOfType<RotatingFan>();
        myTower = FindObjectOfType<SatelliteDish>();
        myBlinds = FindObjectsOfType<RollerBlind>();
        myValveBox = FindObjectOfType<ValveBox>();
        myGauges = FindObjectOfType<MasterGauges>();
        allVHSPlayers = FindObjectsOfType<VHSPlayer>();
        myFaxMachine = myFaxMachineGameObject.GetComponent<FaxMachine>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myInterferenceSystem = FindObjectOfType<InterferenceSystem>();
        myMasterController = FindObjectOfType<MasterController>();
        mySequenceController = FindObjectOfType<SequenceController>();
        myGUIController = FindObjectOfType<GUIController>();
        myEventController = FindObjectOfType<EventController>();
        myBackWallClock = FindObjectOfType<BackWallClock>();
        allTapes = FindObjectsOfType<VHSTape>();

        defaultRoomLightIntensity = roomLight.intensity;
        fadingSound = false;
        roomMuted = false;
    }

    // Update is called once per frame

    void Update()
    {
        if (fadingSound)
        {
            currentSoundFadeVolume += soundFadeIncrement * Time.deltaTime;
            if (currentSoundFadeVolume >= 100)
            {
                currentSoundFadeVolume = 100;
                fadingSound = false;
            }
            float thisDB = LinearToDecibel(currentSoundFadeVolume / 100);
            unityMixingDesk.SetFloat("NonGuiVol", thisDB);

        }

        if (fadingLights)
        {
            if (targetLightFadeIntensity > currentLightFadeIntensity)
            {
                //Debug.Log("ROOM GOD - FADING LIGHT UP TO " + targetLightFadeIntensity);

                currentLightFadeIntensity += lightFadeIncrement * Time.deltaTime;

                if (currentLightFadeIntensity > targetLightFadeIntensity)
                {
                    currentLightFadeIntensity = targetLightFadeIntensity;
                    fadingLights = false;
                }
            }
            if (targetLightFadeIntensity < currentLightFadeIntensity)
            {
                //Debug.Log("ROOM GOD - FADING LIGHT DOWN TO " + targetLightFadeIntensity);

                currentLightFadeIntensity -= lightFadeIncrement * Time.deltaTime;
                if (currentLightFadeIntensity < targetLightFadeIntensity)
                {
                    currentLightFadeIntensity = targetLightFadeIntensity;
                    fadingLights = false;
                }
            }
            roomLight.intensity = currentLightFadeIntensity;
            //Debug.Log("ROOM GOD - Light Intensity: " + currentLightFadeIntensity);
        }
    }

    // General

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    public void EnableObject(GameObject thisObject, bool thisEnabled)
    {
        Debug.Log("ENABLING " + thisObject);
        thisObject.SetActive(thisEnabled);
    }

    public void SetRoomTemperature(float thisTemperature)
    {
        myGauges.roomTemperature = thisTemperature;
    }

    public void SetMaximumPower(float thisPower)
    {
        myGauges.maxPower = thisPower;
    }

    public void SetSkyForLevel(int thisLevel)
    {
        mySky.SetSky(thisLevel);
    }


    // Room Controls

    public void FadeRoomLights(float thisTime, bool isFadingUp)
    {
        if (isFadingUp)
        {
            if (roomLight.intensity == defaultRoomLightIntensity) { return; }
            targetLightFadeIntensity = defaultRoomLightIntensity;
            currentLightFadeIntensity = 0f;
        }
        else
        {
            if (roomLight.intensity == 0) { return; }
            currentLightFadeIntensity = defaultRoomLightIntensity;
            targetLightFadeIntensity = 0f;
        }

        lightFadeIncrement = defaultRoomLightIntensity / thisTime;
        fadingLights = true;
        //Debug.Log("ROOM GOD: Fading Light to Intesity of " + targetLightFadeIntensity + " over " + thisTime + " with increnemts of " + lightFadeIncrement + " per second.");
    }

    public void MuteRoom()
    {
        Debug.Log("MUTE ROOM CALLED! -----------------------------------------------------");
        if (!roomMuted)
        {
            Debug.Log("MUTING ROOM! ----------------------------------------------------------------");

            unityMixingDesk.GetFloat("GUIRoom", out previousRoomVolume);
            //Debug.Log("Previous Room Volume: " + previousRoomVolume);
            unityMixingDesk.SetFloat("GUIRoom", -80f);
            roomMuted = true;
        }
    }

    public void UnMuteRoom()
    {
        Debug.Log("UNMUTE ROOM CALLED! -----------------------------------------------------");

        if (roomMuted)
        {
            Debug.Log("UNMUTING ROOM! ----------------------------------------------------------------");

            unityMixingDesk.SetFloat("GUIRoom", previousRoomVolume);
            roomMuted = false;
        }
    }

    public void MuteAll3DSound()
    {
        unityMixingDesk.SetFloat("NonGuiVol", -80f);
        currentSoundFadeVolume = 0;
    }

    public void FadeInAll3DSound(float thisTime)
    {
        currentSoundFadeVolume = 0;
        soundFadeIncrement = 100 / thisTime;
        fadingSound = true;
    }


    // Left View

    public void SetAllPlugs(string thisPlugSet) // Recieves a binary string: "10101010" - 1s turn plug on, 0s turn it off.
    {
        //Debug.Log("StringReceived: " + thisPlugSet);
        for (int n = 0; n <= 7; n++)
        {
            char thisSettingAsChar = thisPlugSet[n];
            int thisSetting = (int)char.GetNumericValue(thisSettingAsChar);
            //Debug.Log("Char " + n + " = " + thisSettingAsChar);
            //Debug.Log("Setting " + n + " = " + thisSetting);
            if (thisSetting == 0)
            {
                //Debug.Log("Turning Off Plug " + n);
                PlugPower(n, false);
            }
            else if (thisSetting == 1)
            {
                //Debug.Log("Turning On Plug " + n);
                PlugPower(n, true);
            }
            else
            {
                //Debug.Log("Plug " + n + " UNCHANGED.  Setting:" + thisSetting);
            }
        }
    }

    public void PlugPower(int thisPlug, bool powerOn)
    {
        if (plugSwitch[thisPlug].isActiveAndEnabled)
        {
            if (powerOn)
            {
                plugSwitch[thisPlug].SwitchOn();
            }
            else
            {
                plugSwitch[thisPlug].SwitchOff();
            }
        }
    }

    public void TripSwitchPower(bool powerOn)
    {
        if (powerOn)
        {
            tripSwitch.SwitchOn();
        }
        else
        {
            tripSwitch.SwitchOff();
        }
    }

    public void SetTowerDropSpeed(float thisSpeed)
    {
        myTower.dropSpeed = thisSpeed;
    }

    public void SetTowerRotationSpeed(float thisSpeed)
    {
        myTower.turnSpeed = thisSpeed;
    }

    public void DropSatellite()
    {
        myTower.DropDish();
    }

    public void SetFanSwitch(bool rotationOff)
    {
        if (rotationOff)
        {
            myFan.myButton.KeyDown();

        }
        else
        {
            myFan.myButton.KeyUp();
        }
    }

    public void SetBlinds(float thisPercentageDown)
    {
        foreach(RollerBlind thisBlind in myBlinds)
        {
            thisBlind.SetToPercentageUp(thisPercentageDown);
        }
    }

    // Right View

    public void SendFax(string thisText, float thisCharacterSize)
    {
        myFaxMachine.ReceiveFax(thisText, thisCharacterSize);
    }


    // Front View

    public void SwitchScreensTo2DSound()
    {
        foreach (AudioSource thisAudiosource in switchableScreenAudioSources)
        {
            thisAudiosource.spatialBlend = 0;
        }
    }

    public void SwitchScreensTo3DSound()
    {
        foreach (AudioSource thisAudiosource in switchableScreenAudioSources)
        {
            thisAudiosource.spatialBlend = 1;
        }

    }

    public void SetControlRoomVolumeSlider(float thisSetting)
    {
        controlRoomVolumeSlider.value = thisSetting;
    }

    public void SetBroadcastVolumeSlider(float thisSetting)
    {
        broadcastVolumeSlider.value = thisSetting;
    }

    public void SetMixingDeskChannelSelect(int thisChannel)
    {
        channelSelectSlider.value = thisChannel;
    }

    public void SetVisionMixerLinkSwitch(bool linkOn)
    {
        if (linkOn)
        {
            if (!myVisionMixer.myLinkSwitch.isOn)
            {
                myVisionMixer.myLinkSwitch.KeyDown();

            }
        }
        else
        {
            if (myVisionMixer.myLinkSwitch.isOn)
            {
                myVisionMixer.myLinkSwitch.KeyDown();

            }

        }
    }

    public void ShowTapeOnMixingDesk(bool showMe)
    {
        if (showMe)
        {
            mixingDeskFacePlateMesh.material = tapedMixingDesk;
        } else
        {
            mixingDeskFacePlateMesh.material = noTapeMixingDesk;
        }
    }


    // Down View

    public void LoadTapeRack(string thisLoadString)
    {
        myVHSTapeRack.SetAllTapesFromString(thisLoadString);
    }


    // Controllers

    public void ChangeScoringMode(string thisMode)
    {
        if (thisMode == "Multi")
        {
            myScoringController.myScoringMode = ScoringController.ScoringMode.MultiCam;
        }
        else if (thisMode == "Single")
        {
            myScoringController.myScoringMode = ScoringController.ScoringMode.SingleCam;
        }
    }

    public void SwitchToMulticamMode()
    {
        myScoringController.myScoringMode = ScoringController.ScoringMode.MultiCam;
    }

    public void SwitchToSingleCamMode()
    {
        myScoringController.myScoringMode = ScoringController.ScoringMode.SingleCam;
    }

    public void SwitchToRhythmCamMode()
    {
        // For Full Version
        myScoringController.myScoringMode = ScoringController.ScoringMode.RhythmCam;
    }

    public void SetDownWeight(float thisWeight)
    {
        myScoringController.upWeight = thisWeight;
    }

    public void SetUpWeight(float thisWeight)
    {
        myScoringController.downWeight = thisWeight;
    }

    public void SetGodWeight(float thisWeight)
    {
        myScoringController.godWeighting = thisWeight;
    }



    //public void TEMPResetSky()
    //{
    //    mySky.ResetSky();
    //}

    public void TEMPRestartBroadcast()
    {
        myGUIController.StartBroadcast(6);
    }

    public void ResetRoom()
    {
        // Returns Room to Default State
        MuteAll3DSound();
        MuteRoom();
        SetRoomTemperature(20);
        FadeRoomLights(0.01f, true);

        //// All Views - May Not Be Necessary

        //ButtonAnimating[] allButtons = FindObjectsOfType<ButtonAnimating>();
        //foreach(ButtonAnimating thisButton in allButtons)
        //{
        //    thisButton.ResetMe();
        //}
        //Switch[] allSwitches = FindObjectsOfType<Switch>();
        //foreach (Switch thisSwitch in allSwitches)
        //{
        //    thisSwitch.ResetMe();
        //}

        //// Left View

        // Power
        SetAllPlugs("00000000");
        TripSwitchPower(false);

        // Broadcast Tower
        SetTowerDropSpeed(0);
        SetTowerRotationSpeed(10);
        myTower.transform.rotation = myTower.myStartRotation;
        myTower.ResetMe();

        // Tower Control
        myTowerControlPlug.SetActive(true);
        myTowerControlLever.SetActive(true);

        // Fan
        SetFanSwitch(true);
        myFan.ResetHead();


        // Blinds
        foreach (RollerBlind thisBlind in myBlinds)
        {
            thisBlind.ResetMe();
        }

        // ValveBox
        myValveBox.ResetMe();

        // Skybox
        mySky.ResetSky();
        myLensFlareLight.SetActive(false);


        //// Right View

        // Comms
        myIntercom.SetActive(false);
        myFaxMachine.ResetMe();
        myFaxMachineGameObject.SetActive(false);


        //// Front View

        // Gauges
        myGauges.ResetMe();

        // Mixing Desk
        myMuteCover.SetActive(false);
        mixingDeskFacePlateMesh.material = tapedMixingDesk;
        //SetMixingDeskChannelSelect(1);
        //SetControlRoomVolumeSlider(1);
        //SetBroadcastVolumeSlider(1);

        // Vision Mixer
        myVisionMixerLinkSwitch.SetActive(true);
        myVisionMixerUpgradePanel.SetActive(true);
        SetVisionMixerLinkSwitch(false);
        myVisionMixer.ResetSystem();

        // Frequency Control
        myFrequencyControlSlider.ResetMe();


        // Interference Scroller
        myInterferenceSystem.ResetMe();

        // Screens 1-4 and Master
        foreach (Television thisTelevision in allScreensExceptBroadcast)
        {
            thisTelevision.ResetMe();
        }


        // Broadcast System
        myBroadcastSystem.ResetMe();

        // On-Air Light
        mySequenceController.myOnAirLight.LightOff();

        // Audience Display
        myScoringController.InitialiseVU();

        // Clock
        myBackWallClock.ResetMe();

        //// Down View

        // Tape Rack
        myVHSTapeRack.SetAllTapesFromString("XXX#XXX#XXX#XXX#XXX#XXX#XXX#XXX");
        myVHSPlayerSelectionPanel.ResetMe();
        // VHS Machines and Panel
        foreach (VHSPlayer thisPlayer in allVHSPlayers)
        {
            thisPlayer.ResetMe();
        }

        

        //// Controllers
        myScoringController.ResetMe();
        
        // EDL Controller
        // Sequence Controller
        // Input Controller?
        // 

        Debug.Log("GOD: Room is reset!");

    }

    public void MakeCheckpoint(int thisCheckpointNumber)
    {
        // Level Data
        myCheckpoints[thisCheckpointNumber] = new CheckpointData();
        myCheckpoints[thisCheckpointNumber].currentLevel = myMasterController.currentLevel;
        myCheckpoints[thisCheckpointNumber].nextSequence = myMasterController.currentSequence;

        // Left View
        myCheckpoints[thisCheckpointNumber].fanIsLocked = myFan.myButton.isDepressed;
        myCheckpoints[thisCheckpointNumber].tripSwitchIsOn = tripSwitch.isOn;
        string thisPlugString = "";
        for (int n = 0; n <= 7; n++)
        {
            if (plugSwitch[n].isOn)
            {
                thisPlugString += "1";
            }
            else
            {
                thisPlugString += "0";
            }
        }
        //Debug.Log("Make Checkpoint Plug String: " + thisPlugString);
        myCheckpoints[thisCheckpointNumber].plugSwitchStatusString = thisPlugString;
        myCheckpoints[thisCheckpointNumber].blind01height = myBlinds[0].transform.localPosition.y;
        myCheckpoints[thisCheckpointNumber].blind02height = myBlinds[1].transform.localPosition.y;

        // Right View

        // Add InTray Fax Data Storage Here when In-Tray is Implemented



        // Down View

        for (int n = 0; n < 3; n++)
        {
            if (myVHSPlayers[n].myTape)
            {
                Debug.Log("GOD: Making Checkpoint - VHS Player " + (n + 1) + " has tape " + myVHSPlayers[n].myTape.myName);
                myCheckpoints[thisCheckpointNumber].VHSTapeTitles[n] = myVHSPlayers[n].myTape.myName;
            }
            else
            {
                Debug.Log("GOD: Making Checkpoint - VHS Player " + (n + 1) + " is EMPTY.");

                myCheckpoints[thisCheckpointNumber].VHSTapeTitles[n] = "EMPTY";
            }
        }

        myCheckpoints[thisCheckpointNumber].currentVHSPlayerSelected = myVHSPlayerSelectionPanel.selectedPlayer;
        Debug.Log("GOD: Making Checkpoint - Currently Selected VHS Player: " + myVHSPlayerSelectionPanel.selectedPlayer);

        myCheckpoints[thisCheckpointNumber].currentAdVideo = myBroadcastSystem.myAdvertScreen.clip;
        Debug.Log("GOD: Making Checkpoint - Current Ad Video: " + myBroadcastSystem.myAdvertScreen.clip);

        myCheckpoints[thisCheckpointNumber].currentAdAudio = myBroadcastSystem.myAdvertAudiosource.clip;
        Debug.Log("GOD: Making Checkpoint - Current Ad Audio: " + myBroadcastSystem.myAdvertAudiosource.clip);

        // Controllers

        //for (int n = 0; n <= thisCheckpointNumber; n++)
        //{
        //    myCheckpoints[thisCheckpointNumber].previousEDLs[n] = myMasterController.broadcastEDL[n];
        //}

        // Front View

        myCheckpoints[thisCheckpointNumber].audienceNumbers = myScoringController.audiencePercentage;
        myCheckpoints[thisCheckpointNumber].masterVolume = controlRoomVolumeSlider.value;
        myCheckpoints[thisCheckpointNumber].broadcastVolume = broadcastVolumeSlider.value;
        myCheckpoints[thisCheckpointNumber].soundDeskSelectValue = channelSelectSlider.value;
        myCheckpoints[thisCheckpointNumber].linkSwitchStatus = myVisionMixer.myLinkSwitch.isOn;

        Debug.Log("GOD: Checkpoint Created!! Number: " + thisCheckpointNumber);

    }

    void LoadCheckpoint(int thisCheckpointNumber)
    {
        // Level Data
        CheckpointData thisCheckpoint = myCheckpoints[thisCheckpointNumber];

        // Left View
        SetFanSwitch(thisCheckpoint.fanIsLocked);
        TripSwitchPower(thisCheckpoint.tripSwitchIsOn);
        SetAllPlugs(thisCheckpoint.plugSwitchStatusString);
        myBlinds[0].transform.localPosition = new Vector3(myBlinds[0].transform.localPosition.x, thisCheckpoint.blind01height, myBlinds[0].transform.localPosition.z);
        myBlinds[1].transform.localPosition = new Vector3(myBlinds[1].transform.localPosition.x, thisCheckpoint.blind02height, myBlinds[1].transform.localPosition.z);

        // Right View

        // Add InTray Fax Data Storage Here when In-Tray is Implemented



        // Down View
        for (int n = 0; n < 3; n++)
        {
            Debug.Log("GOD: Checkpoint Loader Looking for VHS TAPE " + thisCheckpoint.VHSTapeTitles[n] + " for Player " + (n + 1));
            if (thisCheckpoint.VHSTapeTitles[n]!="EMPTY")
            {
                Debug.Log("GOD is Searching...");
                foreach(VHSTape thisTape in allTapes)
                {
                    Debug.Log("Comparing " + thisCheckpoint.VHSTapeTitles[n] + " with " + thisTape.myName);
                    if (thisTape.myName == thisCheckpoint.VHSTapeTitles[n])
                    {
                        Debug.Log("Attempting to Load Tape " + thisTape.myTitle + " into VHS Player " + (n + 1));
                        myVHSPlayers[n].InstantLoadTape(thisTape);
                    }
                }
            }
        }
        if (thisCheckpoint.currentVHSPlayerSelected > 0)
        {
            Debug.Log("GOD (Checkpoint): Selecting Player " + thisCheckpoint.currentVHSPlayerSelected)
;           myVHSPlayerSelectionPanel.selectionButtons[thisCheckpoint.currentVHSPlayerSelected-1].myButton.MoveDown();
        }
        advertAfterCheckpointVideo = thisCheckpoint.currentAdVideo;
        advertAfterCheckpointAudio = thisCheckpoint.currentAdAudio;


        // Front View

        myScoringController.audiencePercentage = thisCheckpoint.audienceNumbers;
        controlRoomVolumeSlider.value=thisCheckpoint.masterVolume;
        broadcastVolumeSlider.value=thisCheckpoint.broadcastVolume;
        channelSelectSlider.value=thisCheckpoint.soundDeskSelectValue;

        if (thisCheckpoint.linkSwitchStatus)
        {
            SetVisionMixerLinkSwitch(true);
        }
        else
        {
            SetVisionMixerLinkSwitch(false);
        }

        // Controllers

        myMasterController.currentSequence = thisCheckpoint.nextSequence;
        myEventController.currentAdvert = thisCheckpoint.nextSequence-1;
        myScoringController.currentSequenceNumber = thisCheckpoint.nextSequence;
        //for (int n = 0; n <= thisCheckpointNumber; n++)
        //{
        //    myCheckpoints[thisCheckpointNumber].previousEDLs[n] = myMasterController.broadcastEDL[n];
        //}

        Debug.Log("GOD: Checkpoint is Loaded!");

    }

    public void ContinueFromCheckpoint(int thisCheckpointNumber)
    {
        // Tell Master Controller to LoadPreviousEDLs(adNumber);

        // Call RetryFromCheckpoint(int thisCheckpointNumber)

    }


    public void RetryFromCheckpoint()
    {
        Debug.Log("CURRENT SEGMENT WHEN RETRY CALLED: " + myMasterController.currentSequence);
        advertAfterCheckpointAudio = null;
        advertAfterCheckpointVideo = null;
        myGUIController.ReplaySegment();

        // Reset Room
        ResetRoom();
        myVHSTapeRack.SetAllTapesFromString(myMasterController.myLevelData.advertList);
        // Do all Events up to Current Sequence - Alter behaviour of some (eg - SendFax should add Fax straight to InTray)
        myEventController.DoAllEventsUpTo(myMasterController.currentSequence-1);
        Debug.Log("GOD: Events Are all Caught Up!");

        // Load and Implement Checkpoint Data
        LoadCheckpoint(myMasterController.currentSequence - 1);

        // Setup Master Controller to continue from relevant sequence
        myVisionMixer.inPostRoll = false;
        myMasterController.ClearEDL(myMasterController.currentSequence);
        myMasterController.PrepareAdvert(advertAfterCheckpointVideo, advertAfterCheckpointAudio);
        myScoringController.SetUpScoreTracker(myMasterController.myLevelData.sequenceNames[0], myMasterController.myLevelData.sequenceNames[1], myMasterController.myLevelData.sequenceNames[2]);
        Invoke("RetryFromCheckpointPart02", 3);

    }

    public void RetryFromCheckpointPart02()
    {
        myGUIController.FadeSegment();
        UnMuteRoom();
        FadeInAll3DSound(5f);
        // Tell Master Controller to RetryFromAd(int adNumber);


    }
}
