using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour {

    private GodOfTheRoom myRoomGod;
    public enum RoomControllerState { Dormant, PreRoll, Advert, Sequence }
    [HideInInspector]
    public RoomControllerState myState;
    private float myClock;
    private List<GameEvent> myEventList = new List<GameEvent>();
    [HideInInspector]
    public int currentAdvert, currentSequence, currentListPosition, maxListPosition;
    private bool catchingUp;

	// Use this for initialization
	void Start () {
        myRoomGod = GetComponent<GodOfTheRoom>();
	}
	
	// Update is called once per frame
	void Update () {
        if (myState != RoomControllerState.Dormant)
        {
            myClock += Time.deltaTime;
            if (currentListPosition < maxListPosition)
            {
                while (myEventList[currentListPosition].eventTime <= myClock)
                {
                    GameEvent thisEvent = myEventList[currentListPosition];

                    switch (myState)
                    {
                        case RoomControllerState.PreRoll:
                            if (thisEvent.myTimeframe == GameEvent.TimeFrame.Preroll)
                            {
                                DoEvent(thisEvent);
                            }
                            break;

                        case RoomControllerState.Advert:
                            if (thisEvent.myTimeframe == GameEvent.TimeFrame.Advert)
                            {
                                if (thisEvent.mySequenceOrAdNumber == currentAdvert)
                                {
                                    DoEvent(thisEvent);
                                }
                            }

                            break;

                        case RoomControllerState.Sequence:
                            if (thisEvent.myTimeframe == GameEvent.TimeFrame.Sequence)
                            {
                                if (thisEvent.mySequenceOrAdNumber == currentSequence)
                                {
                                    DoEvent(thisEvent);
                                }
                            }

                            break;

                    }

                    currentListPosition++;
                    if (currentListPosition >= maxListPosition) { break; }
                }
            }
        }
    }

    public void InitialiseAndStartEventReading(List<GameEvent> thisList)
    {
        myEventList.Clear();
        for (int i = 0; i < thisList.Count; i++)
        {
            myEventList.Add(thisList[i]);
            //Debug.Log("Adding Event: " + thisList[i].eventType);
        }
        myEventList.Sort();
        maxListPosition = myEventList.Count;
        myClock = 0;
        currentAdvert = -1;
        currentSequence = 0;
        currentListPosition = 0;
        myState = RoomControllerState.PreRoll;
        catchingUp = false;
    }

    public void SwitchToNextAdvert()
    {

        myClock = 0;
        currentAdvert++;
        //Debug.Log("EVENT CONTROLLER: Switching to Advert " + currentAdvert);

        currentListPosition = 0;
        if (currentAdvert >= 4)
        {
            myState = RoomControllerState.Dormant;
        }
        else
        {
            myState = RoomControllerState.Advert;

        }
        myRoomGod.FadeRoomLights(2.5f, true);
    }

    public void SwitchToNextSequence()
    {
        myClock = 0;
        currentSequence++;
        currentListPosition = 0;
        myState = RoomControllerState.Sequence;
        //Debug.Log("EVENT CONTROLLER: Switching to Sequence " + currentSequence);
        myRoomGod.FadeRoomLights(2f, false);

    }

    void DoEvent(GameEvent thisEvent)
    {
        //Debug.Log("Doing Event " + thisEvent.eventType + " in " + thisEvent.myTimeframe + " " + thisEvent.mySequenceOrAdNumber);
        switch (thisEvent.eventType)
        {
            case GameEvent.EventType.SetPlugs:
                myRoomGod.SetAllPlugs(thisEvent.stringData);
                break;


            case GameEvent.EventType.TripPower:
                myRoomGod.TripSwitchPower(thisEvent.boolData);

                break;


            case GameEvent.EventType.CorridorEvent:


                break;


            case GameEvent.EventType.WindowEvent:


                break;


            case GameEvent.EventType.SendFax:
                if (catchingUp)
                {
                    // Put Fax Straight on In-Tray
                }
                else
                {
                    myRoomGod.SendFax(thisEvent.stringData, thisEvent.floatData);
                }
                break;

            case GameEvent.EventType.RingPhone:


                break;

            case GameEvent.EventType.PopFanSwitch:
                if (!catchingUp)
                {
                    myRoomGod.SetFanSwitch(thisEvent.boolData);
                }
                break;

            case GameEvent.EventType.SetBroadcastVolumeSlider:
                myRoomGod.SetBroadcastVolumeSlider(thisEvent.floatData);

                break;

            case GameEvent.EventType.SetMasterVolumeSlider:
                myRoomGod.SetControlRoomVolumeSlider(thisEvent.floatData);


                break;

            case GameEvent.EventType.EnableObject:
                myRoomGod.EnableObject(thisEvent.objectData, thisEvent.boolData);

                break;

            case GameEvent.EventType.MultiCamScoringOn:
                myRoomGod.SwitchToMulticamMode();

                break;

            case GameEvent.EventType.SingleCamScoringOn:
                myRoomGod.SwitchToSingleCamMode();

                break;

            case GameEvent.EventType.RhythmCamScoringOn:
                // For Full Version
                myRoomGod.SwitchToRhythmCamMode();

                break;

            case GameEvent.EventType.SetTowerDropSpeed:
                myRoomGod.SetTowerDropSpeed(thisEvent.floatData);

                break;

            case GameEvent.EventType.SetTowerTurnSpeed:
                myRoomGod.SetTowerRotationSpeed(thisEvent.floatData);

                break;

            case GameEvent.EventType.SetMaximumPower:
                myRoomGod.SetMaximumPower(thisEvent.floatData);

                break;

            case GameEvent.EventType.SetRoomTemperature:
                myRoomGod.SetRoomTemperature(thisEvent.floatData);

                break;

            case GameEvent.EventType.DropSatelliteDish:
                myRoomGod.DropSatellite();
                break;

            case GameEvent.EventType.ChangeScoringDownWeight:
                myRoomGod.SetDownWeight(thisEvent.floatData);
                break;

            case GameEvent.EventType.ChangeScoringUpWeight:
                myRoomGod.SetUpWeight(thisEvent.floatData);
                break;

            case GameEvent.EventType.SetScoringGodWeight:
                myRoomGod.SetGodWeight(thisEvent.floatData);
                break;

            case GameEvent.EventType.SetVisionMixerLinkSwitch:
                myRoomGod.SetVisionMixerLinkSwitch(thisEvent.boolData);
                break;

            case GameEvent.EventType.ShowTapeOnMixingDesk:
                myRoomGod.ShowTapeOnMixingDesk(thisEvent.boolData);

                break;

            case GameEvent.EventType.SetBlindsToPercentageUp:
                myRoomGod.SetBlinds(thisEvent.floatData);

                break;


        }
    }

    public void DoAllEventsUpTo(int thisSequence)
    {
        Debug.Log("EVENT CONTROLLER: Doing All Events up to end of Sequence " + thisSequence);
        catchingUp = true;
        myState = RoomControllerState.Advert;
        myClock = 0;
        currentSequence = 0;
        currentAdvert = -1;
        currentListPosition = 0;
        DoAllEventsLabelled(GameEvent.TimeFrame.Preroll, 0);
        while (currentSequence < thisSequence)
        {
            currentAdvert++;
            DoAllEventsLabelled(GameEvent.TimeFrame.Advert, currentAdvert);
            currentSequence++;
            DoAllEventsLabelled(GameEvent.TimeFrame.Sequence, currentSequence);
        }



        catchingUp = false;
    }

    void DoAllEventsLabelled(GameEvent.TimeFrame thisTimeFrame, float thisSequenceOrAdNumber)
    {
        Debug.Log("EVENT CONTROLLER: Doing All Events Labelled " + thisTimeFrame + ":" + thisSequenceOrAdNumber);
        for (int n = 0; n < myEventList.Count; n++)
        {
            if (myEventList[n].myTimeframe == thisTimeFrame)
            {
                if (myEventList[n].mySequenceOrAdNumber == thisSequenceOrAdNumber)
                {
                    DoEvent(myEventList[n]);
                }
            }
        }


    }
}
