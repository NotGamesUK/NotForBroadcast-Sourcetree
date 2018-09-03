using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteDish : MonoBehaviour
{

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
    public AudioClip myFallingSFX;
    public AudioClip myRaisingSFX;
    public AudioClip myTurningSFX;


    private int turnDirection = 0;
    private float currentTurn;
    private float lastTargetTurn;
    private float sliderRange;
    private PlayerFrequencyDisplayObject myReadout;
    private AudioSource[] mySFX;
    public bool isDropping;
    //[HideInInspector]
    public bool isRaising;
    public bool isTurning;
    private bool lastRaising = false;
    private AudioSource myDropSFX;
    private AudioSource myRaiseSFX;
    private AudioSource myTurnSFX;
    [HideInInspector]
    public Quaternion myStartRotation;



    // Use this for initialization
    void Start()
    {
        currentTurn = transform.eulerAngles.y;
        targetTurn = currentTurn;
        lastTargetTurn = currentTurn;
        sliderRange = maxTurn - minTurn;
        myReadout = FindObjectOfType<PlayerFrequencyDisplayObject>();
        MoveTheDot();
        mySFX = GetComponents<AudioSource>();
        myDropSFX = mySFX[0];
        myDropSFX.clip = myFallingSFX;
        myDropSFX.loop = true;
        myDropSFX.Stop();
        myRaiseSFX = mySFX[1];
        myRaiseSFX.clip = myRaisingSFX;
        myRaiseSFX.loop = true;
        myRaiseSFX.Stop();
        myTurnSFX = mySFX[2];
        myTurnSFX.clip = myTurningSFX;
        myTurnSFX.loop = true;
        myTurnSFX.Stop();
        myStartRotation = transform.rotation;
    }

    public void ResetMe()
    {
        isRaising = false;
        isTurning = false;
        myRaiseSFX.Stop();
        myTurnSFX.Stop();
        myDropSFX.Stop();
        myReadout.MoveDotTo(0.5f);
    }


    // Update is called once per frame
    void Update()
    {
        bool lastTurning = isTurning;
        bool lastDropping = isDropping;
        isDropping = false;
        isTurning = false;
        // Dropping Dish

        if (dropSpeed > 0)
        {
            Quaternion localRotation = Quaternion.Euler(dropSpeed * Time.deltaTime, 0f, 0f);
            transform.rotation = transform.rotation * localRotation;
            isDropping = true;
        }

        if (transform.localEulerAngles.x > maxTilt)
        {
            transform.eulerAngles = new Vector3(maxTilt, transform.eulerAngles.y, transform.eulerAngles.z);
            isDropping = false;
        }

        // Turning Dish

        currentTurn = transform.eulerAngles.y;
        //Debug.Log("Current Turn: " + currentTurn);
        if (currentTurn != targetTurn)
        {
            //Quaternion localRotation = Quaternion.Euler(0f, turnSpeed * turnDirection * Time.deltaTime, 0f);
            //transform.rotation = transform.rotation * localRotation;
            transform.Rotate(0f, turnSpeed * turnDirection * Time.deltaTime, 0f, Space.World);
            isTurning = true;
            currentTurn = Mathf.Round(transform.eulerAngles.y);
            if (turnDirection == -1)
            {
                if (currentTurn < targetTurn)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetTurn, transform.eulerAngles.z);
                    turnDirection = 0;
                    isTurning = false;
                }
            }
            if (turnDirection == 1)
            {
                if (currentTurn > targetTurn)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetTurn, transform.eulerAngles.z);
                    turnDirection = 0;
                    isTurning = false;
                }
            }
            if (turnDirection==0) { isTurning = false; }

            MoveTheDot();

            // SoundManagement

        if (lastTurning != isTurning)
            {
                if (isTurning)
                {
                    //Debug.Log("Playing Turn Sound");
                    myTurnSFX.Play();
                } else
                {
                    //Debug.Log("Stopping Turn Sound");
                    myTurnSFX.Stop();
                }
            }

        if (lastRaising !=isRaising)
            {
                if (isRaising)
                {
                    //Debug.Log("Playing Raise Sound");
                    myRaiseSFX.Play();
                }
                else
                {
                    //Debug.Log("Stopping Raise Sound");
                    myRaiseSFX.Stop();
                }

            }

        if (lastDropping != isDropping)
            {
                if (isDropping)
                {
                    //Debug.Log("Playing Drop Sound");
                    myDropSFX.Play();
                }
                else
                {
                    //Debug.Log("Stopping Drop Sound");
                    myDropSFX.Stop();
                }

            }
            // Do this at the end to allow for a RaiseDish() call from the tower controller.

            lastRaising = isRaising;
        }


        if (lastTargetTurn != targetTurn)
        {
            MakeTurnTo(targetTurn);
        }
        lastTargetTurn = targetTurn;
    }

    public void MoveTheDot()
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

    public void SignalSliderChange(float thisSliderSetting)
    {
        float tempTarget = minTurn + (thisSliderSetting * sliderRange);
        MakeTurnTo(tempTarget);
    }

    public void RaiseDish()
    {
        Quaternion localRotation = Quaternion.Euler(-raiseSpeed * Time.deltaTime, 0f, 0f);
        transform.rotation = transform.rotation * localRotation;
        isRaising = true;
        if (transform.localEulerAngles.x < minTilt)
        {
            transform.eulerAngles = new Vector3(minTilt, transform.eulerAngles.y, transform.eulerAngles.z);
            isRaising = false;
        }

    }

    public void DropDish()
    {
        transform.eulerAngles = new Vector3(maxTilt, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}

