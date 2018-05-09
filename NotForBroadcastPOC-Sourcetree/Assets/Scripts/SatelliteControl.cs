using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteControl : MonoBehaviour {

    private ButtonAnimating myButton;
    private SatelliteDish myDish;

	// Use this for initialization
	void Awake () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        myDish = FindObjectOfType<SatelliteDish>();
	}
	
	// Update is called once per frame
	void Update () {
		if (myButton.isDepressed && myButton.hasPower)
        {
            myDish.RaiseDish();
        }
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
