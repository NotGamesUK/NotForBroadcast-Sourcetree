﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour {

    [HideInInspector]
    public VideoPlayer myScreen;
    public bool hasPower=false;
    public bool screenSync = true;
    public bool isPlaying;


    private bool currentSyncScreen;
    private AudioSource myAudioSource;
    //private MeshRenderer myNoSignal;
    private VideoClip myClip;
    private AudioClip myAudioClip;
    private bool waitingForPrepare = false;

	// Use this for initialization
	void Awake () {
        myScreen=GetComponentInChildren<VideoPlayer>();
        myAudioSource =GetComponentInChildren<AudioSource>();
        myClip = null;
        myAudioClip = null;
        //myNoSignal = GetComponentInChildren<NoSignal>().GetComponent<MeshRenderer>() ;
        //Debug.LomyScreen.clipg("My Audiosource: " + myAudio);
        isPlaying = false;
	}
	
    public void ResetMe()
    {
        isPlaying = false;
        myClip = null;
        myAudioClip = null;
        myScreen.Stop();
        myScreen.clip = null;
        if (myAudioSource)
        {
            myAudioSource.Stop();
            myAudioSource.clip = null;

        }
    }

	// Update is called once per frame
	void Update () {
        // if waitingForPrepared...
        if (waitingForPrepare)
        {
            if (myScreen.isPrepared)
            {
                // Check Screen is ready.  When it is:
                // turn off waitingForPrepared 
                waitingForPrepare = false;
                // increase SequenceController.preparedScreensCount; 


            }
        }
        if (isPlaying)
        {
            if (!myScreen.isPlaying) { isPlaying = false; }
        }
    }

    public void PrepareScreen(VideoClip thisVideo, AudioClip thisAudio, bool thisLoop)
    {
        // Load Video and Audio Clip
        myClip = thisVideo;
        myScreen.Stop();
        myScreen.clip = myClip;
        myScreen.Prepare();
        myScreen.isLooping = thisLoop;
        if (myAudioSource)
        {
            myAudioClip = thisAudio;
            myAudioSource.Stop();
            myAudioSource.clip = myAudioClip;
        }
        // Tell Update to monitor for Prepared
        waitingForPrepare = true;
        //Debug.Log("Television: Preparing Screen.");
    }

    public void PlayScreen()
    {
        // Disable NoSignal Image
        //myNoSignal.enabled = false;
        // Tell Video and Audio to Play
        myScreen.Play();
        isPlaying = true;
        if (myAudioSource) { myAudioSource.Play(); }
    }

    public void PlayVideoFromFrame (VideoClip thisClip, long thisFrame)
    {
        if (hasPower)
        {
            //Debug.Log("Screen Changing to clip: " + thisClip + " at frame " + thisFrame);

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

}
