using System.Collections;
using System.Collections.Generic;
using System; //This allows the IComparable Interface
using UnityEngine;


//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class EditDecision : IComparable<EditDecision>
{

    public enum EditDecisionType { PlayAd, LoadSequence, Interference, SwitchScreen, MuteChannel, UnMuteChannel, BleepOn, BleepOff, EndSequence };
    public enum InterfereType { WhiteNoise, Audio, Resistance }

    public float editTime;
    public EditDecisionType editType;
    public int channelNumber;
    public string loadName;
    public InterfereType interferenceType;
    public float interferenceLevel;


    public EditDecision(float thisTime, EditDecisionType thisType, int thisChannel) // SwitchScreen, --MuteChannel, --UnMute Channel;
    {
        editTime = thisTime;
        editType = thisType;
        channelNumber = thisChannel;
    }

    public EditDecision(float thisTime, EditDecisionType thisType, string thisLoadName) // PlayAd, LoadSequence
    {
        editTime = thisTime;
        editType = thisType;
        loadName = thisLoadName;
    }

    public EditDecision(float thisTime, EditDecisionType thisType, InterfereType thisInterfereType, float thisInterfereLevel) // Interference
    {
        editTime = thisTime;
        editType = thisType;
        interferenceType = thisInterfereType;
        interferenceLevel = thisInterfereLevel;
    }

    public EditDecision(float thisTime, EditDecisionType thisType) // --BleepOn, --BleepOff, EndSequence
    {
        editTime = thisTime;
        editType = thisType;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(EditDecision other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (editTime - other.editTime < 0)
        {
            toSendback = -1;
        } else if (editTime - other.editTime>0)
        {
            toSendback = 1;
        }
        return toSendback;
    }
}
