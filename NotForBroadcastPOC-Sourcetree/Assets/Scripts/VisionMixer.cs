using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMixer : MonoBehaviour {

    public VisionMixerButton[] buttons;
    public PlugSocket myPlug;
    private bool lastPower;
    private bool lastOn;

	// Use this for initialization
	void Start () {
        lastPower = myPlug.hasPower;
        lastOn = myPlug.isOn;
        PowerOff();
        if (myPlug.isOn && myPlug.hasPower)
        {
            PowerOn();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (myPlug.hasPower != lastPower || myPlug.isOn!=lastOn )
        {
            if (myPlug.hasPower && myPlug.isOn)
            {
                PowerOn();
            } else
            {
                PowerOff();
            }
        }
        lastPower = myPlug.hasPower;
        lastOn = myPlug.isOn;
    }

    public void ScreenChange(int selectedScreen)
    {
        Debug.Log("Switched to Screen " + selectedScreen);
        foreach(VisionMixerButton thisButton in buttons)
        {
            if (thisButton.myButton.isDepressed && thisButton.myID != selectedScreen) {

                Debug.Log("Lifting Button " + thisButton.myID);
                thisButton.myButton.MoveUp();
            }
        }
    }

    public void PowerOn()
    {
        foreach (VisionMixerButton thisButton in buttons)
        {
            thisButton.myButton.hasPower = true;
        }
    }

    public void PowerOff()
    {
        foreach (VisionMixerButton thisButton in buttons)
        {
            thisButton.myButton.hasPower = false;
        }
    }

}
