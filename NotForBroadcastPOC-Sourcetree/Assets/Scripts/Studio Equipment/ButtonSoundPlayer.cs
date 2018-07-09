using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour {

    public AudioClip myButtonDownSFX;
    public AudioClip myButtonUpSFX;

    private AudioSource mySFX;
    private ButtonAnimating myButton;
    private bool lastCheck;

	// Use this for initialization
	void Start () {
        mySFX = GetComponent<AudioSource>();
        myButton = GetComponent<ButtonAnimating>();
        lastCheck = myButton.isDepressed;
	}
	
	// Update is called once per frame
	void Update () {
		if (lastCheck != myButton.isDepressed)
        {
            if (myButton.isDepressed)
            {
                mySFX.clip = myButtonDownSFX;
                mySFX.Play();
            } else
            {
                mySFX.clip = myButtonUpSFX;
                mySFX.Play();
            }
            lastCheck = myButton.isDepressed;
        }
	}
}
