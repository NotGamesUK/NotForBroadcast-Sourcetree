using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public AudioClip mainMenuMusic;
    public AudioClip pauseMenuMusic;
    public AudioClip succeedMenuMusic;
    public AudioClip upgradeMenuMusic;
    public AudioClip failMenuMusic;
    public AudioClip playbackMenuMusic;

    public Text failMenuReasonForFail;

    public static GUIController uniqueGUIController;

    private AudioSource myMusicPlayer;
    private MasterController myMasterController;
    private OptionsController myOptionsController;
    private PlaybackRoomController myPlaybackController;
    private GameObject currentMenu;
    private GameObject currentCamera;
    private CameraMovement freeLookCamera;
    private DevModeObject[] devModeObjects;

    private bool cameFromMain = true;

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
        myMusicPlayer = GetComponent<AudioSource>();
        devModeObjects = FindObjectsOfType<DevModeObject>();
        freeLookCamera = FindObjectOfType<CameraMovement>();

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


    }

    public void StartBroadcast(int thisBroadcast)
    {
        ChangeMenuTo(null);
        ChangeCameraTo(gameCamera);
        ChangeMusicTo(null);
        myMasterController.StartBroadcast(thisBroadcast);
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
        if (currentMenu == optionsMenu)
        {
            myOptionsController.SaveAllOptions();
        }
        ChangeCameraTo(menuCamera);
        ChangeMenuTo(mainMenu);
        ChangeMusicTo(mainMenuMusic);
    }


    public void GoToPlayback()
    {
        if (currentMenu == mainMenu) { cameFromMain = true; } else { cameFromMain = false; }
        ChangeCameraTo(playbackCamera);
        ChangeMenuTo(playbackMenu);
        ChangeMusicTo(playbackMenuMusic);
        myPlaybackController.PrepareList();
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
        ChangeCameraTo(gameCamera);
        ChangeMenuTo(null);
        ChangeMusicTo(null);
        myMasterController.StartBroadcast(myMasterController.currentLevel);

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

    void ChangeMusicTo(AudioClip thisMusic)
    {
        if (myMusicPlayer.clip != thisMusic)
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

    void Update ()
    {
		
	}
}
