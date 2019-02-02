using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telephone : MonoBehaviour {

    public AudioClip myRingtone;
    public AudioClip myHoldMusic;
    public AudioSource mySpeaker;

    public AudioClip myTEMPincomingCall01FORTESTING;
    public AudioClip myTEMPincomingCall02FORTESTING;

    private AudioClip currentCall;
    private ButtonAnimating myButton;
    private Light myLight;
    [HideInInspector]
    public bool isRinging, isConnected, isOnHold, hangUp;
    private float ringLength, ringCount;



	// Use this for initialization
	void Start () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        myLight = GetComponentInChildren<Light>();


        ///////////////////////// TEST CODE:
        //Invoke("TEMPDELETEME", 2);
        ////////////////////////////////////
	}
	
    void TEMPDELETEME()
    {
        IncomingCall(myTEMPincomingCall01FORTESTING, false, 0);

    }

    void TEMPAGAINDELETEME()
    {
        UnholdAndPlay(myTEMPincomingCall02FORTESTING, true);
    }


	// Update is called once per frame
	void Update () {

        // If isRinging 
        if (isRinging)
        {
            ringCount += Time.deltaTime;
            // Flash Red Light on and off
            float thisRoundedCount = Mathf.Floor(ringCount);
            if ((thisRoundedCount/2) == Mathf.Floor(thisRoundedCount/2))
            {
                myLight.enabled = true;
            } else
            {
                myLight.enabled = false;
            }
            // If ringLength != 0 then increase callWaitTime by time.delta time and, if longer than RingLength
            if (ringLength != 0)
            {
                if (ringCount > ringLength)
                {
                    // End Ringing Audio
                    mySpeaker.Stop();
                    // isRinging = false
                    isRinging = false;
                    // Turn Red Light Off
                    myLight.enabled = false;
                }
            }


            // monitor button and, if pressed:
            if (myButton.isDepressed)
            {
                // answer call (Stop ring, turn Red Light Off, set isConnected, clear isRinging and set the clip to currentCall and call void PlayCall)
                mySpeaker.Stop();
                isRinging = false;
                isConnected = true;
                PlayCall();

                ////////////////////////// REMOVE AFTER TESTING ///////////////////////////////
                //Invoke("TEMPAGAINDELETEME", 40);
                //////////////////////////////////////////////////////

            }

        }

        // If isConnected wait for sound to end then -
        if (isConnected)
        {
            if (!mySpeaker.isPlaying)
            {
                if (hangUp)
                {
                    // If hangUp is true - EndCall()
                    EndCall();
                }
                else
                {
                    PutOnHold();
                }
            }
        }

    }

    public void IncomingCall(AudioClip thisCall, bool thisHangUp, int thisRingLength)
    {
        // Make phone ring for ringLength Seconds.  If player fails to answer call is missed.  If ringLength is 0 then ring indefinitely.  Set isRinging to true. Set ringLength and hangUp
        mySpeaker.clip = myRingtone;
        mySpeaker.loop = true;
        mySpeaker.Play();
        hangUp = thisHangUp;
        currentCall = thisCall;
        ringLength = thisRingLength;
        ringCount = 0;
        isRinging = true;
        myLight.color = Color.red;
    }

    void PlayCall()
    {
        // Play call loaded into currentCall;
        mySpeaker.loop = false;
        mySpeaker.clip = currentCall;
        mySpeaker.Play();
        // Turn on Green Light
        myLight.color = Color.green;
        myLight.enabled = true;
    }

    public void UnholdAndPlay(AudioClip thisCall, bool thisHangUp) // USED TO RESUME WHEN "ON HOLD"
    {
        // IF isOnHold....
        if (isOnHold)
        {
            // Load Call into Source
            currentCall = thisCall;
            // Set hangUp status
            hangUp = thisHangUp;
            // call PlayCall();
            PlayCall();
            isOnHold = false;
        }
    }

    public void PlayImmediately(AudioClip thisCall, bool thisHangUp)
    {
        // Load Call into Source
        currentCall = thisCall;
        // Set hangUp status
        hangUp = thisHangUp;
        // call PlayCall();
        PlayCall();
        isOnHold = false;

    }

    void EndCall ()
    {
        // Release Button
        myButton.MoveUp();
        // Clear currentCall;
        mySpeaker.clip = null;
        currentCall = null;
        // clear hangUp
        hangUp = false;
        // Clear isConnected;
        isConnected = false;
        // Turn Green Light off
        myLight.enabled = false;
    }

    void PutOnHold()
    {
        // Switch to hold clip;
        mySpeaker.clip = myHoldMusic;
        // Turn on looping
        mySpeaker.loop = true;
        // Play holdClip;
        mySpeaker.Play();
        // set isOnHold to true;
        isOnHold = true;
    }

}
