using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSControlPanel : MonoBehaviour {

    public ButtonAnimating[] selectionButtons;
    public ButtonAnimating playButton;

    private bool hasPower = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PowerOn()
    {
        foreach (ButtonAnimating thisButton in selectionButtons)
        {
            thisButton.hasPower = true;
        }
        playButton.hasPower = true;
        hasPower = true;

    }

    public void PowerOff()
    {
        foreach (ButtonAnimating thisButton in selectionButtons)
        {
            thisButton.hasPower = false;
        }
        playButton.hasPower = false;
        hasPower = false;

    }
}
