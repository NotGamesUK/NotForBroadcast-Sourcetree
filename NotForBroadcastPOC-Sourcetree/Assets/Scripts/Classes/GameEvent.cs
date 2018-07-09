using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //This allows the IComparable Interface

[System.Serializable]
public class GameEvent : IComparable<GameEvent>
{

    public float eventTime;
    public enum EventType { TripPower, PopFanSwitch, RingPhone, SendFax, NextCorridorEvent, NextWindowEvent }
    public EventType eventType;

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(GameEvent other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (eventTime - other.eventTime < 0)
        {
            toSendback = -1;
        }
        else if (eventTime - other.eventTime > 0)
        {
            toSendback = 1;
        }
        return toSendback;
    }

}
