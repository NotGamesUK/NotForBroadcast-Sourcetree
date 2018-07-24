using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GodOfTheRoom : MonoBehaviour {

    public AudioMixer UnityMixingDesk;
    public AudioSource[] SwitchableScreenAudioSources;
    public Switch[] PlugSwitch;
    public Switch TripSwitch;

    private float previousRoomVolume;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchScreensTo2DSound()
    {
        foreach(AudioSource thisAudiosource in SwitchableScreenAudioSources)
        {
            thisAudiosource.spatialBlend = 0;
        }
    }

    public void SwitchScreensTo3DSound()
    {
        foreach (AudioSource thisAudiosource in SwitchableScreenAudioSources)
        {
            thisAudiosource.spatialBlend = 1;
        }

    }

    public void PlugPower(int thisPlug, bool powerOn)
    {
        if (powerOn)
        {
            PlugSwitch[thisPlug].SwitchOn();
        } else
        {
            PlugSwitch[thisPlug].SwitchOff();
        }
    }

    public void TripSwitchPower(bool powerOn)
    {
        if (powerOn)
        {
            TripSwitch.SwitchOn();
        }
        else
        {
            TripSwitch.SwitchOff();
        }
    }

    public void MuteRoom()
    {
        UnityMixingDesk.GetFloat("GUIRoom", out previousRoomVolume);
        UnityMixingDesk.SetFloat("GUIRoom", -80f);
    }


    public void UnMuteRoom()
    {
        UnityMixingDesk.SetFloat("GUIRoom", previousRoomVolume);
    }
}
