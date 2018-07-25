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

    private float previousRoomVolume;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

}
