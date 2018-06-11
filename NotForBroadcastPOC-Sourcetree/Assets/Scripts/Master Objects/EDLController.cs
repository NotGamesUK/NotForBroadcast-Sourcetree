using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDLController : MonoBehaviour {

    public List<EditDecision> EDL = new List<EditDecision>();
    public enum EDLModes { Waiting, Recording, Playback};
    //[HideInInspector]
    public EDLModes myMode;

    private float EDLTime;
    private float broadcastDelay;
    private SequenceController mySequenceController;


	// Use this for initialization
	void Start () {
        mySequenceController = GetComponent<SequenceController>();
        myMode = EDLModes.Waiting;
        broadcastDelay = mySequenceController.broadcastScreenDelayTime;
    }

    public void StartRecordingEDL()
    {
        EDL.Clear();
        EDLTime = 0;
        myMode = EDLModes.Recording;
        broadcastDelay = mySequenceController.broadcastScreenDelayTime;
        EDL.Add(new EditDecision(-10f, EditDecision.EditDecisionType.LoadSequence, mySequenceController.sequenceName));
    }

    public void StartPlayingEDL()
    {
        EDLTime = 0;
        myMode = EDLModes.Playback;
    }

    public void AddMute(int thisChannel)
    {
        Debug.Log("Adding Mute to EDL at "+(EDLTime+broadcastDelay));
        EDL.Add(new EditDecision(EDLTime + broadcastDelay, EditDecision.EditDecisionType.MuteChannel, thisChannel));
    }

    public void AddUnMute(int thisChannel)
    {
        Debug.Log("Adding UnMute to EDL");
        EDL.Add(new EditDecision(EDLTime + broadcastDelay, EditDecision.EditDecisionType.UnMuteChannel, thisChannel));
    }

    public void AddBleepOn()
    {
        Debug.Log("Adding BleepOn to EDL");
        EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.BleepOn));
    }

    public void AddBleepOff()
    {
        Debug.Log("Adding BleepOff to EDL");
        EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.BleepOff));
    }

    public void FinishRecordingEDL()
    {
        // Add any final commands
        // SORT THE LIST BY editTime
        // Save The List under the same name as the sequence in the Player's folder.
        // 
    }


    // Update is called once per frame
    void Update () {

        EDLTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int thisRandom = Random.Range(2, 5);
            if (thisRandom == 1)
            {
                Debug.Log("Adding BleepOn to EDL");
                EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.BleepOn));
            }
            else if(thisRandom == 2)
            {
                Debug.Log("Adding PlayAd to EDL");
                EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.PlayAd, "ThisString"));
            }
            else if (thisRandom == 3)
            {
                Debug.Log("Adding Interference to EDL");
                EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.Interference, EditDecision.InterfereType.WhiteNoise, 20f));
            }
            else if (thisRandom == 4)
            {
                Debug.Log("Adding Switch Screen to EDL");
                EDL.Add(new EditDecision(EDLTime, EditDecision.EditDecisionType.SwitchScreen, 3));
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int decisionCount = 1;
            foreach (EditDecision thisEdit in EDL)
            {
                print("Time: "+thisEdit.editTime +"  Decision " + decisionCount + ": " + thisEdit.editType);
                decisionCount++;
            }
        }
    }

    
    
}
