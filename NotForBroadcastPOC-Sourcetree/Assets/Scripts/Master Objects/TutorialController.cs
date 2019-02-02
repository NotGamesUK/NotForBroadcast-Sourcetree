using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialController : MonoBehaviour
{
    private EventController myEventController;
    private GodOfTheRoom myRoomGod;
    private MasterController myMasterController;
    private AudienceVUMeter myVUMeter;
    private MasterTripSwitch myTripSwitch;
    private VHSPlayer myVHSPlayer01, myVHSPlayer02, myVHSPlayer03;
    private VHSControlPanel myVHSControlPanel;
    private BackWallClock myClock;
    private ScoringController myScoringController;
    private VisionMixer myVisionMixer;
    private SatelliteDish myTower;

    public int myLevel, currentStep, preDormantStep;

    private bool tutorialIsActive, firstPass;
    private float waitTime, timeToFeed, distanceFromTitleJump; // Used as counters

    // For Tutorial Level 01
    public CameraMovement myCamera;
    public Telephone myPhone;
    public VUBar myUpArrow, myDownArrow;

    [Space(5)]
    [Header("Floating Prompts")]

    public GameObject prompt01lookRight01;
    public GameObject prompt01lookRight02;
    public GameObject prompt02answerPhone01;
    public GameObject prompt02answerPhone02;
    public GameObject prompt03LookLeft01;
    public GameObject prompt03LookLeft02;
    public GameObject prompt04LookLeft01;
    public GameObject prompt04LookLeft02;
    public GameObject prompt05ThrowTripSwitch01;
    public GameObject prompt05ThrowTripSwitch02;
    public GameObject prompt06lookRight01;
    public GameObject prompt06lookRight02;
    public GameObject prompt07lookDown01;
    public GameObject prompt07lookDown02;
    public GameObject prompt08loadTapes;
    public GameObject prompt08ClickAndDrag;
    public GameObject prompt09aLeftClick;
    public GameObject prompt09bLookUp;
    public GameObject prompt09cLookUp;
    public GameObject prompt10Use1234;
    public GameObject prompt11Use1234;
    public GameObject prompt12Mousewheel;




    [Space(5)]
    [Header("Telephone Audio")]

    public AudioClip sfx01_01HelloMate;
    public AudioClip sfx01_02ExplainingTheFrontView;
    public AudioClip sfx01_03TurnOnTripSwitch;
    public AudioClip sfx01_04EndTitlesEnd;
    public AudioClip sfx01_05LoadTapes;
    public AudioClip sfx01_06SelectAd;
    public AudioClip sfx01_07StudioSignalComing;
    public AudioClip sfx01_08aSelect01NoRush;
    public AudioClip sfx01_08bSelect01;
    public AudioClip sfx01_08cSelect01Rush;
    public AudioClip sfx01_09NewsStarted;
    public AudioClip sfx01_10CountToTitles;
    public AudioClip sfx01_11aLovelyMate;
    public AudioClip sfx01_11bBitLateMate;
    public AudioClip sfx01_11cBitEarlyMate;
    public AudioClip sfx01_12NextThrowIsJeremy;
    public AudioClip sfx01_13Select01;
    public AudioClip sfx01_14InterferenceExplained;
    public AudioClip sfx01_15PlayerHitInterference;
    public AudioClip sfx01_16PlayerHitInterferenceAgain;
    public AudioClip sfx01_17ClockExplained;
    public AudioClip sfx01_18CountIntoAd;
    public AudioClip sfx01_19AdLate;
    public AudioClip sfx01_20AdVeryLate;

    [Space(5)]
    [Header("Flashing Objects")]

    public TutorialFlasher flashAudienceDown;
    public TutorialFlasher flashAudienceUp;
    public TutorialFlasher flashFreqControlBox;
    public TutorialFlasher flashTripSwitch;
    public TutorialFlasher flashBroadcastTV;
    public TutorialFlasher flashVMButton01;
    public TutorialFlasher flashVMButton02;
    public TutorialFlasher flashVMButton03;
    public TutorialFlasher flashVMButton04;
    public TutorialFlasher flashVUBar;
    public TutorialFlasher flashPlayAdBox;
    public TutorialFlasher flashMasterTV;
    public TutorialFlasher flashFreqControlKnob;
    public TutorialFlasher flashPlug01;
    public TutorialFlasher flashPlug02;
    public TutorialFlasher flashPlug03;
    public TutorialFlasher flashPlug04;
    public TutorialFlasher flashPlug05;
    public TutorialFlasher flashPlug06;
    public TutorialFlasher flashPlug07;
    public TutorialFlasher flashTV01;
    public TutorialFlasher flashTV02;
    public TutorialFlasher flashTV03;
    public TutorialFlasher flashTV04;
    public TutorialFlasher flashVHSTapes;
    public TutorialFlasher flashVHSButton01;
    public TutorialFlasher flashVHSButton02;
    public TutorialFlasher flashVHSButton03;
    public TutorialFlasher flashVHSPlayer01;
    public TutorialFlasher flashVHSPlayer02;
    public TutorialFlasher flashVHSPlayer03;
    public TutorialFlasher flashClock;





    // Start is called before the first frame update
    void Start()
    {
        myEventController = GetComponent<EventController>();
        myRoomGod = GetComponent<GodOfTheRoom>();
        myMasterController = FindObjectOfType<MasterController>();
        myVUMeter = FindObjectOfType<AudienceVUMeter>();
        myTripSwitch = FindObjectOfType<MasterTripSwitch>();
        myVHSPlayer01 = flashVHSPlayer01.GetComponentInParent<VHSPlayer>();
        myVHSPlayer02 = flashVHSPlayer02.GetComponentInParent<VHSPlayer>();
        myVHSPlayer03 = flashVHSPlayer03.GetComponentInParent<VHSPlayer>();
        myVHSControlPanel = FindObjectOfType<VHSControlPanel>();
        myClock = FindObjectOfType<BackWallClock>();
        myScoringController = FindObjectOfType<ScoringController>();
        myVisionMixer = FindObjectOfType<VisionMixer>();
        myTower = FindObjectOfType<SatelliteDish>();
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

                    switch (currentStep)
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
                                prompt01lookRight01.SetActive(true); // Prompt Player to Look Right
                                currentStep = 30;
                                waitTime = 4;
                            }
                            break;

                        case 30: // Wait for player to look right after prompt 01
                            if (myCamera.myPosition == "Right") { currentStep = 40; } // No need to display prompt
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                prompt01lookRight02.SetActive(true); // Prompt Player to Look Right
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

                            }
                            else
                            {
                                waitTime -= Time.deltaTime;
                                if (waitTime < 0)
                                {
                                    prompt02answerPhone01.SetActive(true); // Prompt Player to Look Right
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
                                    prompt02answerPhone02.SetActive(true); // Prompt Player to Look Right
                                    currentStep = 70;
                                }
                            }
                            break;

                        case 70: // Wait for player to answer phone after all prompts

                            if (myPhone.isConnected)
                            {
                                prompt01lookRight01.SetActive(false); // Hide Prompt 01
                                prompt01lookRight02.SetActive(false); // Hide Prompt 02
                                prompt02answerPhone01.SetActive(false); // Hide Prompt 01
                                prompt02answerPhone02.SetActive(false); // Hide Prompt 02
                                myRoomGod.SpawnHuman("MaleCorridorWalker", 10);
                                currentStep = 80;
                                firstPass = true;
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
                                    waitTime = 1.7f; // Time till Flash VU Bar
                                    firstPass = true;
                                }
                                else if (firstPass)
                                {
                                    firstPass = false;
                                    prompt03LookLeft01.SetActive(true);
                                    prompt03LookLeft02.SetActive(true);
                                }

                            }

                            break;

                        case 90: // Wait for waitTime and flash VU Bar
                            if (firstPass)
                            {
                                prompt03LookLeft01.SetActive(false);
                                prompt03LookLeft02.SetActive(false);
                                firstPass = false;
                            }
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                myRoomGod.FlashObject(flashVUBar, 7f, 0.5f);
                                currentStep = 100;
                                waitTime = 6.5f - 1.7f; // Time until Flash Audience Up
                            }
                            break;

                        case 100:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                myVUMeter.SetToPercentage(52f);
                                myUpArrow.LightOn();
                                currentStep = 110;
                                waitTime = 8.3f - 6.5f; // Time from here until Flash Audience Down
                            }

                            break;

                        case 110:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                myVUMeter.SetToPercentage(50f);
                                myUpArrow.LightOff();
                                myDownArrow.LightOn();
                                currentStep = 115;
                                waitTime = 2.3f; // Time from here until Down Arrow Goes Off
                            }

                            break;

                        case 115:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                myDownArrow.LightOff(); // Turn off Down arrow
                                waitTime = 12.2f - 10.6f; // Time until Broadcast Screen Flashes
                                currentStep = 120;
                            }
                            break;

                        case 120:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                // FLASH Broadcast Screen for 3.6 secs
                                myRoomGod.FlashObject(flashBroadcastTV, 5.5f, 0.5f);
                                currentStep = 130;
                                waitTime = 17.9f - 12.2f; // Time from here until Flash Master Screen
                            }

                            break;

                        case 130:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                // FLASH Master Screen for 3.8 secs
                                myRoomGod.FlashObject(flashMasterTV, 5f, 0.5f);
                                currentStep = 140;
                                waitTime = 22.9f - 17.9f; // Time from here until Flash Small Screens
                            }

                            break;

                        case 140:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                // FLASH Small Screens for 4 secs
                                myRoomGod.FlashObject(flashTV01, 6f, 0.5f);
                                myRoomGod.FlashObject(flashTV02, 6f, 0.5f);
                                myRoomGod.FlashObject(flashTV03, 6f, 0.5f);
                                myRoomGod.FlashObject(flashTV04, 6f, 0.5f);
                                currentStep = 150;
                                waitTime = 29.4f - 22.9f; // Time from here until Flash VM Buttons
                            }

                            break;

                        case 150:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                // FLASH VM Buttons for 3.7 secs
                                myRoomGod.FlashObject(flashVMButton01, 6f, 0.2f);
                                myRoomGod.FlashObject(flashVMButton02, 6f, 0.2f);
                                myRoomGod.FlashObject(flashVMButton03, 6f, 0.2f);
                                myRoomGod.FlashObject(flashVMButton04, 6f, 0.2f);
                                currentStep = 160;
                                firstPass = true;
                                waitTime = 2; // Time until player prompted to turn left
                                myCamera.leftViewLocked = false;
                                waitTime = 12;
                            }

                            break;

                        case 160:
                            // if (VIEW IS LEFT) move on to 170 else...
                            if (myCamera.myPosition == "Left")  // No need to display prompt
                            {
                                firstPass = true;
                                currentStep = 170;
                                break;
                            }

                            waitTime -= Time.deltaTime;
                            if (waitTime < 0 && firstPass)
                            {
                                firstPass = false;
                                // DISPLAY TURN LEFT PROMPTS
                                prompt04LookLeft01.SetActive(true);
                                prompt04LookLeft02.SetActive(true);

                            }
                            break;

                        case 170:
                            if (firstPass)
                            {
                                firstPass = false;
                                // HIDE TURN LEFT PROMPTS
                                prompt04LookLeft01.SetActive(false);
                                prompt04LookLeft02.SetActive(false);
                            }
                            if (myPhone.isOnHold)
                            {
                                myPhone.UnholdAndPlay(sfx01_03TurnOnTripSwitch, false);
                                currentStep = 180;
                                waitTime = 0.7f;
                            }
                            break;

                        case 180:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                // FLASH Plugs for 6.5 secs
                                myRoomGod.FlashObject(flashPlug01, 6.5f, 0.5f);
                                myRoomGod.FlashObject(flashPlug02, 6.5f, 0.5f);
                                myRoomGod.FlashObject(flashPlug03, 6.5f, 0.5f);
                                myRoomGod.FlashObject(flashPlug04, 6.5f, 0.5f);
                                myRoomGod.FlashObject(flashPlug05, 6.5f, 0.5f);
                                myRoomGod.FlashObject(flashPlug06, 6.5f, 0.5f);
                                currentStep = 190;
                                waitTime = 7.8f - 0.7f; // Time from here until Flash Trip Switch
                            }

                            break;

                        case 190:
                            waitTime -= Time.deltaTime;
                            if (waitTime < 0)
                            {
                                // FLASH Trip Switch for 10 secs
                                myRoomGod.FlashObject(flashTripSwitch, -1f, 0.25f); // A TIME OF -1 WILL FLASH UNTIL TOLD TO STOP
                                currentStep = 200;
                                waitTime = 11.5f - 7.8f; // Time from here until Prompt on Trip Switch
                                firstPass = true;
                            }

                            break;

                        case 200:

                            waitTime -= Time.deltaTime;
                            if (waitTime < 0 && firstPass)
                            {
                                firstPass = false;
                                // DISPLAY TRIP SWITCH PROMPTS
                                prompt05ThrowTripSwitch01.SetActive(true);
                                prompt05ThrowTripSwitch02.SetActive(true);

                            }


                            // IF TRIP SWITCH IS ON: 
                            if (myTripSwitch.isOn)
                            {
                                // STOP TRIP SWITCH FLASHING
                                myRoomGod.StopFlashingObject(flashTripSwitch);
                                // HIDE TRIP SWITCH PROMPTS
                                prompt05ThrowTripSwitch01.SetActive(false);
                                prompt05ThrowTripSwitch02.SetActive(false);
                                // SET WAIT TIME TO 3
                                waitTime = 3;
                                // JUMP TO 210
                                currentStep = 210;

                            }

                            break;

                        case 210:

                            // decrease timer - WHEN LESS THAN 0 
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                // IF LOOKING LEFT:
                                if (myCamera.myPosition == "Left")
                                {
                                    // SHOW LOOK RIGHT PROMPTS
                                    prompt06lookRight01.SetActive(true);
                                    prompt06lookRight02.SetActive(true);
                                }
                                else
                                {
                                    // else LOCK LEFT
                                    myCamera.leftViewLocked = true;
                                }

                            }

                            // IF ON HOLD AND LOOKING FRONT:
                            if (myPhone.isOnHold && myCamera.myPosition == "Centre")
                            {

                                // HIDE LOOK RIGHT PROMPTS 
                                prompt06lookRight01.SetActive(false);
                                prompt06lookRight02.SetActive(false);
                                // LOCK LEFT
                                myCamera.leftViewLocked = true;
                                // PLAY NEXT AUDIO
                                myPhone.UnholdAndPlay(sfx01_04EndTitlesEnd, false);
                                // SET WAIT-TIME TO 7.0
                                waitTime = 7;
                                // JUMP TO 220
                                currentStep = 220;
                            }


                            break;

                        case 220:

                            // Decrease Timer then 
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                // start PRE-ROLL
                                myMasterController.tutorialSaysStart = true;
                                // Set timer for 12-7 secs
                                waitTime = 12 - 7;
                                // Jump to 230
                                currentStep = 230;
                            }
                            break;

                        case 230:

                            // Decrease Timer then:
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                // Unlock Down
                                myCamera.downViewLocked = false;
                                // Set Timer for 4 secs
                                waitTime = 4;
                                // JUMP to 240;
                                currentStep = 240;
                                firstPass = true;
                            }
                            break;

                        case 240:

                            // IF LOOKING DOWN AND ON HOLD 
                            if (myCamera.myPosition == "Down" && myPhone.isOnHold)
                            {

                                // SET TIMER TO 1 second THEN JUMP TO 250
                                waitTime = 1;
                                currentStep = 250;
                                firstPass = true;
                                break;

                            }
                            else
                            {

                                // ELSE decrease timer
                                waitTime -= Time.deltaTime;
                                if (waitTime <= 0 && firstPass)
                                {
                                    // when done - Display LOOK DOWN PROMPTS.
                                    firstPass = false;
                                    prompt07lookDown01.SetActive(true);
                                    prompt07lookDown02.SetActive(true);
                                }
                            }


                            break;

                        case 250:

                            // First Pass 
                            if (firstPass)
                            {
                                // Hide prompts.  Start new audio.
                                prompt07lookDown01.SetActive(false);
                                prompt07lookDown02.SetActive(false);
                                myPhone.UnholdAndPlay(sfx01_05LoadTapes, false);
                                firstPass = false;
                            }
                            // When Timer hits 0...
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                // Flash Tapes for 5 seconds.  Set timer for 6.5 - 1.0.  Jump to 260.
                                myRoomGod.FlashObject(flashVHSTapes, 5f, 0.3f);
                                waitTime = 6.5f - 1.5f;
                                currentStep = 260;
                                firstPass = true;
                            }


                            break;

                        case 260:

                            // When Timer hits 0 - 
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                // if FirstPass: 
                                if (firstPass)
                                {
                                    firstPass = false;
                                    // Flash VHS Players for 5 seconds.  
                                    myRoomGod.FlashObject(flashVHSPlayer01, 5f, 0.3f);
                                    myRoomGod.FlashObject(flashVHSPlayer02, 5f, 0.3f);
                                    myRoomGod.FlashObject(flashVHSPlayer03, 5f, 0.3f);
                                    // If no tapes are loaded...
                                    if (!myVHSPlayer01.isLoaded && !myVHSPlayer02.isLoaded && !myVHSPlayer03.isLoaded)
                                    {
                                        // Display Click and Drag.
                                        prompt08loadTapes.SetActive(true);
                                        prompt08ClickAndDrag.SetActive(true);

                                    }
                                }
                            }
                            // When All three machines are loaded Set timer to 0.2 jump to 270
                            if (myVHSPlayer01.isLoaded && myVHSPlayer02.isLoaded && myVHSPlayer03.isLoaded && myPhone.isOnHold)
                            {
                                waitTime = 0.2f;
                                currentStep = 270;
                                firstPass = true;
                            }
                            break;

                        case 270:

                            // First Pass - 
                            if (firstPass)
                            {
                                // Hide click and drag prompts.  Start new audio.
                                prompt08loadTapes.SetActive(false);
                                prompt08ClickAndDrag.SetActive(false);
                                myPhone.UnholdAndPlay(sfx01_06SelectAd, false);
                                firstPass = false;
                            }
                            // When Timer hits 0
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                // Flash VHS Selection buttons for 7 seconds.  
                                myRoomGod.FlashObject(flashVHSButton01, -1f, 0.3f);
                                myRoomGod.FlashObject(flashVHSButton02, -1f, 0.3f);
                                myRoomGod.FlashObject(flashVHSButton03, -1f, 0.3f);
                                // Set timer for 5.7=0.2.  
                                waitTime = 5.7f - 0.2f;
                                // Jump to 280.
                                currentStep = 280;
                                firstPass = true;
                            }

                            break;

                        case 280:

                            // When timer hits zero (first pass) 
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0 && firstPass)
                            {
                                // Display Left Mouse Prompt by Selection buttons.
                                prompt09aLeftClick.SetActive(true);
                                firstPass = false;
                            }
                            // If a VHS Control Panel button has been selected 
                            if (myVHSControlPanel.selectedPlayer != 0)
                            {
                                // Hide MOUSE CLICK Prompt, stop flashing buttons, and jump to 290.
                                prompt09aLeftClick.SetActive(false);
                                myRoomGod.StopFlashingObject(flashVHSButton01);
                                myRoomGod.StopFlashingObject(flashVHSButton02);
                                myRoomGod.StopFlashingObject(flashVHSButton03);
                                firstPass = true;
                                waitTime = 2;
                                currentStep = 290;
                            }

                            break;

                        case 290:

                            if (myPhone.isOnHold)
                            {
                                // If ON Hold and Facing Down 
                                if (myCamera.myPosition == "Down")
                                {
                                    waitTime -= Time.deltaTime;
                                    if (waitTime <= 0 && firstPass)
                                    {
                                        //- FirstPass - Display LOOK UP Prompt
                                        prompt09bLookUp.SetActive(true);
                                        prompt09cLookUp.SetActive(true);
                                        firstPass = false;
                                    }
                                }
                                else if (myCamera.myPosition == "Centre") // If ON HOLD and Facing Front:

                                {
                                    prompt09bLookUp.SetActive(false);
                                    prompt09cLookUp.SetActive(false);
                                    // IF NEWS HAS STARTED: 
                                    if (myScoringController.broadcastScreensLive)
                                    {
                                        // FOR 10 DEAD SECONDS - 
                                        if (myEventController.myClock > 23) // Calculated as 13 (pre-roll) + 10 seconds into sequence
                                        {
                                            // Throw Level Fail - Reason: Didn't broadcast the news!!
                                            myMasterController.FailMidLevel("Failed to start the Broadcast.<br>You've let the whole country down, Dave.<br>If you ever get another job in TV, try concentrating.");
                                            tutorialIsActive = false;
                                            break;

                                        }
                                        else
                                        {
                                            myPhone.UnholdAndPlay(sfx01_08cSelect01Rush, false);
                                            currentStep = 295;
                                            firstPass = false; // Tell step 295 to skip the audio
                                            break;
                                        }
                                    }
                                    // IF NEWS HASN'T STARTED WE WILL REACH HERE...
                                    // Work out how much time is left and...
                                    timeToFeed = myClock.clockTime - 13f; // 13 is Pre-roll time on sequence 01;


                                    // If MORE THAN 8 SECONDS UNTIL CONTROL ROOM
                                    if (timeToFeed > 8)
                                    {
                                        // Play Audio 07. 
                                        myPhone.UnholdAndPlay(sfx01_07StudioSignalComing, false);
                                        // Jump to 295
                                        currentStep = 295;
                                        firstPass = true; // Tell step 295 to play the audio
                                        break;
                                    }
                                    // IF LESS THAN 8 SECONDS OUT - Jump to 295;
                                    if (timeToFeed > 0)
                                    {
                                        // Jump to 295;
                                        currentStep = 295;
                                        firstPass = true; // Tell step 295 to play the audio
                                        break;
                                    }
                                    // IF STUDIO ALREADY RECIEVED
                                    if (timeToFeed <= 0)
                                    {
                                        // play 08b. 
                                        myPhone.UnholdAndPlay(sfx01_08bSelect01, false);
                                        currentStep = 295;
                                        firstPass = false; // Tell step 295 to skip the audio
                                        break;
                                    }
                                }
                            }

                            break;

                        case 295:
                            // Wait until 2 secs (or less) before studio and 
                            timeToFeed = myClock.clockTime - 13f;
                            if (timeToFeed < 4)
                            {
                                if (firstPass)
                                {
                                    // play 08a - if there's time

                                    myPhone.UnholdAndPlay(sfx01_08aSelect01NoRush, false);
                                    firstPass = false;
                                }
                                // Flash VM01 for 10 secs. - Display USE 1234 to Select Screens and Mouse click Prompts. 
                                myRoomGod.FlashObject(flashVMButton01, -1, 0.25f);
                                prompt10Use1234.SetActive(true);
                                currentStep = 300;
                                firstPass = true;
                            }
                            break;

                        case 300:
                            if (myVisionMixer.currentScreen == 1)
                            {
                                if (firstPass)
                                {
                                    // Stop flashing VM01
                                    myRoomGod.StopFlashingObject(flashVMButton01);
                                    prompt10Use1234.SetActive(false);
                                    firstPass = false;
                                }
                                if (myPhone.isOnHold && myEventController.myClock > 15) // if NEWS STARTED and SCREEN 01 SELECTED

                                {
                                    // Play SFX09.
                                    myPhone.UnholdAndPlay(sfx01_09NewsStarted, false);
                                    firstPass = true;
                                    currentStep = 305;
                                }
                            }
                            break;

                        case 305:
                            // WAIT FOR CLOCK TO REACH JUMP TO NEWS TITLES MINUS length of SFX10 count (5.2secs) (for exact countdown) 
                            if (myEventController.myClock >= (42.5f - 5.2f)) // First Number is Time to switch to news titles.  Second number is time into SFX10 of change. 
                            {
                                // THEN flash VM02 for 5 seconds - Play SFX10.  - Display USE 2 and Mouse click Prompts.
                                myRoomGod.FlashObject(flashVMButton02, -1, 0.25f);
                                prompt11Use1234.SetActive(true);
                                myPhone.UnholdAndPlay(sfx01_10CountToTitles, false);
                                currentStep = 310;
                                firstPass = true;
                                break;
                            }


                            break;

                        case 310:
                            // IF MORE THAN 10 SECONDS LATE INTO NEWS TITLES - Throw Level Fail - Reason: Didn't broadcast the news Titles!!
                            distanceFromTitleJump = myEventController.myClock - 42.5f;
                            if (distanceFromTitleJump >= 5) // 5 secs after player should have switched to news
                            {
                                myMasterController.FailMidLevel("Failed to switch to the News Titles.<br>You're not a f*cking intern, Dave.<br>We expected better.");
                                tutorialIsActive = false;
                                break;
                            }
                            // if SCREEN 02 SELECTED (first Pass) - Stop VM02 Flashing
                            if (myVisionMixer.currentScreen == 2)
                            {
                                myRoomGod.StopFlashingObject(flashVMButton02);
                                prompt11Use1234.SetActive(false);

                                if (distanceFromTitleJump < -1)
                                {
                                    // If Early play 11c (BIT EARLY THERE MATE - NEEDS RECORDING).
                                    myPhone.PlayImmediately(sfx01_11cBitEarlyMate, false);
                                }
                                if (distanceFromTitleJump >= -1 && distanceFromTitleJump <= 1)
                                {
                                    // If On time play 11a (LOVELY MATE)
                                    myPhone.PlayImmediately(sfx01_11aLovelyMate, false);

                                }
                                if (distanceFromTitleJump > 1)
                                {
                                    // If LATE play 11b (Bit Late there mate)
                                    myPhone.PlayImmediately(sfx01_11bBitLateMate, false);

                                }
                                // After playing 11a b or c Set timer to 3 seconds and Jump to 320
                                waitTime = 2;
                                currentStep = 320;
                                firstPass = true;
                            }
                            break;

                        case 320:

                            // If on Hold decrease timer.  When at zero play SFX12.
                            if (myPhone.isOnHold)
                            {
                                waitTime -= Time.deltaTime;
                                if (waitTime <= 0 && firstPass)
                                {
                                    myPhone.UnholdAndPlay(sfx01_12NextThrowIsJeremy, false);
                                    firstPass = false;
                                }
                            }
                            // Wait until Globe vanish MINUS sound file length (3.4 secs to "Now!") then: Play Sound File and Jump to 330.  NO FLASH OR PROMPT - They should know how to do it by now.
                            if (myEventController.myClock >= (74.1f - 3.4f)) // 74.1 is switch back time on video.  3.4 is sound-file run-up.
                            {
                                myPhone.UnholdAndPlay(sfx01_13Select01, false);
                                currentStep = 330;
                            }
                            break;

                        case 330:
                            // IF MORE THAN 10 SECONDS LATE OUT OF NEWS TITLES - Throw Level Fail - Reason: Didn't broadcast the news!
                            distanceFromTitleJump = myEventController.myClock - 74.1f; // 74.1 is the time to switch back to jeremy.
                            if (distanceFromTitleJump >= 5) // 5 secs after player should have switched to Jeremy
                            {
                                myMasterController.FailMidLevel("Failed to switch back to Jeremy.<br>This is a beginner's mistake.<br>Are you drunk, Dave?");
                                tutorialIsActive = false;
                                break;
                            }
                            // if SCREEN 01 SELECTED (first Pass)
                            if (myVisionMixer.currentScreen == 1)
                            {
                                if (distanceFromTitleJump < -1)
                                {
                                    // If Early play 11c (BIT EARLY THERE MATE - NEEDS RECORDING).
                                    myPhone.PlayImmediately(sfx01_11cBitEarlyMate, false);
                                }
                                if (distanceFromTitleJump >= -1 && distanceFromTitleJump <= 1)
                                {
                                    // If On time play 11a (LOVELY MATE)
                                    myPhone.PlayImmediately(sfx01_11aLovelyMate, false);

                                }
                                if (distanceFromTitleJump > 1)
                                {
                                    // If LATE play 11b (Bit Late there mate)
                                    myPhone.PlayImmediately(sfx01_11bBitLateMate, false);

                                }
                                // After playing 11a b or c - Jump to 340
                                waitTime = 2;
                                currentStep = 340;
                                firstPass = true;
                            }

                            break;

                        case 340:

                            // When Interference is about to arrive:
                            if (myEventController.myClock >= 79.1f) // minimum time after jump back to Jeremy to ensure being on hold
                            {
                                // Start SFX 12 (Interference Explained).  Set Timer for first new flash and jump to 350...
                                myPhone.UnholdAndPlay(sfx01_14InterferenceExplained, false);
                                waitTime = 5.1f;
                                currentStep = 350;
                            }
                            break;

                        case 350:
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                myRoomGod.FlashObject(flashFreqControlBox, 5f, 0.5f); // Flash Frequency control box
                                waitTime = 11.6f - 5.1f;
                                currentStep = 360;
                            }
                            break;

                        case 360:
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                myRoomGod.FlashObject(flashFreqControlKnob, -1f, 0.25f); // Flash Frequency control box
                                prompt12Mousewheel.SetActive(true);
                                waitTime = 5;
                                firstPass = true;
                                currentStep = 370;
                            }
                            break;



                        case 370:
                            if (myTower.isTurning && firstPass)
                            {
                                prompt12Mousewheel.SetActive(false);
                                myRoomGod.StopFlashingObject(flashFreqControlKnob);
                                firstPass = false;
                            }
                            if (myEventController.myClock >= 104f)
                            {
                                if (firstPass)
                                {
                                    prompt12Mousewheel.SetActive(false);
                                    myRoomGod.StopFlashingObject(flashFreqControlKnob);
                                    firstPass = false;
                                }
                                myPhone.UnholdAndPlay(sfx01_17ClockExplained, false);
                                waitTime = 6; // Time to Flash Clock
                                currentStep = 380;
                            }
                            break;

                        case 380:
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                myRoomGod.FlashObject(flashClock, 5f, 0.5f); // Flash Frequency control box
                                waitTime = 12f - 6f; // Time to flash Ads button
                                currentStep = 390;
                            }

                            break;

                        case 390:
                            waitTime -= Time.deltaTime;
                            if (waitTime <= 0)
                            {
                                myRoomGod.FlashObject(flashPlayAdBox, -1, 1f); // Flash Play Ad control box
                                currentStep = 400;
                            }
                            break;

                        case 400:

                            if (myEventController.myClock >= (131.3f - 3.5f)) // Time to cut to ads minus count-in audio time
                            {
                                myRoomGod.FlashObject(flashPlayAdBox, -1, 0.2f); // Flash Play Ad Box FASTER
                                myPhone.UnholdAndPlay(sfx01_18CountIntoAd, false);
                                currentStep = 410;
                            }
                            break;

                        case 410:

                            if (myVisionMixer.inPostRoll)
                            {
                                myRoomGod.StopFlashingObject(flashPlayAdBox);
                                currentStep = 500;
                                break;
                            }
                            if (myEventController.myClock > 133f)
                            {
                                myPhone.UnholdAndPlay(sfx01_19AdLate, false);
                                currentStep = 420;
                            }
                            break;

                        case 420:

                            if (myVisionMixer.inPostRoll)
                            {
                                myRoomGod.StopFlashingObject(flashPlayAdBox);
                                currentStep = 500;
                                break;
                            }
                            if (myEventController.myClock > (141.5f - 1.7f)) // End time of broadcast MINUS sound file length
                            {
                                myPhone.UnholdAndPlay(sfx01_20AdVeryLate, true);
                                currentStep = 420;
                            }
                            break;

                        case 500: // FIRST AD BREAK REACHED

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
