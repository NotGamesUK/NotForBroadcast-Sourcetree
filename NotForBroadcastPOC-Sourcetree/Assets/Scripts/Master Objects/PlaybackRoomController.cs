using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackRoomController : MonoBehaviour {

    public Text myFilenameDisplay;
    public AudioSource mySFXPlayer;
    public AudioClip myLeftRightSFX;


    private MasterController myMasterController;
    private int myFileListPosition, maxFileListPosition;
    private string currentFilename;


	// Use this for initialization
	void Start () {
        myMasterController = FindObjectOfType<MasterController>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PrepareList()
    {
        myFileListPosition = 0;
        maxFileListPosition = myMasterController.savedFiles.Count-1;
        if (maxFileListPosition != -1)
        {
            Debug.Log("At Position 0 of " + maxFileListPosition);
            for (int n = 0; n <= maxFileListPosition; n++)
            {
                Debug.Log("FileName " + n + ": " + myMasterController.savedFiles[n]);
            }
            myFileListPosition = maxFileListPosition;
            ParseFilenameAndDisplay(myMasterController.savedFiles[myFileListPosition]);
        }
        else
        {
            ParseFilenameAndDisplay("No Finished\nBroadcasts\nReady To View");
        }
    }

    void ParseFilenameAndDisplay(string thisFileName)
    {
        string thisDisplayFilename = thisFileName.Replace(" BR ", "\n");
        thisDisplayFilename = thisDisplayFilename.Replace("XX", "/");
        thisDisplayFilename = thisDisplayFilename.Replace("XX", "/");
        thisDisplayFilename = thisDisplayFilename.Replace("CC", ":");
        myFilenameDisplay.text = thisDisplayFilename;

    }

    public void MoveUpFileList()
    {
        if (myFileListPosition < maxFileListPosition)
        {
            myFileListPosition++;
            ParseFilenameAndDisplay(myMasterController.savedFiles[myFileListPosition]);
            mySFXPlayer.clip = myLeftRightSFX;
            mySFXPlayer.Play();
        }
    }

    public void MoveDownFileList()
    {
        if (myFileListPosition > 0)
        {
            myFileListPosition--;
            ParseFilenameAndDisplay(myMasterController.savedFiles[myFileListPosition]);
            mySFXPlayer.clip = myLeftRightSFX;
            mySFXPlayer.Play();

        }
    }

}
