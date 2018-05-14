using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMixerButton : MonoBehaviour {

    public int myID;

    public ButtonAnimating myButton;
    private VisionMixer visionMixer;
    private bool lastCheck;

	// Use this for initialization
	void Start () {
        visionMixer = GetComponentInParent<VisionMixer>();
        lastCheck = myButton.isDepressed;
        myButton.isLocked = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (lastCheck!=myButton.isDepressed)
        {
            //Debug.Log("State of button changed.");
            lastCheck = myButton.isDepressed;
            if (lastCheck==true) {
                visionMixer.ScreenChange(myID);
                myButton.isLocked = true;
            }
        }
	}
}
