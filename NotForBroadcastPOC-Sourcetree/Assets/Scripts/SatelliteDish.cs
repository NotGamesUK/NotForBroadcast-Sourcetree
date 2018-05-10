using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteDish : MonoBehaviour {

    public float dropSpeed;
    public float raiseSpeed;
    public float idealTilt = 310;
    public float minTilt = 290;
    public float maxTilt = 350;
    public float turnSpeed = 10;
    //[HideInInspector]
    public float targetTurn;
    public float minTurn = 15;
    public float maxTurn = 95;
    private int turnDirection=0;
    private float currentTurn;
    private float lastTargetTurn;
    private float sliderRange;
    private PlayerFrequencyDisplayObject myReadout;


    // Use this for initialization
    void Start () {
        currentTurn = transform.eulerAngles.y;
        targetTurn = currentTurn;
        lastTargetTurn = currentTurn;
        sliderRange = maxTurn - minTurn;
        myReadout = FindObjectOfType<PlayerFrequencyDisplayObject>();
        MoveTheDot();
    }

    // Update is called once per frame
    void Update()
    {
        // Dropping Dish

        if (dropSpeed > 0)
        {
            Quaternion localRotation = Quaternion.Euler(dropSpeed * Time.deltaTime, 0f, 0f);
            transform.rotation = transform.rotation * localRotation;
        }

        if (transform.localEulerAngles.x > maxTilt)
        {
            transform.eulerAngles = new Vector3(maxTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        // Turning Dish

        currentTurn = transform.eulerAngles.y;
        //Debug.Log("Current Turn: " + currentTurn);
        if (currentTurn != targetTurn)
        {
            //Quaternion localRotation = Quaternion.Euler(0f, turnSpeed * turnDirection * Time.deltaTime, 0f);
            //transform.rotation = transform.rotation * localRotation;
            transform.Rotate(0f, turnSpeed * turnDirection * Time.deltaTime, 0f, Space.World);
            currentTurn = Mathf.Round(transform.eulerAngles.y);
            if (turnDirection == -1)
            {
                if (currentTurn<targetTurn)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetTurn, transform.eulerAngles.z);
                    turnDirection = 0;
                }
            }
            if (turnDirection == 1)
            {
                if (currentTurn > targetTurn)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetTurn, transform.eulerAngles.z);
                    turnDirection = 0;
                }
            }

            MoveTheDot();
        }


        if (lastTargetTurn != targetTurn)
        {
            MakeTurnTo(targetTurn);
        }
        lastTargetTurn = targetTurn;
    }

    void MoveTheDot()
    {
        // Move PlayerDisplayObject to reflect current turn

        float displayTurn = currentTurn - minTurn;
        //Debug.Log("Display Turn pre Divide: " + displayTurn);
        displayTurn = (displayTurn / sliderRange);
        //Debug.Log("Sent to Dot as turn: " + displayTurn);
        myReadout.MoveDotTo(displayTurn);

    }

    void MakeTurnTo(float thisTargetTurn)
    {
        //Debug.Log("Make Turn Called.");
        thisTargetTurn = Mathf.Clamp(thisTargetTurn, minTurn, maxTurn);
        targetTurn = thisTargetTurn;
        if (targetTurn > currentTurn) { turnDirection = 1; }
        if (targetTurn < currentTurn) { turnDirection = -1; }
    }

    public void SignalSliderChange (float thisSliderSetting)
    {
        float tempTarget = minTurn + (thisSliderSetting * sliderRange);
        MakeTurnTo(tempTarget);
    }

    public void RaiseDish()
    {
        Quaternion localRotation = Quaternion.Euler(-raiseSpeed * Time.deltaTime, 0f, 0f);
        transform.rotation = transform.rotation * localRotation;

        if (transform.localEulerAngles.x < minTilt)
        {
            transform.eulerAngles = new Vector3(minTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        }

    }
}
