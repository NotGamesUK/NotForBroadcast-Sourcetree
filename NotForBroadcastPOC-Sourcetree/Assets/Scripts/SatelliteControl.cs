using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteControl : MonoBehaviour {

    private ButtonAnimating myButton;

	// Use this for initialization
	void Awake () {
        myButton = GetComponentInChildren<ButtonAnimating>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PowerOn()
    {
        myButton.hasPower = true;
    }

    void PowerOff ()
    {
        myButton.hasPower = false;
    }
}
