using System.Collections;
using System.Collections.Generic;
using System; //This allows the IComparable Interface
using UnityEngine;


//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class InterferenceLog : IComparable<InterferenceLog>
{

    public enum InterfereType { WhiteNoise, Audio, Resistance }

    public float logTime;
    public InterfereType interferenceType;
    public float interferenceLevel;


    public InterferenceLog(float thisTime, InterfereType thisInterfereType, float thisInterfereLevel) // Interference
    {
        logTime = thisTime;
        interferenceType = thisInterfereType;
        interferenceLevel = thisInterfereLevel;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(InterferenceLog other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (logTime - other.logTime < 0)
        {
            toSendback = -1;
        } else if (logTime - other.logTime > 0)
        {
            toSendback = 1;
        }
        return toSendback;
    }
}
