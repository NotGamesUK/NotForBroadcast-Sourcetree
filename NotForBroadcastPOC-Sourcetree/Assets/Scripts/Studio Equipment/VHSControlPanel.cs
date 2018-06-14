using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSControlPanel : MonoBehaviour
{

    public VHSPlayerSelectionButton[] selectionButtons;
    public ButtonAnimating playButton;

    private SequenceController mySequenceController;
    private bool hasPower = false;
    private bool isPlayingTape = false;
    public int selectedPlayer = 0;

    // Use this for initialization
    void Start()
    {
        playButton.Lock();
        playButton.oneWay = true;
        mySequenceController = FindObjectOfType<SequenceController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playButton.isDepressed && !isPlayingTape)
        {
            Debug.Log("VHS CONTROL PANEL PLAY BUTTON NOW DEPRESSED.");
            PlayButtonPressed();
        }
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
        if (!isPlayingTape)
        {
            foreach (VHSPlayerSelectionButton thisVHSSelectionButton in selectionButtons)
            {
                if (thisVHSSelectionButton.myButton.isDepressed && thisVHSSelectionButton.myID != requestedPlayer)
                {
                    //Debug.Log("Lifting Button " + thisButton.myID);
                    thisVHSSelectionButton.myButton.MoveUp();
                    thisVHSSelectionButton.myButton.Unlock();
                }
            }
            selectedPlayer = requestedPlayer;
            if (selectedPlayer != 0)
            {

                playButton.hasPower = true;
                playButton.Unlock();
            }
            else
            {
                playButton.hasPower = false;
                playButton.Lock();
            }
        }
    }

    public void PlayButtonPressed()
    {
        // if ready to go (ie player is loaded and not currently playing) tell selected player to play tape.
        Debug.Log("Play Button Pressed");
        Debug.Log("Selected Player = " + selectedPlayer);
        VHSPlayer[] thesePlayers = FindObjectsOfType<VHSPlayer>();
        string thisName = "";
        foreach (VHSPlayer thisPlayer in thesePlayers)
        {
            if (thisPlayer.myID == selectedPlayer)
            {
                thisName = thisPlayer.myTape.myTitle;
                thisPlayer.isPlaying = true;
                thisPlayer.myButton.isLocked = true;
            }
        }
        Debug.Log("Selected Advert: " + thisName);
        mySequenceController.EndSequenceAndPlayAdvert(thisName);
        isPlayingTape = true;
    }

    public void TapeComplete()
    {
        isPlayingTape = false;
        playButton.MoveUp();
        VHSPlayer[] thesePlayers = FindObjectsOfType<VHSPlayer>();
        foreach (VHSPlayer thisPlayer in thesePlayers)
        {
            if (thisPlayer.myID == selectedPlayer)
            {
                thisPlayer.isPlaying = false;
                thisPlayer.myButton.isLocked = false;
            }
        }
    }
}
