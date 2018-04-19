using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMixer : MonoBehaviour {

    public VisionMixerButton[] buttons;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
