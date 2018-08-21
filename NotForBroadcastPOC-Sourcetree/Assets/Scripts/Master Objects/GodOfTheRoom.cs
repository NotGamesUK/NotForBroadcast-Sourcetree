using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GodOfTheRoom : MonoBehaviour {

    public AudioMixer unityMixingDesk;
    public AudioSource[] switchableScreenAudioSources;
    public Switch[] plugSwitch;
    public Switch tripSwitch;
    public Slider controlRoomVolumeSlider;
    public Slider broadcastVolumeSlider;
    public Slider channelSelectSlider;
    public Light roomLight;

    private float previousRoomVolume;
    private ScoringController myScoringController;
    private VHSTapeController myVHSTapeRack;

    private bool fadingSound, fadingLights;
    private float soundFadeIncrement, currentSoundFadeVolume, lightFadeIncrement, currentLightFadeIntensity, targetLightFadeIntensity, defaultRoomLightIntensity;

    // Use this for initialization
    void Start () {
        myScoringController = FindObjectOfType<ScoringController>();
        myVHSTapeRack = FindObjectOfType<VHSTapeController>();
        defaultRoomLightIntensity = roomLight.intensity;
        fadingSound = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (fadingSound)
        {
            currentSoundFadeVolume += soundFadeIncrement * Time.deltaTime;
            if (currentSoundFadeVolume >= 100)
            {
                currentSoundFadeVolume = 100;
                fadingSound = false;
            }
            float thisDB = LinearToDecibel(currentSoundFadeVolume/100);
            unityMixingDesk.SetFloat("NonGuiVol", thisDB);

        }

        if (fadingLights)
        {
            if (targetLightFadeIntensity > currentLightFadeIntensity)
            {
                //Debug.Log("ROOM GOD - FADING LIGHT UP TO " + targetLightFadeIntensity);

                currentLightFadeIntensity += lightFadeIncrement*Time.deltaTime;

                if (currentLightFadeIntensity > targetLightFadeIntensity)
                {
                    currentLightFadeIntensity = targetLightFadeIntensity;
                    fadingLights = false;
                }
            }
            if (targetLightFadeIntensity < currentLightFadeIntensity)
            {
                //Debug.Log("ROOM GOD - FADING LIGHT DOWN TO " + targetLightFadeIntensity);

                currentLightFadeIntensity -= lightFadeIncrement*Time.deltaTime;
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

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    public void FadeRoomLights(float thisTime, bool isFadingUp)
    {
        if (isFadingUp)
        {
            if (roomLight.intensity == defaultRoomLightIntensity) { return; }
            targetLightFadeIntensity = defaultRoomLightIntensity;
            currentLightFadeIntensity = 0;
        }
        else
        {
            if (roomLight.intensity == 0) { return; }
            currentLightFadeIntensity = defaultRoomLightIntensity;
            targetLightFadeIntensity = 0;
        }

        lightFadeIncrement = defaultRoomLightIntensity / thisTime;
        fadingLights = true;
        Debug.Log("ROOM GOD: Fading Light to Intesity of " + targetLightFadeIntensity + " over " + thisTime + " with increnemts of " + lightFadeIncrement + " per second.");
    }

    public void SwitchScreensTo2DSound()
    {
        foreach(AudioSource thisAudiosource in switchableScreenAudioSources)
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

    public void SetAllPlugs (string thisPlugSet) // Recieves a binary string: "10101010" - 1s turn plug on, 0s turn it off.
    {
        Debug.Log("StringReceived: " + thisPlugSet);
        for (int n=0; n<=7; n++)
        {
            char thisSettingAsChar = thisPlugSet[n];
            int thisSetting = (int)char.GetNumericValue(thisSettingAsChar);
            Debug.Log("Char " + n + " = " + thisSettingAsChar);
            Debug.Log("Setting " + n + " = " + thisSetting);
            if (thisSetting == 0)
            {
                Debug.Log("Turning Off Plug " + n);
                PlugPower(n, false);
            } else if (thisSetting==1) {
                Debug.Log("Turning On Plug " + n);
                PlugPower(n, true);
            }
            else
            {
                Debug.Log("Plug " + n + " UNCHANGED.  Setting:" + thisSetting);
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

    public void MuteRoom()
    {
        unityMixingDesk.GetFloat("GUIRoom", out previousRoomVolume);
        unityMixingDesk.SetFloat("GUIRoom", -80f);
    }


    public void UnMuteRoom()
    {
        unityMixingDesk.SetFloat("GUIRoom", previousRoomVolume);
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

    public void EnableObject(GameObject thisObject, bool thisEnabled)
    {
            thisObject.SetActive(thisEnabled);
    }

    public void LoadTapeRack(string thisLoadString)
    {
        myVHSTapeRack.SetAllTapesFromString(thisLoadString);
    }
}
