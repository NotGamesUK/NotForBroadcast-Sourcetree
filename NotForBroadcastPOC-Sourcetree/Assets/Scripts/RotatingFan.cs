using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFan : MonoBehaviour {

    public GameObject myBlades;
    public GameObject myHead;
    public float bladeMaxSpeed;
    public float bladeAcceleration;
    public float bladeDeceleration;
    public float turnSpeed;
    public float maxTurnAngle;
    public float currentTurnSpeed;
    [HideInInspector]
    public bool isTurning = false;
    [HideInInspector]
    public float currentBladeSpeed;
    public AudioClip mySpinUpSFX;
    public AudioClip mySlowDownSFX;
    public AudioClip myBladesSFX;
    public AudioClip myTurningSFX;
    private bool lastButtonCheck;




    private ButtonAnimating myButton;
    private AudioSource mySFX;
    private AudioSource myTurnSFX;


    // Use this for initialization
    void Awake () {
        myButton = GetComponentInChildren<ButtonAnimating>();
        mySFX = GetComponent<AudioSource>();
	}

    private void Start()
    {
        currentTurnSpeed = turnSpeed;
        myTurnSFX = myHead.GetComponent<AudioSource>();
        myTurnSFX.loop = true;
        myTurnSFX.clip = myTurningSFX;
        myTurnSFX.Stop();
        lastButtonCheck = myButton.isDepressed;
        isTurning = false;
        if (!lastButtonCheck) { isTurning = true; }
    }
    // Update is called once per frame
    void Update () {
		if (myButton.hasPower)
        {
            if (mySFX.clip == mySpinUpSFX)
            {
                if (!mySFX.isPlaying)
                {
                    mySFX.clip = myBladesSFX;
                    mySFX.loop = true;
                    mySFX.Play();
                }
            }
            // Rotate Blades
            currentBladeSpeed += bladeAcceleration*Time.deltaTime;
            if (currentBladeSpeed>bladeMaxSpeed) { currentBladeSpeed = bladeMaxSpeed; }
            myBlades.transform.Rotate(new Vector3(0, currentBladeSpeed * Time.deltaTime, 0));
            if (lastButtonCheck != myButton.isDepressed)
            {
                lastButtonCheck = myButton.isDepressed;
                if (lastButtonCheck)
                {
                    isTurning = false;
                    myTurnSFX.Stop();
                } else
                {
                    isTurning = true;
                    myTurnSFX.Play();
                }
            }
            if (isTurning)
            {
                isTurning = true;
                // Turn Head
                myHead.transform.Rotate(new Vector3(0, 0, currentTurnSpeed * Time.deltaTime));
                float thisRot = myHead.transform.localEulerAngles.z;
                if (thisRot >= 180+maxTurnAngle)
                {
                    currentTurnSpeed = -turnSpeed;
                }
                if (thisRot <=180-maxTurnAngle)
                {
                    currentTurnSpeed = turnSpeed;
                }
            }
        } else
        {
            
            // No Power.  Decelerate Blades if required.
            if (currentBladeSpeed>0)
            {
                currentBladeSpeed -= bladeDeceleration*Time.deltaTime;
                if (currentBladeSpeed<0) { currentBladeSpeed = 0; }
                myBlades.transform.Rotate(new Vector3(0, currentBladeSpeed * Time.deltaTime, 0));
            }
        }
	}

    void PowerOn()
    {
        myButton.hasPower = true;
        mySFX.clip = mySpinUpSFX;
        mySFX.loop = false;
        mySFX.Play();
        if (isTurning)
        {
            myTurnSFX.Play();
        }

    }

    void PowerOff()
    {
        myButton.hasPower = false;
        mySFX.clip = mySlowDownSFX;
        mySFX.loop = false;
        mySFX.Play();
        if (isTurning)
        {
            myTurnSFX.Stop();
        }

    }
}
