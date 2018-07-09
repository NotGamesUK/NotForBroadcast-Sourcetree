using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public bool freeLook = false;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Animator myAnimator;
    private string myPosition="Centre";
    private Vector3 startPosition;
    private float startFOV;
    private Camera myCamera;
    private float currentFocusDistance, currentAperture, targetFocusDistance, targetAperture;


    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        startPosition = this.transform.position;
        myCamera = GetComponent<Camera>();
        startFOV = myCamera.fieldOfView;
        //if (freeLook)
        //{
        //    FreeLookToggle(true);
        //}
    }

    void Update()
    {
        if (freeLook)
        {
            // Right Click Mouse Control

            if (Input.GetMouseButton(1))
            {
                yaw += speedH * Input.GetAxis("Mouse X");
                pitch -= speedV * Input.GetAxis("Mouse Y");

                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
        } else
        {
            // Keyboard Controls
            if (Input.GetButtonDown("Down"))
            {
                if (myPosition!="Down")
                {
                    myAnimator.SetTrigger("downPressed");
                    myPosition = "Down";
                }
            }

            if (Input.GetButtonDown("Up"))
            {
                if (myPosition == "Down")
                {
                    myAnimator.SetTrigger("upPressed");
                    myPosition = "Centre";
                }
            }

            if (Input.GetButtonDown("Left"))
            {
                if (myPosition != "Left")
                {
                    myAnimator.SetTrigger("leftPressed");
                    if (myPosition == "Right")
                    {
                        myPosition = "Centre";
                    }
                    else
                    {
                        myPosition = "Left";
                    }
                }
            }

            if (Input.GetButtonDown("Right"))
            {
                if (myPosition != "Right")
                {
                    myAnimator.SetTrigger("rightPressed");
                    if (myPosition == "Left")
                    {
                        myPosition = "Centre";
                    }
                    else
                    {
                        myPosition = "Right";
                    }
                }
            }

        }

    }

    public void FreeLookToggle(bool thisSetting)
    {
        if (thisSetting)
        {
            myAnimator.enabled = false;
            this.transform.position = new Vector3(0f, 0.91f, -0.04f);
            myCamera.fieldOfView = 68;
            freeLook = true;
        }
        else
        {
            myAnimator.enabled = true;
            this.transform.position = startPosition;
            myCamera.fieldOfView = startFOV;
            freeLook = false;
        }
    }

    public void PullFocusOverTime(float thisFocusDistance, float thisAperture, float thisTime)
    {

    }

}