using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VisionMixer : MonoBehaviour {

    public VisionMixerButton[] buttons;
    public Television[] smallScreens;
    public Television masterScreen;
    private bool hasPower;
    private int jumpToTV;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

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
        foreach (Television thisTV in smallScreens)
        {
            thisTV.PowerOff();
        }
        hasPower = false;
    }

}
