using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SkySetup : IComparable<SkySetup> {

    public string myName;
    public int levelNumber;
    public Material skyboxMaterial;
    public Vector3 sunRotation;
    public Color lightColour;
    public float lightIntensity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(SkySetup other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (levelNumber - other.levelNumber < 0)
        {
            toSendback = -1;
        }
        else if (levelNumber - other.levelNumber > 0)
        {
            toSendback = 1;
        }
        return toSendback;
    }

}
