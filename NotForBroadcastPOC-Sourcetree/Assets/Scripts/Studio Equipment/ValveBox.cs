using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveBox : MonoBehaviour {

    [HideInInspector]
    public bool isOverheated = false;
    //[HideInInspector]
    public int valveCount=0;
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
    public Material elementMaterial;
    public AudioClip myHumSFX;
    public AudioClip mySizzleSFX;


    private MasterGauges myGauges;
    private RotatingFan myFan;
    private AudioSource[] mySFX;

	// Use this for initialization
	void Start () {
        myGauges = FindObjectOfType<MasterGauges>();
        currentTemperature = myGauges.roomTemperature;
        myFan = FindObjectOfType<RotatingFan>();
        mySFX = GetComponents<AudioSource>();
        mySFX[0].clip = myHumSFX;
        mySFX[0].loop = true;
        mySFX[0].volume = 0;
        mySFX[0].Play();
        mySFX[1].clip = mySizzleSFX;
        mySFX[1].loop = true;
        mySFX[1].volume = 0;
        mySFX[1].Play();


    }

    // Update is called once per frame
    void Update () {
        // Add Heat Gain from Power Status
        float thisHeatGain = 0;
        switch (myGauges.myPowerStatus)
        {
            case ("Green"):
                thisHeatGain = greenHeatUpSpeed * Time.deltaTime;
                break;

            case ("Orange"):
                thisHeatGain = orangeHeatUpSpeed * Time.deltaTime;
                break;

            case ("Red"):
                thisHeatGain = redHeatUpSpeed * Time.deltaTime;
                break;

            case ("None"):
                //Debug.Log("System on but no power draining.");
                break;

            default:
                Debug.LogError("Valve Box: No Matching Power Status Set");
                break;

        }
        switch (valveCount)
        {
            case (3):
                thisHeatGain *= 1.33f;
                break;

            case (2):
                thisHeatGain *= 2f;
                break;

            case (1):
                thisHeatGain *= 4f;
                break;
        }
        currentTemperature += thisHeatGain;


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


        float emission = currentTemperature / myGauges.maxTemperature * 5;
        float humVolume = emission / 10;
        float sizzleVolume = Mathf.Clamp(emission - 4, 0f, 1f);
        //Debug.Log("Emission: " + emission + "   Hum: " + humVolume + "   Sizzle" + sizzleVolume);
        Color baseColor = Color.Lerp(Color.red, Color.yellow, currentTemperature / myGauges.maxTemperature); //Replace this with whatever you want for your base color at emission level '1'

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        elementMaterial.SetColor("_EmissionColor", finalColor);

        mySFX[0].volume = humVolume;
        mySFX[1].volume = sizzleVolume;
    }
}
