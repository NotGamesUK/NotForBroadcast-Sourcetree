using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public ButtonAnimating[] myVisionMixerButtons;
    public ButtonAnimating[] myMuteButtons;
    public ButtonAnimating myBleepButton;
    public ButtonAnimating myPlayAdButton;
    public Slider myFrequencyControlSlider;
    public Slider mySourceSelectSlider;
    public Switch myVisonMixerLinkSwitch;

    public enum InputMode { Menu, InGame, PlayBack, RushesRoom, Inactive }
    [HideInInspector]
    public InputMode myMode;

    private CameraMovement myCameraController;
    private GUIController myGUIController;
    private bool isPaused;

	// Use this for initialization
	void Start () {
        myCameraController = FindObjectOfType<CameraMovement>();
        myGUIController = FindObjectOfType<GUIController>();
        myMode = InputMode.Inactive;
        isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch (myMode)
        {
            case InputMode.Inactive:


                break;

            case InputMode.Menu:


                break;

            case InputMode.InGame:

                // Keyboard Controls

                if (Input.GetButtonDown("Down"))
                {
                    myCameraController.LookDown();
                }

                if (Input.GetButtonDown("Up"))
                {
                    myCameraController.LookUp();
                }

                if (Input.GetButtonDown("Left"))
                {
                    myCameraController.LookLeft();
                }

                if (Input.GetButtonDown("Right"))
                {
                    myCameraController.LookRight();
                }

                if (Input.GetButtonDown("VM01"))
                {
                    myVisionMixerButtons[0].KeyDown();
                }

                if (Input.GetButtonDown("VM02"))
                {
                    myVisionMixerButtons[1].KeyDown();
                }

                if (Input.GetButtonDown("VM03"))
                {
                    myVisionMixerButtons[2].KeyDown();
                }

                if (Input.GetButtonDown("VM04"))
                {
                    myVisionMixerButtons[3].KeyDown();
                }

                if (Input.GetButtonDown("Mute01"))
                {
                    myMuteButtons[0].KeyDown();
                }

                if (Input.GetButtonUp("Mute01"))
                {
                    myMuteButtons[0].KeyUp();
                }

                if (Input.GetButtonDown("Mute02"))
                {
                    myMuteButtons[1].KeyDown();
                }

                if (Input.GetButtonUp("Mute02"))
                {
                    myMuteButtons[1].KeyUp();
                }

                if (Input.GetButtonDown("Mute03"))
                {
                    myMuteButtons[2].KeyDown();
                }

                if (Input.GetButtonUp("Mute03"))
                {
                    myMuteButtons[2].KeyUp();
                }

                if (Input.GetButtonDown("Mute04"))
                {
                    myMuteButtons[3].KeyDown();
                }

                if (Input.GetButtonUp("Mute04"))
                {
                    myMuteButtons[3].KeyUp();
                }

                if (Input.GetButtonDown("Bleep"))
                {
                    myBleepButton.KeyDown();
                }

                if (Input.GetButtonUp("Bleep"))
                {
                    myBleepButton.KeyUp();
                }

                if (Input.GetButtonDown("PlayAd"))
                {
                    myPlayAdButton.KeyDown();
                }

                if (Input.GetButtonDown("SourceLeft"))
                {
                    mySourceSelectSlider.value -= 1;
                }

                if (Input.GetButtonDown("SourceRight"))
                {
                    mySourceSelectSlider.value += 1;
                }

                if (Input.GetButtonDown("VMLink"))
                {
                    myVisonMixerLinkSwitch.KeyDown();
                }

                if (Input.GetButtonDown("Pause"))
                {
                    if (isPaused)
                    {
                        myGUIController.Unpause();
                        isPaused = false;
                    }
                    else
                    {
                        if (myGUIController.currentMenu == null)
                        {
                            myGUIController.GoToPauseMenu();
                            isPaused = true;
                        }
                    }
                }




                var thisWheelInput = Input.GetAxis("DotControl");
                if (thisWheelInput > 0)
                {
                    myFrequencyControlSlider.value += 0.1f;
                }
                else if (thisWheelInput < 0)
                {
                    myFrequencyControlSlider.value -= 0.1f;
                }

                break;

            case InputMode.PlayBack:


                break;

            case InputMode.RushesRoom:


                break;


        }
    }
}
