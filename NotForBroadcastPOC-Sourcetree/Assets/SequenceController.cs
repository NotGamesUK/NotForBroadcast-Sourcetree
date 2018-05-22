using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SequenceController : MonoBehaviour {

    [Tooltip("Number of seconds delay between broadcast screen and master screen")]
    public float broadcastScreenDelayTime;
    public string searchForThis;
    [HideInInspector]
    public int preparedScreensCount = 0;


    private DataStorage myDataStore;
    private int preparedCountTarget;
    private bool waitingForScreens = false;



    // Use this for initialization
    void Start()
    {
        myDataStore = GetComponent<DataStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        // If waiting for screens...
        // If Ad Screen is Prepared: START CLOCK. START AD SCREEN.
        // Invoke StartSequence in Ad-length Seconds minus Run-In Seconds
    }

    public void PrepareSequence(string thisSequenceName)
    {
        int foundPos = -1;
        int thisLength = myDataStore.sequenceData.Length;
        print("Searching For Sequence: " + thisSequenceName);
        for (int n = 0; n < thisLength; n++)
        {
            if (thisSequenceName == myDataStore.sequenceData[n].SequenceName)
            {
                foundPos = n;
                print("FOUND at Position " + foundPos);
            }
        }
        if (foundPos == -1)
        {
            Debug.LogError("Cannot Find Sequence with name: " + thisSequenceName);
            return; // BREAK OUT OF LOAD LOOP EARLY AS THERE IS NO SEQUENCE HERE
        }

        // Set prepared screens count to 0
        // Tell each VM screen to prepare and increment targetcount
        // Get Length of Video for Screen 01
        // Set clock to Ad-Length second countdown - First Ad is always Countdown
        // Prepare Ad (or countdown if at start) on Broadcast screen
        // Tell Broadcast Screens to prepare and increment targetcount
        // Check VM settings. Tell control room screen to prepare selected screen.  Lock VM buttons.  If no screen selected set to Screen 01 as default.
        // Tell Resistance Screen to prepare (and increment targetcount if required)
        // Load Level into Level Controller
        // Tell update to begin monitoring prepared screens count until target is reached - all ready
        // Clear EDL
    }

    void StartSequence()
    {
        // Send GO! to VM Screens
        // Send GO to Control Room Screen
        // Invoke GO! on StartBroadcastScreens with delay of broadcastScreenDelayTime;
        // Set LevelController.LevelHasStarted
        // Send GO to Resistance screen
        // Set clock to sequence length minus overspill
    }

    void StartBroadcastScreens()
    {
        // Send GO to Broadcast Screens
        // Set Mode to follow EDL
    }

    void SequenceOverrun()
    {
        // THIS IS CALLED WHEN SEQUENCE HAS CUT TO BREAK AND AD HAS NOT BEEN PLAYED
        // Sound Alarms (fringe screen in red? flash lights? siren?)
        // If Player does not play in the AD after X Seconds the level is failed
    }

    public void AdvertPlayed(VideoClip thisAdvert)
    {
        // THIS IS CALLED BY VHS CONTROL PANEL WHEN PLAY BUTTON IS PRESSED
        // Switch Broadcast Screen to Advert Mode and play Ad.
    }
}
