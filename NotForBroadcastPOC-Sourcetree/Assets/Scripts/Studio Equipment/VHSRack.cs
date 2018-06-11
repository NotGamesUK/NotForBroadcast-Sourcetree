using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSRack : MonoBehaviour {

    public VHSPlayer[] videoPlayers;
    private VHSControlPanel myControlPanel;
    public bool hasPower=false;

    // Use this for initialization
    void Start()
    {
        myControlPanel = GetComponentInChildren<VHSControlPanel>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void PowerOn()
    {
        foreach (VHSPlayer thisPlayer in videoPlayers)
        {
            thisPlayer.myButton.hasPower = true;
            thisPlayer.hasPower = true;
        }
        myControlPanel.PowerOn();
        hasPower = true;
    }

    void PowerOff()
    {
        foreach (VHSPlayer thisPlayer in videoPlayers)
        {
            thisPlayer.myButton.hasPower = false;
            thisPlayer.hasPower = false;
        }
        myControlPanel.PowerOff();
        hasPower = false;
    }

}
