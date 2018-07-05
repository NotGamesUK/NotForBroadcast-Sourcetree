﻿using System.Collections;
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

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (freeLook)
        {
            myAnimator.enabled = false;
            // Move Camera and set Depth of Field
        }
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

}