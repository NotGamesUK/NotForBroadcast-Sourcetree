using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundDesk : MonoBehaviour {

    public GroovedSlider[] mySliders;
    public ButtonAnimating[] myMutes;
    public AudioMixer myDesk;
    public ButtonAnimating myBleepButton;
    public bool hasPower=false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

    public void MuteChannel (int thisChannel)
    {
        Debug.Log("Call to mute Channel: " + thisChannel);
        string channelRequired = "Screen0" + thisChannel + "Vol";
        Debug.Log(channelRequired);
        myDesk.SetFloat(channelRequired, -80f);
    }

    public void UnmuteChannel (int thisChannel)
    {
        Debug.Log("Call to Unmute Channel: " + thisChannel);
        string channelRequired = "Screen0" + thisChannel + "Vol";
        Debug.Log(channelRequired);
        myDesk.SetFloat(channelRequired, 0f);

    }

    public void SetMasterVolume(float thisVolume)
    {
        thisVolume = 1 - thisVolume;
        myDesk.SetFloat("MasterVol", thisVolume*-80f);
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
    }
}

