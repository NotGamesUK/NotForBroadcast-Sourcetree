using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSPlayerSelectionButton : MonoBehaviour {

    [HideInInspector]
    public ButtonAnimating myButton;
    public VHSPlayer myPlayer;
    public int myID;

    private bool lastCheck;
    [HideInInspector]
    public VHSControlPanel myBox;


    // Use this for initialization
    void Start () {
        myButton = GetComponent<ButtonAnimating>();
        myBox = GetComponentInParent<VHSControlPanel>();
        lastCheck = myButton.isDepressed;
        myButton.isLocked = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (lastCheck != myButton.isDepressed)
        {
            //Debug.Log("State of button changed.");
            lastCheck = myButton.isDepressed;
            if (lastCheck == true)
            {
                myBox.VHSPlayerSelected(myID);
                //myButton.isLocked = true;
            }
        }

    }
}
