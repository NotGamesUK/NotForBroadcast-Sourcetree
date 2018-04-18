using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackWallClock : MonoBehaviour {

    public Text myDisplay;
    private float minutes=0;
    private string minutesString;
    private float seconds=0;
    private string secondsString;
    private float frames=0;
    private string framesString;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        minutes = Mathf.Floor (Time.time / 60);
        minutesString = (minutes.ToString());
        if (minutes<10) { minutesString = "0" + minutesString; }

        seconds = Mathf.Floor (Time.time) % 60;
        secondsString = (seconds.ToString());
        if (seconds < 10) { secondsString = "0" + secondsString; }

        frames = Mathf.Floor ((Time.time - Mathf.Floor(Time.time))*30);
        framesString = (frames.ToString());
        if (frames < 10) { framesString = "0" + framesString; }

        myDisplay.text = (minutesString+":"+secondsString+":"+framesString);
	}
}
