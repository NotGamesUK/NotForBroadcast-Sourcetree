using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoringData : IComparable<ScoringData>
{

    public enum ScoreType { Video, Audio };
    public enum ScoreColour { Null,Red, Green, Orange };

    public float scoreFrame;
    public ScoreType editType;
    public int channelNumber;
    public ScoreColour scoreColour;

    public ScoringData (float thisFrame, ScoreType thisType, int thisChannel, ScoreColour thisColour) // SwitchScreen, --MuteChannel, --UnMute Channel;
    {
        scoreFrame = thisFrame;
        editType = thisType;
        channelNumber = thisChannel;
        scoreColour = thisColour;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(ScoringData other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (scoreFrame - other.scoreFrame < 0)
        {
            toSendback = -1;
        }
        else if (scoreFrame - other.scoreFrame > 0)
        {
            toSendback = 1;
        }
        return toSendback;
    }
}
