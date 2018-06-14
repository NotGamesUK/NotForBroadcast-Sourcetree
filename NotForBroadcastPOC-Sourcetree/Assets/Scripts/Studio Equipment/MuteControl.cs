using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteControl : MonoBehaviour {

    public int myID;
    private ButtonAnimating myButton;
    private bool lastCheck;
    private SoundDesk myDesk;
    private AudioSource mySFX;
    public AudioClip myButtonSound;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<ButtonAnimating>();
        myDesk = FindObjectOfType<SoundDesk>();
        mySFX = GetComponent<AudioSource>();
        mySFX.clip = myButtonSound;
	}
	
	// Update is called once per frame
	void Update () {
        if (lastCheck != myButton.isDepressed)
        {
            //Debug.Log("State of button changed.");
            lastCheck = myButton.isDepressed;
            if (myButton.hasPower)
            {
                if (lastCheck == true)
                {
                    myDesk.MuteChannel(myID);
                    // Log Change to EDL
                }
                else
                {
                    myDesk.UnmuteChannel(myID);
                    // Log Change to EDL
                }
            }
            mySFX.Play();
        }

    }
}
