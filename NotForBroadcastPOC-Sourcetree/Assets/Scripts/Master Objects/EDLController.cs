using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDLController : MonoBehaviour {

    public List<EditDecision> EDL = new List<EditDecision>();
    public enum EDLModes { Waiting, Recording, Playback};
    //[HideInInspector]
    public EDLModes myMode;

    private float EDLTime;
    private float lastEDLTime;
    private int currentEDLPosition;
    private float broadcastDelay;
    private SequenceController mySequenceController;
    private SoundDesk myMixingDesk;
    private BroadcastTV myBroadcastSystem;


	// Use this for initialization
	void Start () {
        mySequenceController = GetComponent<SequenceController>();
        myMixingDesk = FindObjectOfType<SoundDesk>();
        myBroadcastSystem = FindObjectOfType<BroadcastTV>();
        myMode = EDLModes.Waiting;
        broadcastDelay = mySequenceController.broadcastScreenDelayTime;
    }

    // Update is called once per frame
    void Update()
    {

        EDLTime += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int decisionCount = 1;
            foreach (EditDecision thisEdit in EDL)
            {
                print("Time: " + thisEdit.editTime + "  Decision " + decisionCount + ": " + thisEdit.editType);
                decisionCount++;
            }
        }

        // IF EDL in Recording mode
        if (myMode == EDLModes.Recording)
        {
            // Check list from last actioned position.
            int thisStopPoint = EDL.Count;
            for (int n=currentEDLPosition; n<thisStopPoint; n++)
            {
                // Check event time.  
                EditDecision thisEditDecision = EDL[n];
                float thisEventTime = thisEditDecision.editTime;
                //Debug.Log("Position: " + n + " = "+thisEventTime+" - "+thisEditDecision.editType);
                // If it lies between last time and current time AND is a screen switche or mute:
                
                if (thisEventTime>lastEDLTime && thisEventTime <= EDLTime)
                {
                    // Send command to broadcast TV or SoundDesk
                    //Debug.Log("Sending Command: " + thisEditDecision.editType);
                    // MUTE
                    if (thisEditDecision.editType == EditDecision.EditDecisionType.MuteChannel)
                    {
                        myMixingDesk.MuteBroadcastChannel(thisEditDecision.channelNumber);
                    }
                    // UNMUTE
                    if (thisEditDecision.editType == EditDecision.EditDecisionType.UnMuteChannel)
                    {
                        myMixingDesk.UnMuteBroadcastChannel(thisEditDecision.channelNumber);
                    }
                    // SWITCH SCREEN
                    if (thisEditDecision.editType == EditDecision.EditDecisionType.SwitchScreen)
                    {
                        myBroadcastSystem.ScreenChange(thisEditDecision.channelNumber);
                    }
                    // Set currentposition to n
                    //Debug.Log("Setting Listposition to " + n);
                    currentEDLPosition = n;
                }

            }

        }



        lastEDLTime = EDLTime;
    }


    public void StartRecordingEDL()
    {
        EDL.Clear();
        EDLTime = 0;
        lastEDLTime = 0;
        currentEDLPosition = 0;
        myMode = EDLModes.Recording;
        broadcastDelay = mySequenceController.broadcastScreenDelayTime;
        EDL.Add(new EditDecision(-10f, EditDecision.EditDecisionType.SequenceInfo, mySequenceController.sequenceName));
    }

    public void StartPlayingEDL(List<EditDecision> thisEDL)
    {
        EDLTime = 0;
        EDL = thisEDL;
        myMode = EDLModes.Playback;
    }

    public void AddMute(int thisChannel)
    {
        //Debug.Log("Adding Mute to EDL at "+(EDLTime+broadcastDelay));
        EDL.Add(new EditDecision(EDLTime + broadcastDelay, EditDecision.EditDecisionType.MuteChannel, thisChannel));
    }

    public void AddUnMute(int thisChannel)
    {
        //Debug.Log("Adding UnMute to EDL");
        EDL.Add(new EditDecision(EDLTime + broadcastDelay, EditDecision.EditDecisionType.UnMuteChannel, thisChannel));
    }

    public void AddBleepOn()
    {
        //Debug.Log("Adding BleepOn to EDL");
        EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.BleepOn));
    }

    public void AddBleepOff()
    {
        //Debug.Log("Adding BleepOff to EDL");
        EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.BleepOff));
    }

    public void AddScreenChange(int thisScreen)
    {
        //Debug.Log("Adding Screen Change to EDL at " + (EDLTime + broadcastDelay));
        EDL.Add(new EditDecision(EDLTime + broadcastDelay, EditDecision.EditDecisionType.SwitchScreen, thisScreen));

    }

    public void AddAdvert(string thisAdvertName)
    {
        EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.PlayAd, thisAdvertName));
    }

    public void FinishRecordingEDL()
    {
        // Add any final commands
        // SORT THE LIST BY editTime
        // Save The List under the same name as the sequence in the Player's folder.
        // 
    }



    
    
}
