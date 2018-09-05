using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

[System.Serializable]
public class CheckpointData : IComparable<CheckpointData>
{
    // Level Data
    public int currentLevel, nextSequence;

    // Left View
    public bool fanIsLocked, tripSwitchIsOn;
    public string plugSwitchStatusString;
    public float blind01height, blind02height;

    // Right View
    public string[] namesOfFaxesInInTray = new string[20]; // If maximum possible number of faxes in the In-Tray Changes update this to MAX.

    // Down View
    public string[] VHSTapeTitles = new string[3];
    public int currentVHSPlayerSelected;
    public VideoClip currentAdVideo;
    public AudioClip currentAdAudio;

    // Controllers
    public List<EditDecision>[] previousEDLs = new List<EditDecision>[2];

    // Front View
    public float audienceNumbers, masterVolume, broadcastVolume, soundDeskSelectValue;
    public bool linkSwitchStatus;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int CompareTo(CheckpointData other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (nextSequence - other.nextSequence < 0)
        {
            toSendback = -1;
        }
        else if (nextSequence - other.nextSequence > 0)
        {
            toSendback = 1;
        }
        return toSendback;
    }

}
