using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering.PostProcessing;


public class CameraMovement : MonoBehaviour {

    public bool freeLook = false;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public PostProcessVolume myNewPostProcessing;
    public PostProcessingProfile myPostProcessing;
    public float focusPullTime, frontViewFocusLength, frontViewAperture, leftViewFocusLength, leftViewAperture, rightViewFocusLength, rightViewAperture, downViewFocusLength, downViewAperture, inTrayViewFocusLength, inTrayViewAperture;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Animator myAnimator;
    [HideInInspector]
    public string myPosition="Centre";
    private Vector3 startPosition;
    private float startFOV;
    private Camera myCamera;
    private float currentFocusDistance, currentAperture, targetFocusDistance, targetAperture, focusIncrement, apertureIncrement, focusPullCountdown;
    private bool pullingFocus;

    private DepthOfField myNewDOF;
    [HideInInspector]
    public bool leftViewLocked, downViewLocked, rightViewLocked;



    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        startPosition = this.transform.position;
        myCamera = GetComponent<Camera>();
        startFOV = myCamera.fieldOfView;
        var myDOF = myPostProcessing.depthOfField.settings;
        myNewPostProcessing.profile.TryGetSettings(out myNewDOF);
        currentFocusDistance = frontViewFocusLength;
        currentAperture = frontViewAperture;
        myDOF.focusDistance = frontViewFocusLength;
        myDOF.aperture = frontViewAperture;
        myPostProcessing.depthOfField.settings = myDOF;


        targetFocusDistance = currentFocusDistance;
        targetAperture = currentAperture;
        pullingFocus = false;
        leftViewLocked = false;
        downViewLocked = false;
        rightViewLocked = false;
    }

    public void LookDown()
    {
        //Debug.Log("TOLD TO LOOK DOWN.  My Current Position: "+myPosition);
        if (myPosition != "Down" && !downViewLocked)
        {
            myAnimator.SetTrigger("downPressed");
            myPosition = "Down";
            PullFocusOverTime(downViewFocusLength, downViewAperture, focusPullTime);
        }

    }

    public void LookUp()
    {
        if (myPosition == "Down")
        {
            myAnimator.SetTrigger("upPressed");
            myPosition = "Centre";
            PullFocusOverTime(frontViewFocusLength, frontViewAperture, focusPullTime);

        }

    }

    public void LookLeft()
    {
        if (myPosition != "Left")
        {
            if (myPosition == "Right")
            {
                myAnimator.SetTrigger("leftPressed");
                myPosition = "Centre";
                PullFocusOverTime(frontViewFocusLength, frontViewAperture, focusPullTime);

            }
            else
            {
                if (!leftViewLocked)
                {
                    myPosition = "Left";
                    myAnimator.SetTrigger("leftPressed");
                    PullFocusOverTime(leftViewFocusLength, leftViewAperture, focusPullTime);
                }
            }
        }

    }

    public void LookRight()
    {
        if (myPosition != "Right")
        {
            if (myPosition == "Left")
            {
                myAnimator.SetTrigger("rightPressed");
                myPosition = "Centre";
                PullFocusOverTime(frontViewFocusLength, frontViewAperture, focusPullTime);

            }
            else
            {
                if (!rightViewLocked)
                {
                    myAnimator.SetTrigger("rightPressed");
                    myPosition = "Right";
                    PullFocusOverTime(rightViewFocusLength, rightViewAperture, focusPullTime);
                }
            }
        }

    }

    public void LookToInTray()
    {
        if (myPosition == "Right")
        {
            myAnimator.SetTrigger("goToInTray");
            myPosition = "InTray";
            PullFocusOverTime(inTrayViewFocusLength, inTrayViewAperture, focusPullTime);

        }

    }

    public void BackFromInTray()
    {
        if (myPosition == "InTray")
        {
            myAnimator.SetTrigger("backFromInTray");
            myPosition = "Right";
            PullFocusOverTime(rightViewFocusLength, rightViewAperture, focusPullTime);

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
        }

        if (pullingFocus)
        {
            var myDOF = myPostProcessing.depthOfField.settings;
            currentFocusDistance = myDOF.focusDistance;
            currentAperture = myDOF.aperture;
            currentFocusDistance += (focusIncrement * Time.deltaTime);
            currentAperture += (apertureIncrement * Time.deltaTime);
            focusPullCountdown -= Time.deltaTime;
            if (focusPullCountdown <= 0)
            {
                currentFocusDistance = targetFocusDistance;
                currentAperture = targetAperture;
                focusPullCountdown = 0;
                pullingFocus = false;
            }
            myDOF.focusDistance = currentFocusDistance;
            myDOF.aperture = currentAperture;
            myPostProcessing.depthOfField.settings = myDOF;

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
        focusIncrement = 0;
        apertureIncrement = 0;
        focusPullCountdown = 0;
        pullingFocus = false;
        if (thisFocusDistance != currentFocusDistance)
        {
            targetFocusDistance = thisFocusDistance;
            focusIncrement = (targetFocusDistance - currentFocusDistance) / thisTime;
            focusPullCountdown = thisTime;
            pullingFocus = true;
            //Debug.Log("Main Camera: Adjusting Focus Distance.");
        }
        if (thisAperture != currentAperture)
        {
            targetAperture = thisAperture;
            apertureIncrement = (targetAperture - currentAperture) / thisTime;
            focusPullCountdown = thisTime;
            pullingFocus = true;
            //Debug.Log("Main Camera: Adjusting Aperture.");

        }
    }

    public void ResetFocus()
    {
        switch (myPosition)
        {
            case "Centre":
                PullFocusOverTime(frontViewFocusLength, frontViewAperture, focusPullTime);
                break;

            case "Left":
                PullFocusOverTime(leftViewFocusLength, leftViewAperture, focusPullTime);
                break;

            case "Right":
                PullFocusOverTime(rightViewFocusLength, rightViewAperture, focusPullTime);
                break;

            case "Down":
                PullFocusOverTime(downViewFocusLength, downViewAperture, focusPullTime);
                break;

            default:
                Debug.LogError("TRYING TO RESET CAMERA FOCUS WITH NO POSITION SELECTED.");
                break;

        }
    }

}