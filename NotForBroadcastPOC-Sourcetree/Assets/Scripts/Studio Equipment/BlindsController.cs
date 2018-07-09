using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindsController : MonoBehaviour {

    private ButtonAnimating[] myButtons;

	// Use this for initialization
	void Start () {
        myButtons = GetComponentsInChildren<ButtonAnimating>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PowerOn()
    {
        foreach (ButtonAnimating thisButton in myButtons)
        {
            thisButton.hasPower = true;
        }
        // Mute all sound
    }

    void PowerOff()
    {
        foreach (ButtonAnimating thisButton in myButtons)
        {
            thisButton.hasPower = false;
        }
        // Enable all sound
    }

}
