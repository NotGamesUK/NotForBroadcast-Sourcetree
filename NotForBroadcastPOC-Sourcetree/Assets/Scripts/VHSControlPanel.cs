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
        playButton.hasPower = false;
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
        Debug.Log("Selection Request from Button " + requestedPlayer);

        foreach (VHSPlayerSelectionButton thisVHSSelectionButton in selectionButtons)
        {
            if (thisVHSSelectionButton.myButton.isDepressed && thisVHSSelectionButton.myID != requestedPlayer)
            {
                //Debug.Log("Lifting Button " + thisButton.myID);
                thisVHSSelectionButton.myButton.MoveUp();
                thisVHSSelectionButton.myButton.isLocked = false;
            }
        }
        selectedPlayer = requestedPlayer;
        if (selectedPlayer != 0)
        {

            playButton.hasPower = true;
            playButton.isLocked = false;
        } else
        {
            playButton.hasPower = false;
            playButton.isLocked = true;
        }
    }

    public void PlayButtonPressed()
    {
        // if ready to go (ie player is loaded and not currently playing) tell selected player to play tape.
        Debug.Log("Play Button Pressed");
    }
}
