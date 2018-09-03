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
    private bool isRunning;
    [HideInInspector]
    public float clockTime = 0f;
    private MasterController myController;

	// Use this for initialization
	void Start () {
        myController = FindObjectOfType<MasterController>();
	}
	
    public void ResetMe()
    {
        clockTime = 0;
        isRunning = false;
        myDisplay.text = "00:00:00";
    }

	// Update is called once per frame
	void Update () {

        if (clockTime==0 || !isRunning)
        {
            myDisplay.text = "00:00:00";
        }
        else
        {
        clockTime -= Time.deltaTime;
        if (clockTime<0)
        {
            clockTime = 0;
            isRunning = false;
            myController.ClockAtZero();
        }

        minutes = Mathf.Floor (clockTime / 60);
        minutesString = (minutes.ToString());
        if (minutes<10) { minutesString = "0" + minutesString; }

        seconds = Mathf.Floor (clockTime) % 60;
        secondsString = (seconds.ToString());
        if (seconds < 10) { secondsString = "0" + secondsString; }

        frames = Mathf.Floor ((clockTime - Mathf.Floor(clockTime))*30);
        framesString = (frames.ToString());
        if (frames < 10) { framesString = "0" + framesString; }

        myDisplay.text = (minutesString+":"+secondsString+":"+framesString);

        }
    }

    public void SetTimeAndHold(float thisSetTime, bool shouldHold)
    {
        // Set Clock to thisSetTime
        clockTime = thisSetTime;
        // if shouldHold turn off isRunning;
        isRunning = true;
        if (shouldHold)
        {
            isRunning = false;
        }
    }

    public void StartClock()
    {
        isRunning = true;
    }
}
