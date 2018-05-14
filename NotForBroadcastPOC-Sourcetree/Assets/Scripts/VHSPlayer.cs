using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSPlayer : MonoBehaviour {

    public ButtonAnimating myButton;
    public int myID;
    public bool isLoaded = false;
    public bool isPlaying = false;
    public bool isAnimating = false;
    public bool hasPower = false;
    //[HideInInspector]
    public  VHSTape myTape;
    [HideInInspector]
    public  TapeLoader myLoader;
    public VHSPlayerSelectionButton mySelectionButton;
    private bool lastButtonCheck;

	// Use this for initialization
	void Start () {
        myLoader = GetComponentInChildren<TapeLoader>();
        lastButtonCheck = myButton.isDepressed;
        myButton.isLocked = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (lastButtonCheck != myButton.isDepressed)
        {
            lastButtonCheck = myButton.isDepressed;
            if (lastButtonCheck==false) // Eject Has Been Pressed
            {
                EjectTape();
            }
        }
	}

    public void LoadTape(VHSTape thisTape)
    {
        myTape = thisTape;
        Debug.Log("Machine " + myID + " loading tape " + myTape.myTitle);

        myLoader.LoadTape(myTape);
        isAnimating = true;
    }

    public void LoadComplete()
    {
        isAnimating = false;
        isLoaded = true;
        myButton.isLocked = false;
        mySelectionButton.myButton.isLocked = false;
        myButton.MoveDown();

    }

    void EjectTape()
    {
        Debug.Log("Machine " + myID + " EJECTING tape " + myTape.myTitle);
        myLoader.EjectTape();
        isLoaded = false;
        if (mySelectionButton.myButton.isDepressed)
        {
            mySelectionButton.myButton.MoveUp();
            mySelectionButton.myBox.VHSPlayerSelected(0);

        }
        myButton.isLocked = true;
        mySelectionButton.myButton.isLocked = true;
        isAnimating = true;

    }
}
