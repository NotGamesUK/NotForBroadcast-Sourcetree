using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingDesk : MonoBehaviour {

    public GroovedSlider[] mySliders;
    public ButtonAnimating[] myMutes;
    public ButtonAnimating myBleepButton;
    public bool hasPower=false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PowerOn()
    {
        hasPower = true;
        foreach (GroovedSlider thisSlider in mySliders)
        {
            thisSlider.hasPower = true;
        }
        foreach (ButtonAnimating thisMute in myMutes)
        {
            thisMute.hasPower = true;
        }
        myBleepButton.hasPower = true;
    }

    void PowerOff()
    {
        hasPower = false;
        foreach (GroovedSlider thisSlider in mySliders)
        {
            thisSlider.hasPower = false;
        }
        foreach (ButtonAnimating thisMute in myMutes)
        {
            thisMute.hasPower = false;
        }
        myBleepButton.hasPower = false;

    }
}

