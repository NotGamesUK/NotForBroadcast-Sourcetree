using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class SatelliteControl : MonoBehaviour {

    public PostProcessingProfile myPostProcessing;

    private ButtonAnimating myButton;
    private SatelliteDish myDish;

	// Use this for initialization
	void Awake () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        myDish = FindObjectOfType<SatelliteDish>();
	}

    private void Start()
    {

    }

    // Update is called once per frame
    void Update () {
		if (myButton.isDepressed && myButton.hasPower)
        {
            myDish.RaiseDish();
            var myDOF = myPostProcessing.depthOfField.settings;
            myDOF.focusDistance = 4.9f;
            myPostProcessing.depthOfField.settings = myDOF;

        }
        else
        {
            myDish.isRaising = false;
            var myDOF = myPostProcessing.depthOfField.settings;
            myDOF.focusDistance = 1.4f;
            myPostProcessing.depthOfField.settings = myDOF;

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
