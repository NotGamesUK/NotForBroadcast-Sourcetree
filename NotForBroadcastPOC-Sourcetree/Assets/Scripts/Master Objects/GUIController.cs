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
    public GameObject upgradesMenu;

    private AudioSource myMusicPlayer;

    private MasterController myMasterController;

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

    }

    public void StartBroadcast(int thisBroadcast)
    {
        mainMenu.SetActive(false);
        myMusicPlayer.Stop();
        gameCamera.SetActive(true);
        menuCamera.SetActive(false);

        myMasterController.TEMPStartGame(thisBroadcast);
    }


    // Update is called once per frame

    void Update ()
    {
		
	}
}
