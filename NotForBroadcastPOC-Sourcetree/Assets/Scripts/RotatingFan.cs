using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFan : MonoBehaviour {

    private ButtonAnimating myButton;
    private Animator[] tempAnimationControls;

	// Use this for initialization
	void Start () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        tempAnimationControls = GetComponentsInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PowerOn()
    {
        myButton.hasPower = true;
        // Start Blades and if button not depressed, rotation.
        foreach(Animator thisAnim in tempAnimationControls)
        {
            thisAnim.enabled = true;
        }
    }

    void PowerOff()
    {
        myButton.hasPower = false;
        // Stop Blades and Rotation
        foreach (Animator thisAnim in tempAnimationControls)
        {
            thisAnim.enabled = false;
        }

    }
}
