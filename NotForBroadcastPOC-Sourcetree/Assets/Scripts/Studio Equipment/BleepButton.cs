using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleepButton : MonoBehaviour {

    private ButtonAnimating myButton;
    private bool lastCheck;
    private SoundDesk myDesk;

    // Use this for initialization
    void Start()
    {
        myButton = GetComponent<ButtonAnimating>();
        myDesk = FindObjectOfType<SoundDesk>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCheck != myButton.isDepressed)
        {
            lastCheck = myButton.isDepressed;
            if (myButton.hasPower)
            {
                if (lastCheck == true)
                {
                    //Debug.Log("Bleep On.");
                    myDesk.BleepOn();
                }
                else
                {
                    //Debug.Log("Bleep Off.");
                    myDesk.BleepOff();
                }
            }
        }

    }
}
