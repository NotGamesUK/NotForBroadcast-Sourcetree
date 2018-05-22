using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour {

    [HideInInspector]
    public VideoPlayer myScreen;
    private AudioSource myAudioSource;
    public bool hasPower=false;
    public bool screenSync = true;
    private bool currentSyncScreen;
    private VideoClip myClip;
    private AudioClip myAudioClip;
    private bool waitingForPrepare = false;

	// Use this for initialization
	void Awake () {
        myScreen=GetComponentInChildren<VideoPlayer>();
        myAudioSource =GetComponentInChildren<AudioSource>();
        myClip = null;
        myAudioClip = null;
        //Debug.LomyScreen.clipg("My Audiosource: " + myAudio);
	}
	
	// Update is called once per frame
	void Update () {
        // if waitingForPrepared...
        // Check Screen is ready.  When it is turn off waiting for prepared and increase SequenceController.preparedScreensCount; 
    }

    public void PrepareScreen(VideoClip thisVideo, AudioClip thisAudio)
    {
        // Load Video and Audio Clip
        // Tell Update to monitor for Prepared
    }

    public void PlayScreen()
    {
        // Disable NoSignal Image
        // Tell Video and Audio to Play
    }

    public void PlayVideoFromFrame (VideoClip thisClip, long thisFrame)
    {
        if (hasPower)
        {
            Debug.Log("Screen Changing to clip: " + thisClip + " at frame " + thisFrame);

            myClip = thisClip;
            myScreen.clip = myClip;
            myScreen.Prepare();
            
        }
    }

    public bool JumpToFrameIfPrepared (long thisFrame)
    {
        if (myScreen.isPrepared)
        {
            myScreen.frame = thisFrame;
            return true;
        }
        return false;
    }

    public void PowerOn()
    {
        // Enable No Signal Image

        hasPower = true;
    }

    public void PowerOff()
    {
        hasPower = false;
    }

    public long WhatFrame()
    {
        long thisFrame = myScreen.frame;
        return thisFrame;
    }

    public VideoClip WhatClip()
    {
        VideoClip thisClip = myScreen.clip;
        return thisClip;
    }

    //void PlayAudio()
    //{
    //    // Find Audio with same name as MyClip
    //    string thisSearchName = myClip.ToString();
    //    //Debug.Log("Searching for AudioClip named " + thisSearchName);

    //    // Play Audio
    //    myAudioSource.Play();

    //}

    //void StopAudio()
    //{

    //}
}
