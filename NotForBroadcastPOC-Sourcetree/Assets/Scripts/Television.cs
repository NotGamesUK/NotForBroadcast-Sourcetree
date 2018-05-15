using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour {

    [HideInInspector]
    public VideoPlayer myScreen;
    private AudioSource myAudio;
    private AudioClip myAudioClip;
    public bool hasPower=false;
    public bool screenSync = true;
    private bool currentSyncScreen;
    private VideoClip myClip;

	// Use this for initialization
	void Awake () {
        myScreen=GetComponentInChildren<VideoPlayer>();
        myAudio =GetComponentInChildren<AudioSource>();
        myClip =myScreen.clip;
        //Debug.Log("My Audiosource: " + myAudio);
	}
	
	// Update is called once per frame
	void Update () {
		
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
        if (myClip)
        {
            myScreen.clip = myClip;
            myScreen.Play();
            if (myAudio)
            {
                PlayAudio();
            }
            hasPower = true;

        }
    }

    public void PowerOff()
    {
        myScreen.Stop();
        if (myAudio)
        {
            //Debug.Log("Stopping Audio");
            StopAudio();
        }
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

    void PlayAudio()
    {
        // Find Audio with same name as MyClip
        string thisSearchName = myClip.ToString();
        //Debug.Log("Searching for AudioClip named " + thisSearchName);

        // Play Audio
        myAudio.Play();

    }

    void StopAudio()
    {

    }
}
