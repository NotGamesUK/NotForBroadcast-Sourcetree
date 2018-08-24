using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVStatusLEDs : MonoBehaviour {

    public int myTvId;
    public LED myLED;
    public VideoPlayer myScreen;

    private ScoringController myScoringController;
    private ScoringData.ScoreColour lastColour;
    private BroadcastTV myBroadcastSystem;
    private VisionMixer myVisionMixer;

	// Use this for initialization
	void Start () {
        myScoringController = FindObjectOfType<ScoringController>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myBroadcastSystem = FindObjectOfType<BroadcastTV>();
        lastColour = ScoringData.ScoreColour.Green;
        }

    // Update is called once per frame
    void Update () {
		if (myScreen.isPlaying)
        {
            bool canDoIt = true;
            int thisCheckID = myTvId;
            if (thisCheckID == -1)
            {
                // Master Screen
                thisCheckID = myVisionMixer.currentScreen + 3;
                if (myVisionMixer.currentScreen==0)
                {
                    canDoIt = false;
                }

            } else if (thisCheckID == -2)
            {
                // Broadcast Screen
                thisCheckID = myBroadcastSystem.currentScreen - 1;
                Debug.Log("THIS CHECK ID: " + thisCheckID);
                if (!myScoringController.broadcastScreensLive || thisCheckID<0)
                {
                    canDoIt = false;
                }
            }
            if (canDoIt)
            {

                if (lastColour != myScoringController.screenVideoColour[thisCheckID])
                {
                    lastColour = myScoringController.screenVideoColour[thisCheckID];
                    switch (lastColour)
                    {
                        case ScoringData.ScoreColour.Red:
                            myLED.GoRed();
                            break;


                        case ScoringData.ScoreColour.Orange:
                            myLED.GoOrange();

                            break;


                        case ScoringData.ScoreColour.Green:
                            myLED.GoGreen();

                            break;


                        case ScoringData.ScoreColour.Null:
                            myLED.TurnOff();

                            break;






                    }
                }
            }
        }
        else
        {
            if (lastColour != ScoringData.ScoreColour.Null)
            {
                lastColour = ScoringData.ScoreColour.Null;
                myLED.TurnOff();
            }
        }
	}
}
