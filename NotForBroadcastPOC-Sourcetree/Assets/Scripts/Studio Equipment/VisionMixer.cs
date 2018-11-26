using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VisionMixer : MonoBehaviour {

    public VisionMixerButton[] myVisionMixerButtons;
    public Television[] smallScreens;
    public Television masterScreen;
    public VideoClip barsAndTone;
    public AudioClip barsAndToneAudio;
    public Switch myLinkSwitch;
    public NoSignal masterNoSignal;
    public Light myPowerLight;
    private SoundDesk myMixingDesk;
    private bool hasPower;
    [HideInInspector]
    public bool inPostRoll;
    private int jumpToTV;
    [HideInInspector]
    public int currentScreen;
    private int maxScreen;
    private AudioClip thisAudioClip;
    private EDLController myEDLController;
    private MasterController myMasterController;

    /// REMOVE AFTER EDL WORKING
    public BroadcastTV myBroadcastTV;
    /////////////////////////////


	// Use this for initialization
	void Start () {
        //myLinkSwitch = GetComponentInChildren<Switch>();
        myMixingDesk = FindObjectOfType<SoundDesk>();
        myEDLController = FindObjectOfType<EDLController>();
        myBroadcastTV = FindObjectOfType<BroadcastTV>();
        myMasterController = FindObjectOfType<MasterController>();
        currentScreen = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (inPostRoll)
        {
            bool stillPlaying = false;
            if (masterScreen.isPlaying)
            {
                stillPlaying = true;
            }
            else
            {
                for (int n = 0; n < maxScreen; n++)
                {
                    if (smallScreens[n].isPlaying)
                    {
                        stillPlaying = true;
                        break;
                    }
                }
            }
            if (!stillPlaying)
            {
                inPostRoll = false;
                ResetSystem();
            }
        }
    }

    public void PrepareScreen(int thisScreen, VideoClip thisVideo, AudioClip thisAudio, bool thisLooping)
    {
        // Unlock the relevant button

        smallScreens[thisScreen - 1].PrepareScreen(thisVideo, thisAudio, thisLooping);
    }

    public void PrepareScreens(VideoClip[] theseClips, AudioClip[] theseAudioClips)
    {
        // LOCK ALL BUTTONS????
        foreach (VisionMixerButton thisButton in myVisionMixerButtons)
        {
            thisButton.myButton.Lock();
            thisButton.hasContent = false;
        }
        myMixingDesk.LockMutes();
        inPostRoll = false;
        maxScreen = theseClips.Length;
        for (int n = 0; n < maxScreen; n++)
        {

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


        //if (currentScreen == 0 || currentScreen>maxScreen)
        //{
        //    masterScreen.PrepareScreen(barsAndTone, barsAndToneAudio, false);
        //} else
        //{
        //    masterScreen.PrepareScreen(theseClips[currentScreen - 1], theseAudioClips[currentScreen - 1], false);
        //}

    }

    public void PlayScreens()
    {
        for (int n = 0; n < maxScreen; n++)
        {
            smallScreens[n].PlayScreen();
            myVisionMixerButtons[n].hasContent = true;
            myVisionMixerButtons[n].myButton.Unlock();
            myMixingDesk.myMutes[n].Unlock();
            //Debug.Log("Unlocking Mute " + (n));


        }
        //masterScreen.PlayScreen();

    }

    public void PlayScreen(int thisScreen)
    {
        smallScreens[thisScreen - 1].PlayScreen();
        myVisionMixerButtons[thisScreen-1].hasContent = true;
        myVisionMixerButtons[thisScreen-1].myButton.Unlock();
        //Debug.Log("Unlocking Mute " + (thisScreen - 1));
        myMixingDesk.myMutes[thisScreen].Unlock();

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
            if (currentScreen == 0)
            {
                //Debug.Log("Sending NO SIGNAL to Back on MASTER SCREEN.");
                masterNoSignal.JumpBack();
            }
            currentScreen = selectedScreen;
            Invoke("JumpToFrame", 0.01f);
            // Log Change to EDL
            if (myEDLController.myMode == EDLController.EDLModes.Recording)
            {
                myEDLController.AddScreenChange(selectedScreen);
            }


            // Mute other sound channels as required
            if (myLinkSwitch.isOn && myMasterController.myState != MasterController.MasterState.Menu)
            {
                Debug.Log("Making Linked Muting Settings");
                for (int n = 1; n <= 4; n++)
                {
                    if (n == selectedScreen)
                    {
                        // Tell Mixing Desk to unmute the channel
                        myMixingDesk.UnmuteChannel(n);
                    }
                    else
                    {
                        // Tell Mixing Desk to mute the channel
                        myMixingDesk.MuteChannel(n);
                    }
                }
            }
            // Lock all VM Buttons - Prevents making an EDL that is impossible to play back.
            //Debug.Log("Locking Vision Mixer Buttons.");
            foreach(VisionMixerButton thisButton in myVisionMixerButtons)
            {
                thisButton.myButton.Lock();
            }
        }

        foreach(VisionMixerButton thisButton in myVisionMixerButtons)
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
            foreach (VisionMixerButton thisButton in myVisionMixerButtons)
            {
                if (thisButton.hasContent) //  && !thisButton.myButton.isDepressed
                {
                    thisButton.myButton.Unlock();
                }
            }
        }

    }

    void PowerOn()
    {
        foreach (VisionMixerButton thisButton in myVisionMixerButtons)
        {
            thisButton.myButton.hasPower = true;
            if (thisButton.hasContent) {
                thisButton.myButton.Unlock();
            }
        }
        myLinkSwitch.hasPower = true;
        foreach (Television thisTV in smallScreens)
        {
            thisTV.PowerOn();
        }
        myPowerLight.enabled = true;
        hasPower = true;
    }

    void PowerOff()
    {
        foreach (VisionMixerButton thisButton in myVisionMixerButtons)
        {
            thisButton.myButton.hasPower = false;
            thisButton.myButton.Lock();
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
        myPowerLight.enabled = false;
        hasPower = false;
    }

    public void ResetSystem()
    {
        //Debug.Log("Vision Mixer Resetting.");
        // Set currentScreen to 0
        currentScreen = 0;
        // Move Up and Lock all buttons
        foreach (VisionMixerButton thisButton in myVisionMixerButtons)
        {
            if (thisButton.myButton.isDepressed)
            {
                thisButton.myButton.MoveUp();
            }
            thisButton.myButton.Lock();
            
            thisButton.hasContent = false;
        }
        masterNoSignal.JumpToFront();
        myBroadcastTV.ResetScreens();
        myMixingDesk.ResetMixingDeskMutes();
        myMixingDesk.LockMutes();
    }

}
