using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SubLevelController : MonoBehaviour {

    [Tooltip ("Number of seconds delay between broadcast screen and master screen")]
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
        if (waitingForScreens)
        {
            // monitor prepared screen count
            // When it reaches target count call StartSequence
        }
    }

    public void PrepareSequence (string thisSequenceName)
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
        if (foundPos==-1)
        {
            Debug.LogError("Cannot Find Sequence with name: " + thisSequenceName);
            return; // BREAK OUT OF LOAD LOOP EARLY AS THERE IS NO SEQUENCE HERE
        }

        // Set prepared screens count to 0
        // Tell each VM screen to prepare and increment targetcount
        // Tell Broadcast Screens to prepare and increment targetcount
        // Check VM settings. Tell control room screen to prepare selected screen.  Lock VM buttons.
        // Tell Resistance Screen to prepare (and increment targetcount if required)
        // Load Level into Level Controller
        // Tell update to begin monitoring prepared screens count until target is reached - all ready

    }

    void StartSequence()
    {
        // Send GO! to VM Screens
        // Invoke GO! on StartBroadcastScreens with delay of broadcastScreenDelayTime;
        // Set LevelController.LevelHasStarted

    }
}
