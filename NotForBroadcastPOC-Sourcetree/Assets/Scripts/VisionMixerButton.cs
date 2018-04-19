using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMixerButton : MonoBehaviour {

    public int myID;

    private ButtonAnimating myButton;
    private VisionMixer visionMixer;
    private bool lastCheck;

	// Use this for initialization
	void Start () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        visionMixer = GetComponentInParent<VisionMixer>();
        lastCheck = myButton.isDepressed;
	}
	
	// Update is called once per frame
	void Update () {
		if (lastCheck!=myButton.isDepressed)
        {
            Debug.Log("State of button changed.");
            lastCheck = myButton.isDepressed;
            visionMixer.ScreenChange(myID);
        }
	}
}
