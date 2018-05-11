﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSMachineSlot : MonoBehaviour {

    public Material readyToLoad;
    public Material selectedForLoad;

    private MeshRenderer myRenderer;
    private Material defaultMaterial;
    [HideInInspector]
    public VHSPlayer myPlayer;
    private bool isLit;
    public bool isSelected;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = myRenderer.material;
        myPlayer = GetComponentInParent<VHSPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SlotLightOn()
    {
        if (!myPlayer.isLoaded && !myPlayer.isAnimating && myPlayer.hasPower)
        {
            myRenderer.material = readyToLoad;
            isLit = true;
        }
    }

    public void SlotLightOff()
    {
        myRenderer.material = defaultMaterial;
        isLit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLit && myPlayer.hasPower)
        {
            myRenderer.material = selectedForLoad;
            isSelected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLit && myPlayer.hasPower)
        {
            myRenderer.material = readyToLoad;
            isSelected = false;
        }
    }
}