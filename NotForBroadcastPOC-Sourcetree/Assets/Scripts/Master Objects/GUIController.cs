using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class GUIController : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject menuCamera;
    public GameObject playbackCamera;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject succeedMenu;
    public GameObject upgradesMenu;
    public GameObject playbackMenu;
    public GameObject failMenu;
    public GameObject pauseMenu;
    public GameObject paperworkMenu;

    public Image myBlackout;
    public Image myPaperworkImageDisplay;
    public Text myPaperworkTextDisplay;
    public Sprite myFaxPageBackgroundImage;
    public Text myDayDisplay;
    public Text mySegmentDisplay;

    public GameObject retryFloater;
    public Text myRetryCountdownDisplay;
    public Text myRetryGradeDisplay;


    public AudioClip mainMenuMusic;
    public AudioClip pauseMenuMusic;
    public AudioClip succeedMenuMusic;
    public AudioClip upgradeMenuMusic;
    public AudioClip failMenuMusic;
    public AudioClip playbackMenuMusic;
    public AudioClip levelStartSFX;
    
    public Text failMenuReasonForFail;
    public float myStartLevelBlackoutTime=3;

    public static GUIController uniqueGUIController;

    private AudioSource myMusicPlayer;
    private MasterController myMasterController;
    private OptionsController myOptionsController;
    private PlaybackRoomController myPlaybackController;
    private ScoringController myScoringController;
    private GodOfTheRoom myRoomGod;
    private InTray myInTray;
    [HideInInspector]
    public GameObject currentMenu;
    private GameObject currentCamera;
    private CameraMovement freeLookCamera;
    private DevModeObject[] devModeObjects;
    private List<VideoPlayer> pausedVideoPlayers = new List<VideoPlayer>();
    private VideoPlayer[] allVideoPlayers;
    private List<AudioSource> pausedAudioSources = new List<AudioSource>();
    private AudioSource[] allAudioSources;
    private bool paused;
    private bool cameFromMain = true;
    private bool showingRetry;
    private float myRetryCountdown;
    private int currentDay, currentSegment, currentTray;
    

    private void Awake()
    {
        if (uniqueGUIController == null)
        {
            DontDestroyOnLoad(gameObject);
            uniqueGUIController = this;
        }
        else if (uniqueGUIController != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        myMasterController = FindObjectOfType<MasterController>();
        myOptionsController = GetComponent<OptionsController>();
        myPlaybackController = GetComponent<PlaybackRoomController>();
        myScoringController = FindObjectOfType<ScoringController>();
        myRoomGod = FindObjectOfType<GodOfTheRoom>();
        myInTray = FindObjectOfType<InTray>();
        myMusicPlayer = GetComponent<AudioSource>();
        devModeObjects = FindObjectsOfType<DevModeObject>();
        freeLookCamera = FindObjectOfType<CameraMovement>();
        allVideoPlayers = FindObjectsOfType<VideoPlayer>();
        allAudioSources = FindObjectsOfType<AudioSource>();

        // Default to Main Menu:
        gameCamera.SetActive(false);
        menuCamera.SetActive(true);
        playbackCamera.SetActive(false);
        mainMenu.SetActive(true);
        myMusicPlayer.clip = mainMenuMusic;
        myMusicPlayer.loop = true;
        myMusicPlayer.Play();
        currentMenu = mainMenu;
        currentCamera = menuCamera;
        myBlackout.enabled = false;
        myDayDisplay.CrossFadeAlpha(0f, 0.1f, false);
        mySegmentDisplay.CrossFadeAlpha(0f, 0.1f, false);
        paused = false;

    }

    void Update()
    {
        if (showingRetry)
        {
            myRetryCountdown -= Time.deltaTime;
            if (myRetryCountdown > 0)
            {
                myRetryCountdownDisplay.text = Mathf.CeilToInt(myRetryCountdown).ToString();
            }
            else
            {
                showingRetry = false;
                retryFloater.SetActive(false);
            }
        }
    }

    public void ShowRetry(float thisTime)
    {
        if (!showingRetry)
        {
            showingRetry = true;
            myRetryCountdown = thisTime;
            myRetryCountdownDisplay.text = Mathf.CeilToInt(thisTime).ToString();
            myRetryGradeDisplay.text = myScoringController.GetCurrentGrade();
            retryFloater.SetActive(true);
        }
    }

    public void ResetPaperwork()
    {

    }

    public void ShowPaperwork(bool showTopTray)
    {
        paperworkMenu.SetActive(true);
        if (showTopTray)
        {
            TraySelected(1);
        }
        else
        {
            TraySelected(2);
        }
        DisplayCurrentPaperwork();
        myInTray.PlayTrayChangeSFX();

    }

    void DisplayCurrentPaperwork()
    {
        if (currentTray == 1)
        {
            myPaperworkImageDisplay.sprite = myFaxPageBackgroundImage;
            myPaperworkTextDisplay.text = myInTray.ReturnTextFromTray();
        } else
        {
            myPaperworkImageDisplay.sprite = myInTray.ReturnImageFromTray();
            myPaperworkTextDisplay.text = "";
        }
        // Rotate myPaperworkImageDisplay a Random Amount +/- 2 degrees



    }

    public void TraySelected (int thisTray)
    {
        if (thisTray != currentTray)
        {
            myInTray.PlayTrayChangeSFX();

            currentTray = thisTray;
            DisplayCurrentPaperwork();
        }

    }

    public void ChangePaperworkPage(int thisShift)
    {
        if (myInTray.ChangePage(currentTray, thisShift))
        {

            DisplayCurrentPaperwork();
        }

    }

    public void HidePaperwork()
    {
        paperworkMenu.SetActive(false);
    }


    public void StartBroadcast(int thisBroadcast)
    {
        myBlackout.enabled = true;
        BlackoutFader(0.01f, true);
        ChangeMenuTo(null);
        ChangeCameraTo(gameCamera);
        myMusicPlayer.clip = null;
        PlayStartSFX();
        myMasterController.StartBroadcast(thisBroadcast);
    }

    public void ReplaySegment()
    {
        if (showingRetry)
        {
            myRetryCountdown = 0;
        }
        myBlackout.enabled = true;
        BlackoutFader(0.01f, true);
        PlayStartSFX();
        DisplaySegmentNumber();
    }

    public void DisplaySegmentNumber()
    {
        DisplayDay(currentDay);
        mySegmentDisplay.text = "Segment  " + myMasterController.currentSequence;
        //myDayDisplay.color = new Color(0.786f, 0.144f, 0.144f, 0f);
        mySegmentDisplay.CrossFadeAlpha(1f, 3f, false);
    }

    public void FadeSegment()
    {
        FadeDay();
        mySegmentDisplay.CrossFadeAlpha(0f, 3f, false);
        BlackoutFader(5f, false);
    }

    public void StartPlayback()
    {
        ChangeMenuTo(null);
        ChangeMusicTo(null);
    }

    public void GoToOptions()
    {
        ChangeMenuTo(optionsMenu);
    }

    public void GoToMainMenu()
    {
        if (paused)
        {
            Unpause();
            myRoomGod.ResetRoom();
            paused = false;
            myOptionsController.SaveAllOptions();
        }
        if (currentMenu == optionsMenu)
        {
            myOptionsController.SaveAllOptions();
        }
        ChangeCameraTo(menuCamera);
        ChangeMenuTo(mainMenu);
        ChangeMusicTo(mainMenuMusic);
    }

    public void GoToPauseMenu()
    {
        pausedVideoPlayers.Clear();
        pausedAudioSources.Clear();
        foreach (VideoPlayer thisPlayer in allVideoPlayers)
        {
            if (thisPlayer.isPlaying)
            {
                thisPlayer.Pause();
                pausedVideoPlayers.Add(thisPlayer);
            }
        }
        foreach(AudioSource thisPlayer in allAudioSources)
        {
            if (thisPlayer.isPlaying)
            {
                thisPlayer.Pause();
                pausedAudioSources.Add(thisPlayer);
            }
        }
        ChangeMenuTo(pauseMenu);
        ChangeMusicTo(pauseMenuMusic);
        myMusicPlayer.Play();
        Time.timeScale = 0f;
        paused = true;
    }

    public void Unpause()
    {
        foreach (VideoPlayer thisPlayer in pausedVideoPlayers)
        {
            thisPlayer.Play();
        }
        foreach (AudioSource thisPlayer in pausedAudioSources)
        {
            thisPlayer.Play();
        }

        ChangeMenuTo(null);
        ChangeMusicTo(null);
        Time.timeScale = 1f;
        myOptionsController.SaveAllOptions();
        paused = false;
    }

    public void GoToPlayback()
    {
        if (currentMenu == mainMenu) {
            cameFromMain = true;
        } else {
            cameFromMain = false;
        }
        ChangeCameraTo(playbackCamera);
        ChangeMenuTo(playbackMenu);
        ChangeMusicTo(playbackMenuMusic);
        myPlaybackController.PrepareList(cameFromMain);
    }

    public void LeavePlayback()
    {
        ChangeCameraTo(menuCamera);
        ChangeMusicTo(mainMenuMusic);

        if (cameFromMain == true) {
            ChangeMenuTo(mainMenu);
        } else
        {
            // Came from End of Level
            ChangeMenuTo(succeedMenu);
        }

    }

    public void GoToUpgrades()
    {
        ChangeMenuTo(upgradesMenu);
        ChangeMusicTo(upgradeMenuMusic);
    }

    public void GoToSuccess()
    {
        // Initialise Audience Display to animate in.
        // Set Notes
        // Set Grade
        // Set Income
        ChangeCameraTo(menuCamera);
        ChangeMenuTo(succeedMenu);
        ChangeMusicTo(succeedMenuMusic);

    }

    public void GoToFailed(string thisReason)
    {
        // Set Reason for Fail
        failMenuReasonForFail.text = thisReason;
        ChangeCameraTo(menuCamera);
        ChangeMenuTo(failMenu);
        ChangeMusicTo(failMenuMusic);
    }

    public void ReplayLevel()
    {
        //ChangeCameraTo(gameCamera);
        //ChangeMenuTo(null);
        //ChangeMusicTo(null);
        myRoomGod.ResetRoom();
        Debug.Log("RESTART TESTING: Calling Broadcast " + myMasterController.levelCalledFromGUI);
        StartBroadcast(myMasterController.levelCalledFromGUI);
    }

    public void NextLevel()
    {
        // Simple version for Proof of Concept - needs expansion for full version
        ChangeCameraTo(gameCamera);
        ChangeMenuTo(null);
        ChangeMusicTo(null);
        myMasterController.StartBroadcast(2);


    }
    // Update is called once per frame

    void PlayStartSFX()
    {
        myMusicPlayer.loop = false;
        ChangeMusicTo(levelStartSFX);
    }

    public void DisplayDay(int thisDay)
    {
        myDayDisplay.text = "Day  " + thisDay;
        //myDayDisplay.color = new Color(0.786f, 0.144f, 0.144f, 0f);
        myDayDisplay.CrossFadeAlpha(1f, 3f, false);
        currentDay = thisDay;
    }

    public void FadeDay()
    {
        myDayDisplay.CrossFadeAlpha(0f, 3f, false);
        BlackoutFader(5f, false);
    }

    void ChangeMusicTo(AudioClip thisMusic)
    {
        if (myMusicPlayer.clip == levelStartSFX && thisMusic!=levelStartSFX) { myMusicPlayer.loop = true; }
        if (myMusicPlayer.clip != thisMusic || thisMusic == levelStartSFX)
        {
            if (thisMusic)
            {
                myMusicPlayer.clip = thisMusic;
                myMusicPlayer.Play();
            } else
            {
                myMusicPlayer.Stop();
            }
        }
    }

    void ChangeCameraTo (GameObject thisCamera)
    {
        if (currentCamera != thisCamera)
        {
            thisCamera.SetActive(true);
            currentCamera.SetActive(false);
            currentCamera = thisCamera;
        }
    }

    void ChangeMenuTo (GameObject thisMenu)
    {
        if (currentMenu != thisMenu)
        {
            if (thisMenu)
            {
                thisMenu.SetActive(true);
                if (currentMenu)
                {
                    currentMenu.SetActive(false);
                }
                currentMenu = thisMenu;
            }
            else
            {
                currentMenu.SetActive(false);
                currentMenu = null;
            }
        }

    }

    public void DevModeToggle(bool thisSetting)
    {
        myMasterController.inDevMode = thisSetting;
        foreach(DevModeObject thisObject in devModeObjects)
        {
            thisObject.DevModeChange(thisSetting);
        }
    }

    public void FreeLookToggle(bool thisSetting)
    {
        // Turn Freelook on and off
        freeLookCamera.FreeLookToggle(thisSetting);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void BlackoutFader(float thisFadeTime, bool isFadingIn)
    {
        float thisTargetAlpha = 0f;
        if (isFadingIn)
        {
            thisTargetAlpha = 1f;
        }
        myBlackout.CrossFadeAlpha(thisTargetAlpha, thisFadeTime, false);

    }

}
