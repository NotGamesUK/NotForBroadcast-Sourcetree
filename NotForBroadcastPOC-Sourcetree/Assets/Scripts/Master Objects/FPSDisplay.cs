using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour {

    float deltaTime = 0.0f;
    private ScoringController myScoringController;
    private MasterController myMasterController;
    private SequenceController mySequenceController;
    private EDLController myEDLController;
    private EventController myEventController;

    

    private void Start()
    {
        myScoringController = FindObjectOfType<ScoringController>();
        myMasterController = FindObjectOfType<MasterController>();
        mySequenceController = FindObjectOfType<SequenceController>();
        myEDLController = FindObjectOfType<EDLController>();
        myEventController = FindObjectOfType<EventController>();
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;//new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        text += "\nFootage Countdown: "+myScoringController.footageCountdown;
        text += "\nBleeps Missed: ";
        text += "\nAudience Percentage: "+myScoringController.audiencePercentage;
        text += "\nFootage Weighting: " + myScoringController.footageWeighting;
        text += "\nInterference Weighting: " + myScoringController.interferenceWeighting;
        text += "\nAudio Weighting: " + myScoringController.audioWeighting;
        text += "\nBleep Weighting: " + myScoringController.bleepWeighting;
        text += "\nAudience Adjustment: " + myScoringController.thisAudienceChange;

        GUI.Label(rect, text, style);

        GUIStyle style02 = new GUIStyle();

        Rect rect02 = new Rect(0, 0, w, h * 2 / 100);
        style02.alignment = TextAnchor.UpperRight;
        style02.fontSize = h * 2 / 100;
        style02.normal.textColor = Color.white;//new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text02 = "";
        text02 += "MASTER CONTROLLER Current Level: " + myMasterController.currentLevel;
        text02 += "\nMASTER CONTROLLER Current Sequence: " + myMasterController.currentSequence;
        text02 += "\nSEQUENCE CONTROLLER Current Sequence: " + mySequenceController.sequenceName;
        text02 += "\nSCORING CONTROLLER Current Sequence: " + myScoringController.currentSequenceNumber;
        text02 += "\nEVENT CONTROLLER Phase: " + myEventController.myState;
        text02 += "\nEVENT CONTROLLER Advert Number: " + myEventController.currentAdvert;
        text02 += "\nEVENT CONTROLLER Sequence Number: " + myEventController.currentSequence;

        GUI.Label(rect02, text02, style02);

    }
}
