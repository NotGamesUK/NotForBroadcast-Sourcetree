using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSControlPanel : MonoBehaviour {

    public VHSPlayerSelectionButton[] selectionButtons;
    public ButtonAnimating playButton;

    private bool hasPower = false;
    public int selectedPlayer = 0;

	// Use this for initialization
	void Start () {
        playButton.isLocked = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PowerOn()
    {
        foreach (VHSPlayerSelectionButton thisPlayer in selectionButtons)
        {
            thisPlayer.myButton.hasPower = true;
        }
        playButton.hasPower = true;
        hasPower = true;

    }

    public void PowerOff()
    {
        foreach (VHSPlayerSelectionButton thisPlayer in selectionButtons)
        {
            thisPlayer.myButton.hasPower = false;
        }
        playButton.hasPower = false;
        hasPower = false;

    }

    public void VHSPlayerSelected(int requestedPlayer)
    {
        foreach(VHSPlayerSelectionButton thisPlayer in selectionButtons)
        {
            if (thisPlayer.myButton.isDepressed && thisPlayer.myID != requestedPlayer)
            {
                //Debug.Log("Lifting Button " + thisButton.myID);
                thisPlayer.myButton.MoveUp();
            }
        }
        selectedPlayer = requestedPlayer;
    }

    public void PlayButtonPressed()
    {
        // if ready to go (ie player is loaded and not currently playing) tell selected player to play tape.

    }
}
