using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GUIController : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject menuCamera;
    public GameObject playbackCamera;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject levelEndMenu;
    public GameObject upgradesMenu;
    public GameObject playbackMenu;

    private AudioSource myMusicPlayer;
    private MasterController myMasterController;
    private GameObject currentMenu;
    private GameObject currentCamera;
    private bool cameFromMain = true;

    private void Start()
    {
        myMasterController = FindObjectOfType<MasterController>();
        myMusicPlayer = GetComponent<AudioSource>();
        gameCamera.SetActive(false);
        menuCamera.SetActive(true);
        playbackCamera.SetActive(false);
        mainMenu.SetActive(true);
        myMusicPlayer.Play();
        upgradesMenu.SetActive(false);
        currentMenu = mainMenu;
        currentCamera = menuCamera;

    }

    public void StartBroadcast(int thisBroadcast)
    {
        mainMenu.SetActive(false);
        myMusicPlayer.Stop();
        gameCamera.SetActive(true);
        menuCamera.SetActive(false);
        currentCamera = gameCamera;
        myMasterController.StartBroadcast(thisBroadcast);
    }

    public void GoToOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        currentMenu = optionsMenu;
    }

    public void GoToMainMenu()
    {
        if (currentCamera != menuCamera)
        {
            menuCamera.SetActive(true);
            currentCamera.SetActive(false);
            currentCamera = menuCamera;

        }
        currentMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = mainMenu;
    }


    public void GoToPlayback()
    {
        if (currentMenu = mainMenu) { cameFromMain = true; } else { cameFromMain = false; }
        currentMenu.SetActive(false);
        playbackMenu.SetActive(true);
        playbackCamera.SetActive(true);
        currentCamera.SetActive(false);
        currentCamera = playbackCamera;
        currentMenu = playbackMenu;
    }

    public void LeavePlayback()
    {
        menuCamera.SetActive(true);
        currentCamera = menuCamera;

        if (cameFromMain == true) {
            mainMenu.SetActive(true);
            currentMenu = mainMenu;
        } else
        {
            // Came from End of Level
            levelEndMenu.SetActive(true);
            currentMenu = levelEndMenu;
        }
        playbackMenu.SetActive(false);
        playbackCamera.SetActive(false);

    }

    public void GoToUpgrades()
    {
        currentMenu.SetActive(false);
        upgradesMenu.SetActive(true);
        currentMenu = upgradesMenu;
    }

    public void ReplayLevel()
    {
        gameCamera.SetActive(true);
        currentMenu.SetActive(false);
        currentCamera.SetActive(false);
        currentMenu = null;
        currentCamera = null;
        myMasterController.StartBroadcast(myMasterController.currentLevel);

    }

    public void NextLevel()
    {
        // Simple version for Proof of Concept - needs expansion for full version
        gameCamera.SetActive(true);
        currentMenu.SetActive(false);
        currentCamera.SetActive(false);
        currentMenu = null;
        currentCamera = null;
        myMasterController.StartBroadcast(2);


    }
    // Update is called once per frame

    void Update ()
    {
		
	}
}
