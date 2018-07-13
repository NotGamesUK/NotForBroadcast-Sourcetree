using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringController : MonoBehaviour {

    public float minimumAudiencePercentage, maxGreenSecsMultiCam, maxOrangeSecsMultiCam, maxOrangeSecsSingleCam;
    [Range (0, 5f)]
    public float footageWeight, audioWeight, interferenceWeight, resistanceWeight;

    private MasterController myMasterController;
    private AudienceVUMeter myVUMeter;
    public enum ScoringMode { SingleCam, MultiCam }
    [HideInInspector]
    public ScoringMode myScoringMode;

    private float audiencePercentage;
    public float footageWeighting, footageCountdown;
    private float audioWeighting;
    private float interferenceWighting;
    private ScoringPlane.ScoreColour currentFootageColour;


    /// FOR TESTING:
    /// 
    private float testPercentageDELETEME = 60;


    // Use this for initialization
    void Start () {
        myMasterController = GetComponent<MasterController>();
        myVUMeter = FindObjectOfType<AudienceVUMeter>();
        myScoringMode = ScoringMode.SingleCam;
        audiencePercentage = 70;
        /// For Testing
        /// 
        //Invoke("TEMPRandomVUAdjust", 0.2f);


    }

    void TEMPRandomVUAdjust()
    {
        testPercentageDELETEME += Random.Range(-3, 3.1f);
        if (testPercentageDELETEME < 0) { testPercentageDELETEME = 0; }
        if (testPercentageDELETEME > 100) { testPercentageDELETEME = 100; }
        myVUMeter.SetToPercentage(testPercentageDELETEME);
        Invoke("TEMPRandomVUAdjust", 0.2f);

    }


    // Update is called once per frame
    void Update () {

        // Adjust Footage Fail Countdown

        if (footageCountdown > 0)
        {
            footageCountdown -= Time.deltaTime;
            if (footageCountdown <= 0)
            {
                footageWeighting = -1;
                footageCountdown = -1;
            }
        }

        // Make Adjustment based on weighting
        audiencePercentage += footageWeighting * Time.deltaTime*footageWeight;

        if (audiencePercentage < 0) { audiencePercentage = 0; }
        if (audiencePercentage > 100) { audiencePercentage = 100; }
        myVUMeter.SetToPercentage(audiencePercentage);
        if (audiencePercentage < minimumAudiencePercentage)
        {
            string failString = "Audience Fell Below " + minimumAudiencePercentage.ToString() + "%";
            myMasterController.FailMidLevel(failString);
        }
    }

    public void FootageColourChange(ScoringPlane.ScoreColour thisColour)
    {

        Debug.Log("Scoring Controller: Screen Colour Changed.");

        switch (thisColour)
        {
            case ScoringPlane.ScoreColour.Red:
                footageWeighting = -1;
                footageCountdown = -1;
                break;

            case ScoringPlane.ScoreColour.Orange:
                footageWeighting = 0;
                switch (myScoringMode)
                {
                    case ScoringMode.SingleCam:
                        footageCountdown = maxOrangeSecsSingleCam;
                        break;

                    case ScoringMode.MultiCam:
                        footageCountdown = maxOrangeSecsMultiCam;
                        break;
                }
                break;

            case ScoringPlane.ScoreColour.Green:
                footageWeighting = 1;
                footageCountdown = -1;
                if (myScoringMode == ScoringMode.MultiCam)
                {
                    footageCountdown = maxGreenSecsMultiCam;
                }
                break;

        }
        currentFootageColour = thisColour;
    }


}
