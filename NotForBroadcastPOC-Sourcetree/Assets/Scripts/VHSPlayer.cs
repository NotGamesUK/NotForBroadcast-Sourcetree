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

	// Use this for initialization
	void Start () {
        myLoader = GetComponentInChildren<TapeLoader>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadTape(VHSTape thisTape)
    {
        myTape = thisTape;
        isLoaded = true;
        Debug.Log("Machine " + myID + " loading tape " + myTape.myTitle);
        myLoader.LoadTape(myTape);
        isAnimating = true;
    }
}
