using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveBox : MonoBehaviour {

    [HideInInspector]
    public bool isOverheated = false;
    public float greenHeatUpSpeed;
    public float orangeHeatUpSpeed;
    public float redHeatUpSpeed;
    public float coolDownSpeed;
    [Tooltip("Amount fan will cool the box while at a perfect angle.")]
    public float fanCoolDownSpeed;
    public float perfectFanAngle;
    [Tooltip ("The number of degrees the fan can travel from its perfect angle before cooling is less effective.")]
    public float fanPerfectDegrees;
    [Tooltip ("The number of degrees from perfect which the fan can turn before it has no effect.")]
    public float fanInfluenceDegrees;
    //[HideInInspector]
    public float currentTemperature;
    private MasterGauges myGauges;
    private RotatingFan myFan;

	// Use this for initialization
	void Start () {
        myGauges = FindObjectOfType<MasterGauges>();
        currentTemperature = myGauges.roomTemperature;
        myFan = FindObjectOfType<RotatingFan>();
    }

    // Update is called once per frame
    void Update () {
        // Add Heat Gain from Power Status
        switch (myGauges.myPowerStatus)
        {
            case ("Green"):
                currentTemperature += greenHeatUpSpeed * Time.deltaTime;
                break;

            case ("Orange"):
                currentTemperature += orangeHeatUpSpeed * Time.deltaTime;
                break;

            case ("Red"):
                currentTemperature += redHeatUpSpeed * Time.deltaTime;
                break;

            case ("None"):
                Debug.Log("System on but no power draining.");
                break;

            default:
                Debug.LogError("Valve Box: No Matching Power Status Set");
                break;

        }

        // If Fan is on subtract heat accordingly - never fall below room temperature
        if (myFan.currentBladeSpeed > 0)
        {
            float thisFanAngle=myFan.myHead.transform.localEulerAngles.z;
            if (thisFanAngle>(perfectFanAngle-fanPerfectDegrees) && thisFanAngle < (perfectFanAngle + fanPerfectDegrees))
            {
                currentTemperature -= fanCoolDownSpeed*Time.deltaTime;
            }
            else
            {
                if (thisFanAngle > (perfectFanAngle - fanInfluenceDegrees) && thisFanAngle < (perfectFanAngle + fanInfluenceDegrees))
                {
                    currentTemperature -= fanCoolDownSpeed / 4*Time.deltaTime; // DELETE WHEN SCALED COOLING IS ADDED
                }
            }
        }


        // Subtract cooldown.
        currentTemperature -= coolDownSpeed * Time.deltaTime;

        // Never fall below room temperature
        if (currentTemperature < myGauges.roomTemperature) { currentTemperature = myGauges.roomTemperature; }

        // If Heat exceeds maximum set "Is Overheated"
        if (currentTemperature > myGauges.maxTemperature)
        {
            isOverheated = true;
        }

        // if "Is Overheated" is set wait until at green heat levels and then turn off
        if (isOverheated)
        {
            float restartTemperature = myGauges.maxTemperature * (myGauges.orangeTemperaturePercent / 100);
            if (currentTemperature < restartTemperature)
            {
                isOverheated = false;
            }
        }
    }
}
