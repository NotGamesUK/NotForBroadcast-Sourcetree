using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSTapeBox : MonoBehaviour {


    public Material mouseOverMaterial;
    public bool isSelected = false;
    public bool isHeld = false;
    public bool isLoaded = false;

    private MeshRenderer myRenderer;
    private Material defaultMaterial;
    private VHSMachineSlot[] tapeSlots;
    private VHSTape myTape;


	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = myRenderer.material;
        tapeSlots = FindObjectsOfType<VHSMachineSlot>();
        myTape = GetComponentInParent<VHSTape>();
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
            foreach (VHSMachineSlot thisSlot in tapeSlots)
            {
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
                if (thisSlot.isSelected)
                {
                    int loadMachine = thisSlot.myPlayer.myID;
                    Debug.Log("Tape Loading Into Machine " + loadMachine);
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
            }
        }
    }
}
