using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //This allows the IComparable Interface

[System.Serializable]
public class GameEvent : IComparable<GameEvent>
{
    public string myName;
    public enum TimeFrame {  Sequence, Advert, Preroll }
    public TimeFrame myTimeframe;
    public int mySequenceOrAdNumber;
    public float eventTime;

    public enum EventType { SetPlugs, TripPower, PopFanSwitch, RingPhone, SendFax, CorridorEvent, WindowEvent, SetMasterVolumeSlider, SetBroadcastVolumeSlider,
        EnableObject, MultiCamScoringOn, SingleCamScoringOn, RhythmCamScoringOn, SetTowerDropSpeed, SetTowerTurnSpeed, SetMaximumPower, SetRoomTemperature, DropSatelliteDish,
        ChangeScoringDownWeight, ChangeScoringUpWeight, SetScoringGodWeight, SetVisionMixerLinkSwitch }
    public EventType eventType;

    public float floatData;
    public string stringData;
    public int integerData;
    public bool boolData;
    public GameObject objectData;

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
