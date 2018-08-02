using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundDesk : MonoBehaviour {

    public GroovedSlider[] mySliders;
    public ButtonAnimating[] myMutes;
    public AudioMixer myDesk;
    public ButtonAnimating myBleepButton;
    public Material greenLED;
    public Material redLED;
    public Light masterMuteLight;
    public MeshRenderer masterMuteLED;
    public Light broadcastMuteLight;
    public MeshRenderer broadcastMuteLED;

    public Light myPowerLight;
    public float myMuteLightIntensity = 6f;
    public bool hasPower=false;
    public bool masterIsMuted;
    private AudioSource myBleep;
    private bool broadcastIsMuted;
    private float storedMasterVolume;
    private float storedBroadcastVolume;
    private EDLController myEDLController;
    private ScoringController myScoringController;
    private GodOfTheRoom myRoomGod;
    

	// Use this for initialization
	void Start () {
        myBleep = GetComponent<AudioSource>();
        myEDLController = FindObjectOfType<EDLController>();
        myScoringController = FindObjectOfType<ScoringController>();
        myRoomGod = FindObjectOfType<GodOfTheRoom>();
        myDesk.SetFloat("ResistanceVol", -80f);
        myDesk.SetFloat("BleepVol", -80f);
        myDesk.SetFloat("WhiteNoiseVol", -80f);
        myDesk.SetFloat("AudioInterferenceVol", -80f);
        LockMutes();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void MuteChannel (int thisChannel)
    {
        //Debug.Log("Call to mute Channel: " + thisChannel);
        string channelRequired = "Screen0" + thisChannel + "Vol";
        //Debug.Log(channelRequired);
        myDesk.SetFloat(channelRequired, -80f);
        if (!myMutes[thisChannel - 1].isDepressed) { myMutes[thisChannel - 1].MoveDown(); }
        if (myEDLController.myMode == EDLController.EDLModes.Recording)
        {
            myEDLController.AddMute(thisChannel);
        }

    }

    public void UnmuteChannel (int thisChannel)
    {
        //Debug.Log("Call to Unmute Channel: " + thisChannel);
        string channelRequired = "Screen0" + thisChannel + "Vol";
        //Debug.Log(channelRequired);
        myDesk.SetFloat(channelRequired, 0f);
        if (myMutes[thisChannel - 1].isDepressed) { myMutes[thisChannel - 1].MoveUp(); }
        if (myEDLController.myMode==EDLController.EDLModes.Recording)
        {
            myEDLController.AddUnMute(thisChannel);
        }

    }

    public void MuteBroadcastChannel(int thisChannel)
    {
        Debug.Log("Setting Scoring Controller.channelMuted[" + (thisChannel - 1) + "] to true");

        string channelRequired = "Broadcast0" + thisChannel + "Vol";
        myDesk.SetFloat(channelRequired, -80f);
        myScoringController.channelMuted[thisChannel - 1] = true;
        
    }

    public void UnMuteBroadcastChannel(int thisChannel)
    {
        Debug.Log("Setting Scoring Controller.channelMuted[" + (thisChannel - 1) + "] to false");

        string channelRequired = "Broadcast0" + thisChannel + "Vol";
        myDesk.SetFloat(channelRequired, 0f);
        myScoringController.channelMuted[thisChannel - 1] = false;

    }

    public void ResetMixingDesk()
    {
        for (int n=1; n<=4; n++)
        {
            UnmuteChannel(n);
            UnMuteBroadcastChannel(n);
        }
        myRoomGod.SetBroadcastVolumeSlider(1);
        myRoomGod.SetControlRoomVolumeSlider(1);
        myRoomGod.SetMixingDeskChannelSelect(1);
    }

    public void LockMutes()
    {
        for (int n = 0; n < 4; n++)
        {
            myMutes[n].Lock();
        }

    }

    public void SwitchToAdvertSound ()
    {
        myDesk.SetFloat("AdvertVol", 0);
        myDesk.SetFloat("LiveVol", -80f);
    }

    public void SwitchToBroadcastLiveSound ()
    {
        myDesk.SetFloat("AdvertVol", -80f);
        myDesk.SetFloat("LiveVol", 0f);

    }
    public void SetMasterVolume(float thisVolume)
    {
        thisVolume = 1 - thisVolume;
        storedMasterVolume = thisVolume * -80f;
        if (masterIsMuted) { thisVolume = 0; }
        myDesk.SetFloat("ControlRoomVol", LinearToDecibel(1 - thisVolume));
    }

    public void SetBroadcastVolume(float thisVolume)
    {
        thisVolume = 1 - thisVolume;
        storedBroadcastVolume = thisVolume * -80f;
        if (broadcastIsMuted) { thisVolume = 1; }
        myDesk.SetFloat("BroadcastVol", LinearToDecibel(1-thisVolume));
    }

    public void SetGUIMasterVolume(float thisVolume)
    {
        myDesk.SetFloat("MasterVol", LinearToDecibel(thisVolume));
    }
    public void SetGUIMusicVolume(float thisVolume)
    {
        myDesk.SetFloat("GUIMusic", LinearToDecibel(thisVolume));
    }
    public void SetGUIRoomVolume(float thisVolume)
    {
        myDesk.SetFloat("GUIRoom", LinearToDecibel(thisVolume));
    }
    public void SetGUISignalVolume(float thisVolume)
    {
        myDesk.SetFloat("GUISignal", LinearToDecibel(thisVolume));
    }

    public void SetWhiteNoiseAudioLevel(float thisVol)
    {
        thisVol = (100 - thisVol)/100;
        float thisDB = LinearToDecibel(thisVol);
        float thisOtherDB = LinearToDecibel(1 - thisVol);
        //Debug.Log("White Noise Volume: " + thisVol + "%="+thisDB+"DB.   Audio Interfered Volume: " + thisOtherDB);
        myDesk.SetFloat("WhiteNoiseVol", thisDB);
        myDesk.SetFloat("AudioInterferedVol", thisOtherDB);

    }

    public void SetResistanceAudioLevel(float thisVol)
    {
        thisVol = (100 - thisVol) / 100;
        float thisOtherDB = LinearToDecibel(thisVol);
        float thisDB = LinearToDecibel(1 - thisVol);
        //Debug.Log("Resistance Volume: " + thisVol + "%=" + thisDB + "DB.   Raw Broadcast Volume: " + thisOtherDB);
        myDesk.SetFloat("ResistanceVol", thisDB);
        myDesk.SetFloat("RawBroadcastVol", thisOtherDB);

    }

    public void SetAudioInterferenceLevel(float thisVol)
    {
        thisVol = (100 - thisVol) / 100;
        float thisOtherDB = LinearToDecibel(thisVol);
        float thisDB = LinearToDecibel(1 - thisVol);
        //Debug.Log("Audio Interference Volume: " + thisVol + "%=" + thisDB + "DB.   Censored Signal Volume: " + thisOtherDB);
        myDesk.SetFloat("AudioInterferenceVol", thisDB);
        myDesk.SetFloat("CensoredVol", thisOtherDB);

    }


    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    public void SetMutes(float thisSetting)
    {
        string thisSwitch = thisSetting.ToString();
        switch (thisSwitch)
        {
            case ("0"):
                masterIsMuted = false;
                myDesk.SetFloat("ControlRoomVol", storedMasterVolume);
                broadcastIsMuted = true;
                myDesk.GetFloat("BroadcastVol", out storedBroadcastVolume);
                myDesk.SetFloat("BroadcastVol", -80f);
                // Set master light to green
                masterMuteLight.color = Color.green;
                masterMuteLED.material = greenLED;
                // Set broadcast light to red
                broadcastMuteLight.color = Color.red;
                broadcastMuteLED.material = redLED;
                break;

            case ("1"):
                masterIsMuted = false;
                broadcastIsMuted = false;
                myDesk.SetFloat("ControlRoomVol", storedMasterVolume);
                myDesk.SetFloat("BroadcastVol", storedBroadcastVolume);
                masterMuteLight.color = Color.green;
                masterMuteLED.material = greenLED;
                broadcastMuteLight.color = Color.green;
                broadcastMuteLED.material = greenLED;
                break;

            case ("2"):
                broadcastIsMuted = false;
                myDesk.SetFloat("BroadcastVol", storedBroadcastVolume);
                broadcastIsMuted = true;
                myDesk.GetFloat("ControlRoomVol", out storedMasterVolume);
                myDesk.SetFloat("ControlRoomVol", -80f);
                // Set master light to red
                masterMuteLight.color = Color.red;
                masterMuteLED.material = redLED;
                // Set broadcast light to green
                broadcastMuteLight.color = Color.green;
                broadcastMuteLED.material = greenLED;
                break;

        }
    }

    public void BleepOn()
    {
        myDesk.SetFloat("SignalVol", -80f);
        myDesk.SetFloat("BleepVol", 0f);
        if (myEDLController.myMode == EDLController.EDLModes.Recording)
        {
            myEDLController.AddBleepOn();
        }
        myScoringController.bleepOn = true;

    }

    public void BleepOff()
    {
        myDesk.SetFloat("SignalVol", 0f);
        myDesk.SetFloat("BleepVol", -80f);
        if (myEDLController.myMode == EDLController.EDLModes.Recording)
        {
            myEDLController.AddBleepOff();
        }
        myScoringController.bleepOn = false;
    }

    void PowerOn()
    {
        hasPower = true;
        foreach (GroovedSlider thisSlider in mySliders)
        {
            thisSlider.hasPower = true;
        }
        foreach (ButtonAnimating thisMute in myMutes)
        {
            thisMute.hasPower = true;
        }
        myBleepButton.hasPower = true;
        masterMuteLight.intensity = myMuteLightIntensity;
        broadcastMuteLight.intensity = myMuteLightIntensity;
        myBleep.Play();
        myPowerLight.enabled = true;
    }

    void PowerOff()
    {
        hasPower = false;
        foreach (GroovedSlider thisSlider in mySliders)
        {
            thisSlider.hasPower = false;
        }
        foreach (ButtonAnimating thisMute in myMutes)
        {
            thisMute.hasPower = false;
        }
        myBleepButton.hasPower = false;
        masterMuteLight.intensity = 0;
        broadcastMuteLight.intensity = 0;
        myPowerLight.enabled = false;

        myBleep.Stop();
    }
}

