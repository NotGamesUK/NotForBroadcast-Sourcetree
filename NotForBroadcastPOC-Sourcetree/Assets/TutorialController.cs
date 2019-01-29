using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialController : MonoBehaviour
{
    private EventController myEventController;
    private GodOfTheRoom myRoomGod;
    private MasterController myMasterController;

    public int myLevel, currentStep, preDormantStep;

    private bool tutorialIsActive;
    private float waitTime; // Used as a counter

    // For Tutorial Level 01
    public CameraMovement myCamera;
    public Telephone myPhone;
    public GameObject lookRightPrompt01;
    public GameObject lookRightPrompt02;
    public GameObject answerPhonePrompt01;
    public GameObject answerPhonePrompt02;
    public AudioClip sfx01_01HelloMate;
    public AudioClip sfx01_02ExplainingTheFrontView;
    public TutorialFlasher VisionMixerButtonOne;
    // Start is called before the first frame update
    void Start()
    {
        myEventController = GetComponent<EventController>();
        myRoomGod = GetComponent<GodOfTheRoom>();
        myMasterController = FindObjectOfType<MasterController>();
        myLevel = -1;
        currentStep = -1;
        tutorialIsActive = false;
    }

    public void StartTutorial(int thisLevel)
    {
        myLevel = thisLevel;
        tutorialIsActive = true;
        currentStep = 10; // Increases in 10s to allow for extra steps if required after testing
        myCamera.leftViewLocked = false;
        myCamera.downViewLocked = false;
        myCamera.rightViewLocked = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (tutorialIsActive)
        {
            switch (myLevel)
            {

                case 1:

                    switch(currentStep)
                    {
                        case -1: // Tutorial is dormant waiting to be changed from outside;
                            //
                            // To go dormant use:
                            // preDormantStep = currentStep;
                            // currentStep = -1; // Go Dormant and wait for change from elsewhere
                            //

                            break;

                        case 10: // Lock all views except front and right
                            myCamera.downViewLocked = true;
                            myCamera.leftViewLocked = true;
                            myPhone.IncomingCall(sfx01_01HelloMate, false, 0);
                            currentStep = 20;
                            waitTime = 9;
                            break;

                        case 20: // Wait for player to look right unprompted
                            if (myCamera.myPosition == "Right")
                            { // No need to display prompt
                                waitTime = 4;
                                currentStep = 50;
                            } 
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                lookRightPrompt01.SetActive(true); // Prompt Player to Look Right
                                currentStep = 30;
                                waitTime = 4;
                            }
                            break;

                        case 30: // Wait for player to look right after prompt 01
                            if (myCamera.myPosition == "Right") { currentStep = 40; } // No need to display prompt
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                lookRightPrompt02.SetActive(true); // Prompt Player to Look Right
                                currentStep = 40;

                            }
                            break;

                        case 40: // Wait for player to look right
                            if (myCamera.myPosition == "Right")
                            {
                                waitTime = 4;
                                currentStep = 50;
                            } 

                            break;

                        case 50: // Player has finally looked right - Wait for player to answer phone unprompted
                            if (myPhone.isConnected)
                            {
                                // Skip to next stage
                                currentStep = 70;

                            } else
                            {
                                waitTime -= Time.deltaTime;
                                if (waitTime < 0)
                                {
                                    answerPhonePrompt01.SetActive(true); // Prompt Player to Look Right
                                    currentStep = 60;
                                    waitTime = 4;
                                }

                            }


                            break;

                        case 60: // Wait for player to answer phone after prompt 01
                            if (myPhone.isConnected)
                            {
                                currentStep = 70;
                            }
                            else
                            { // No need to display prompt
                                waitTime -= Time.deltaTime;
                                if (waitTime < 0)
                                {
                                    answerPhonePrompt02.SetActive(true); // Prompt Player to Look Right
                                    currentStep = 70;
                                }
                            }
                            break;

                        case 70: // Wait for player to answer phone after all prompts

                            if (myPhone.isConnected)
                            {
                                lookRightPrompt01.SetActive(false); // Hide Prompt 01
                                lookRightPrompt02.SetActive(false); // Hide Prompt 02
                                answerPhonePrompt01.SetActive(false); // Hide Prompt 01
                                answerPhonePrompt02.SetActive(false); // Hide Prompt 02
                                myRoomGod.SpawnHuman("MaleCorridorWalker", 10);
                                currentStep = 80;
                            }

                            break;

                        case 80: // Wait for first message to end and player to be facing front then play second message

                            if (myPhone.isOnHold)
                            {
                                if (myCamera.myPosition == "Centre")
                                {
                                    myPhone.UnholdAndPlay(sfx01_02ExplainingTheFrontView, false);
                                    myCamera.rightViewLocked = true;
                                    currentStep = 90;
                                    waitTime = 5;
                                }
                            }

                            break;

                        case 90: // Wait for waitTime and flash first object
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                myRoomGod.FlashObject(VisionMixerButtonOne, 20, 0.1f);
                                currentStep = 100;
                            }
                            break;

                        case 100:


                            break;
                    }



                    break;

                default: // IF THERE IS NO TUTORIAL ON THIS LEVEL

                    tutorialIsActive = false;
                    myLevel = -1;
                    currentStep = -1;
                    break;

            }
        }
    }
}
