using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakers : MonoBehaviour {

    private Light[] powerLEDs;

	// Use this for initialization
	void Start () {
        powerLEDs = GetComponentsInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PowerOn ()
    {
        foreach (Light thisLight in powerLEDs)
        {
            thisLight.enabled = true;
        }
        // Mute all sound
    }

    void PowerOff()
    {
        foreach (Light thisLight in powerLEDs)
        {
            thisLight.enabled = false;
        }
        // Enable all sound
    }

}
