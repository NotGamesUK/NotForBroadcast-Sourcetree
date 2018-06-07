using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telephone : MonoBehaviour {

    public AudioClip myRingtone;
    public AudioClip myHoldMusic;
    public AudioSource mySpeaker;

    private AudioClip currentCall;

    private bool isRinging, isConnected, isOnHold, hangUp;
    private float ringLength, callWaitTime;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // If isRinging 
            // monitor button and, if pressed, answer call (Stop ring, set isConnected, clear isRinging and set the clip to currentCall and call void PlayCall)
            // If ringLength != 0 then increase callWaitTime by time.delta time and, if longer than RingLength -
                // End Ringing Audio
                // isRinging = false

        // If isConnected wait for sound to end then -

            // If hangUp is true - EndCall()
            // ELSE PutOnHold();

            
	}

    public void IncomingCall(AudioClip thisCall, bool thisHangUp, int thisRingLength)
    {
        // Make phone ring for ringLength Seconds.  If player fails to answer call is missed.  If ringLength is 0 then ring indefinitely.  Set isRinging to true. Set ringLength and hangUp
    }

    void PlayCall()
    {
        // Play call loaded into currentCall;
    }

    public void UnholdAndPlay(AudioClip thisCall, bool thisHangUp) // USED TO RESUME WHEN "ON HOLD"
    {
        // IF isOnHold....
        // Load Call into Source
        // Set hangUp status
        // call PlayCall();
    }

    void EndCall ()
    {
        // Release Button
        // Clear currentCall;
        // clear hangUp
        // Clear isConnected;
    }

    void PutOnHold(float thisHoldTime)
    {
        // Turn on looping
        // Switch to loopClip;
        // Play loopClip;
        // set isOnHold to true;
    }
}
