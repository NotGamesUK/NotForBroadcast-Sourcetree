﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour {

    [HideInInspector]
    public VideoPlayer myScreen;
    public bool hasPower=false;
    public bool screenSync = true;
    private bool currentSyncScreen;
    private VideoClip myClip;

	// Use this for initialization
	void Start () {
        myScreen=GetComponentInChildren<VideoPlayer>();
        myClip = myScreen.clip;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayVideoFromFrame (VideoClip thisClip, long thisFrame)
    {
        Debug.Log("Screen Changing to clip: " + thisClip + " at frame " + thisFrame);
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
        if (myClip)
        {
            myScreen.clip = myClip;
            myScreen.Play();
            hasPower = true;
        }
    }

    public void PowerOff()
    {
        myScreen.Stop();
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