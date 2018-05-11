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
    private VHSTape myTape;
    private TapeLoader myLoader;
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

    void EjectTape()
    {
        Debug.Log("Machine " + myID + " EJECTING tape " + myTape.myTitle);
        myLoader.EjectTape();
        isAnimating = true;

    }
}
