using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Speakers : MonoBehaviour {

    public AudioMixer myDesk;
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
        // UnMute all sound
        myDesk.SetFloat("MasterVol", 0f);
    }

    void PowerOff()
    {
        foreach (Light thisLight in powerLEDs)
        {
            thisLight.enabled = false;
        }
        // Mute all sound
        myDesk.SetFloat("MasterVol", -80f);

    }

}
