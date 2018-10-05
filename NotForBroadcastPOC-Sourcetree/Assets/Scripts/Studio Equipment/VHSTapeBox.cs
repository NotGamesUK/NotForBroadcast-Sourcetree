using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSTapeBox : MonoBehaviour {


    public Material mouseOverMaterial;
    public bool isSelected = false;
    public bool isHeld = false;
    public bool isLoaded = false;
    public AudioClip myGrabbedSFX;
    public AudioClip myShelvedSFX;


    private MeshRenderer myRenderer;
    private Material defaultMaterial;
    private VHSMachineSlot[] tapeSlots;
    private VHSTape myTape;
    private AudioSource mySFX;


	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = myRenderer.material;
        tapeSlots = FindObjectsOfType<VHSMachineSlot>();
        myTape = GetComponentInParent<VHSTape>();
        mySFX = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if (!isLoaded && !isHeld)
        {
            myRenderer.material = mouseOverMaterial;
            isSelected = true;
        }

    }

    private void OnMouseExit()
    {
        if (!isLoaded)
        {
            myRenderer.material = defaultMaterial;
            isSelected = false;
        }
    }

    private void OnMouseDown()
    {
        if (isSelected && !isLoaded)
        {
            myRenderer.material = defaultMaterial;
            isHeld = true;
            mySFX.clip = myGrabbedSFX;
            mySFX.Play();
            //Debug.Log("This Tape = " + myTape);
            foreach (VHSMachineSlot thisSlot in tapeSlots)
            {
                if (thisSlot.myPlayer.myTape == myTape)
                {
                    Debug.Log("Looking at Slot " + thisSlot);
                    Debug.Log("Slot Connected to VHS Machine " + thisSlot.myPlayer);

                    Debug.Log("Machine " + thisSlot.myPlayer.myID + " holding tape " + thisSlot.myPlayer.myTape);
                    Debug.Log("Tape " + myTape + " removed from Player " + thisSlot.myPlayer.myID);
                    //thisSlot.myPlayer.myTape = null;
                    //thisSlot.myPlayer.myLoader.myTape = null;
                    thisSlot.myPlayer.myLoader.EjectEndedEarly();
                }
                thisSlot.SlotLightOn();
            }
        }
    }

    private void OnMouseUp()
    {
        if (isHeld && !isLoaded)
        {
            if (isSelected)
            {
                myRenderer.material = mouseOverMaterial;
            }
            isHeld = false;
            foreach (VHSMachineSlot thisSlot in tapeSlots)
            {
                if (thisSlot.isSelected && !isLoaded)
                {
                    //int loadMachine = thisSlot.myPlayer.myID;
                    //Debug.Log("Tape Loading Into Machine " + loadMachine);
                    thisSlot.myPlayer.LoadTape(myTape);
                    myRenderer.material = defaultMaterial;
                    isLoaded = true;
                    isHeld = false;
                }
                thisSlot.SlotLightOff();
            }

            if (!isLoaded)
            {
                SendMessageUpwards("ReturnToShelf");
                mySFX.clip = myShelvedSFX;
                mySFX.Play();
            }
        }
    }
}
