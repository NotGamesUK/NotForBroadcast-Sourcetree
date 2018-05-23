using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VisionMixer : MonoBehaviour {

    public VisionMixerButton[] buttons;
    public Television[] smallScreens;
    public Television masterScreen;
    public VideoClip barsAndTone;
    public AudioClip barsAndToneAudio;
    private Switch myLinkSwitch;
    private SoundDesk myMixingDesk;
    private bool hasPower;
    private int jumpToTV;
    private AudioClip thisAudioClip;

	// Use this for initialization
	void Start () {
        myLinkSwitch = GetComponentInChildren<Switch>();
        myMixingDesk = FindObjectOfType<SoundDesk>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void PrepareScreen(int thisScreen, VideoClip thisVideo, AudioClip thisAudio, bool thisLooping)
    {
        // Unlock the relevant button

        smallScreens[thisScreen - 1].PrepareScreen(thisVideo, thisAudio, thisLooping);
    }

    public void PrepareScreens(VideoClip[] theseClips, AudioClip[] theseAudioClips)
    {
        // LOCK ALL BUTTONS????

        int maxScreen = theseClips.Length;
        for (int n = 0; n < maxScreen; n++)
        {
            PrepareScreen(n + 1, barsAndTone, barsAndToneAudio, true);
            smallScreens[n].myScreen.isLooping = true;

            if (theseAudioClips.Length > 1)
            {
                thisAudioClip = theseAudioClips[n];
            }
            else
            {
                thisAudioClip = theseAudioClips[0];
            }
            PrepareScreen(n + 1, theseClips[n], thisAudioClip, false);
        }

        // Set any Unset screens to Bars And Tone and Turn on Looping

    }

    public void PlayScreens()
    {
        for (int n = 0; n < 4; n++)
        {
            smallScreens[n].PlayScreen();
        }
    }

    public void PlayScreen(int thisScreen)
    {
        smallScreens[thisScreen - 1].PlayScreen();
    }

    public void ScreenChange(int selectedScreen)
    {
        if (hasPower)
        {
            //Debug.Log("Switched to Screen " + selectedScreen);
            long thisFrame = smallScreens[selectedScreen - 1].WhatFrame();
            VideoClip thisClip = smallScreens[selectedScreen - 1].WhatClip();
            //Debug.Log("Clip "+thisClip+" currently at frame " + thisFrame);
            masterScreen.PlayVideoFromFrame(thisClip, thisFrame);
            jumpToTV = selectedScreen;
            Invoke("JumpToFrame", 0.01f);
            // Log Change to EDL

            // Mute other sound channels as required
            if (myLinkSwitch.isOn)
            {
                for (int n = 1; n <= 4; n++)
                {
                    if (n == selectedScreen)
                    {
                        // Tell Mixing Desk to unmute the channel
                        myMixingDesk.UnmuteChannel(n);
                        // Log Change to EDL
                    }
                    else
                    {
                        // Tell Mixing Desk to mute the channel
                        myMixingDesk.MuteChannel(n);
                        // Log Change to EDL
                    }
                }
            }
            // Lock all VM Buttons - Prevents making an EDL that is impossible to play back.
            //Debug.Log("Locking Vision Mixer Buttons.");
            foreach(VisionMixerButton thisButton in buttons)
            {
                thisButton.myButton.isLocked = true;
            }
        }

        foreach(VisionMixerButton thisButton in buttons)
        {
            if (thisButton.myButton.isDepressed && thisButton.myID != selectedScreen) {

                //Debug.Log("Lifting Button " + thisButton.myID);
                thisButton.myButton.MoveUp();
            }
        }
    }

    void JumpToFrame()
    {
        long thisFrame = smallScreens[jumpToTV - 1].WhatFrame();
        //Debug.Log("Jumping to Screen " + jumpToTV + " at Frame " + thisFrame);
        if (!masterScreen.JumpToFrameIfPrepared(thisFrame + 2))
        {
            Invoke("JumpToFrame", 0.01f);
        }
        else
        {
            // Unlock all RAISED VM Buttons because video is now prepared.
            //Debug.Log("Unlocking Vision Mixer Buttons.");
            foreach (VisionMixerButton thisButton in buttons)
            {
                if (!thisButton.myButton.isDepressed)
                {
                    thisButton.myButton.isLocked = false;
                }
            }
        }

    }
    void PowerOn()
    {
        foreach (VisionMixerButton thisButton in buttons)
        {
            thisButton.myButton.hasPower = true;
            thisButton.myButton.isLocked = false;
        }
        myLinkSwitch.hasPower = true;
        foreach (Television thisTV in smallScreens)
        {
            thisTV.PowerOn();
        }
        hasPower = true;
    }

    void PowerOff()
    {
        foreach (VisionMixerButton thisButton in buttons)
        {
            thisButton.myButton.hasPower = false;
            thisButton.myButton.isLocked = true;
            if (thisButton.myButton.isDepressed)
            {
                thisButton.myButton.MoveUp();
            }
        }
        myLinkSwitch.hasPower = false;
        foreach (Television thisTV in smallScreens)
        {
            thisTV.PowerOff();
        }
        hasPower = false;
    }

}
