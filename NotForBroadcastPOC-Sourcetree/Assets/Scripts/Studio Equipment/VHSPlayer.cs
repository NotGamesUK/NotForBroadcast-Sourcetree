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
    private AudioSource mySFX;
    public AudioClip myLoadSFX;
    public AudioClip myEjectSFX;
    private VHSControlPanel myControlPanel;

	// Use this for initialization
	void Start () {
        myLoader = GetComponentInChildren<TapeLoader>();
        myControlPanel = FindObjectOfType<VHSControlPanel>();
        lastButtonCheck = myButton.isDepressed;
        myButton.Lock();
        mySFX = GetComponent<AudioSource>();
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
        //Debug.Log("Machine " + myID + " loading tape " + myTape.myTitle);
        mySFX.clip = myLoadSFX;
        mySFX.Play();
        myLoader.LoadTape(myTape);
        isAnimating = true;
    }

    public void LoadComplete()
    {
        isAnimating = false;
        isLoaded = true;
        if (!myControlPanel.isPlayingTape)
        {
            mySelectionButton.myButton.Unlock();
        }
        myButton.Unlock();
        myButton.MoveDown();
        mySFX.Stop();
    }

    public void EjectTape()
    {
        Debug.Log("Machine " + myID + " EJECTING tape " + myTape.myTitle);
        myLoader.EjectTape();
        isLoaded = false;
        if (mySelectionButton.myButton.isDepressed && !isPlaying)
        {
            mySelectionButton.myButton.MoveUp();
            mySelectionButton.myBox.VHSPlayerSelected(0);

        }
        myButton.Lock();
        mySelectionButton.myButton.Lock();
        isAnimating = true;
        mySFX.clip = myEjectSFX;
        mySFX.Play();
    }
}
