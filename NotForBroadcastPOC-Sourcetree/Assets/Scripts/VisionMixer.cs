using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMixer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ScreenChange(int selectedScreen)
    {
        Debug.Log("Switched to Screen " + selectedScreen);
    }
}
