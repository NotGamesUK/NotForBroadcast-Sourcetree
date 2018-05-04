using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingDesk : MonoBehaviour {

    public GroovedSlider[] mySliders;
    public bool hasPower=false;

	// Use this for initialization
	void Start () {
        mySliders = GetComponentsInChildren<GroovedSlider>();
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
    }

    void PowerOff()
    {
        hasPower = false;
        foreach (GroovedSlider thisSlider in mySliders)
        {
            thisSlider.hasPower = false;
        }
    }
}

