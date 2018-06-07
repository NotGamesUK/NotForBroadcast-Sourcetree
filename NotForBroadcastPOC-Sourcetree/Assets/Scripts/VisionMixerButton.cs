﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMixerButton : MonoBehaviour {

    public int myID;

    public ButtonAnimating myButton;
    private VisionMixer visionMixer;
    private bool lastCheck;
    //[HideInInspector]
    public bool hasContent;
    private AudioSource mySFX;
    public AudioClip myButtonSound;

	// Use this for initialization
	void Start () {
        visionMixer = GetComponentInParent<VisionMixer>();
        lastCheck = myButton.isDepressed;
        myButton.Lock();
        myButton.oneWay = true;
        hasContent = false;
        mySFX = GetComponent<AudioSource>();
        mySFX.clip = myButtonSound;
	}
	
	// Update is called once per frame
	void Update () {
		if (lastCheck!=myButton.isDepressed)
        {
            //Debug.Log("State of button changed.");
            lastCheck = myButton.isDepressed;
            if (lastCheck==true) {
                visionMixer.ScreenChange(myID);
                myButton.Lock();
                mySFX.Play();
            }
        }
	}
}
