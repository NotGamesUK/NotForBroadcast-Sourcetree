using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class SatelliteControl : MonoBehaviour {

    public PostProcessingProfile myPostProcessing;
    public float towerFocusDistance, towerAperture, towerFocusPullTime;

    private ButtonAnimating myButton;
    private SatelliteDish myDish;
    private bool isActive, lastActive;
    private CameraMovement mainCamera;

	// Use this for initialization
	void Awake () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        myDish = FindObjectOfType<SatelliteDish>();
	}

    private void Start()
    {
        lastActive = isActive;
        mainCamera = FindObjectOfType<CameraMovement>();
    }

    // Update is called once per frame
    void Update () {
		if (myButton.isDepressed && myButton.hasPower)
        {
            myDish.RaiseDish();
            isActive = true;

        }
        else
        {
            myDish.isRaising = false;
            isActive = false;
        }
        if (isActive != lastActive)
        {
            Debug.Log("Satellite Control: Sending Focus Pull");
            if (isActive)
            {
                // Pull Focus to Tower.
                mainCamera.PullFocusOverTime(towerFocusDistance, towerAperture, towerFocusPullTime);

            } else
            {
                // Pull Focus to Desk.
                mainCamera.ResetFocus();

            }
            lastActive = isActive;
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
